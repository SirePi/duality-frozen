// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using OpenTK;

namespace FrozenCore.Components
{
    /// <summary>
    /// This Component can be assigned to a Camera in order to detect and manage Input events such as Mouse and
    /// Keyboard events and redirect said events to a BaseInputReceiver.
    /// It allows to specify a default BaseInputReceiver which can receive all enabled events regardless of its
    /// current status, and also to manage InputVisualReceivers in a way that allows the implementation of a basic GUI.
    /// </summary>
    /// <seealso cref="BaseInputReceiver"/>
    [Serializable]
    [RequiredComponent(typeof(Camera))]
    public class InputController : Component, ICmpInitializable
    {
        [Flags]
        public enum ModifierKeys
        {
            None = 0x00,
            LAlt = 0x01,
            RAlt = 0x02,
            LControl = 0x04,
            RControl = 0x08,
            LShift = 0x10,
            RShift = 0x20
        }

        private static readonly OpenTK.Input.MouseButtonEventArgs LEFT_CLICK_DOWN = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Left, true);
        private static readonly OpenTK.Input.MouseButtonEventArgs LEFT_CLICK_UP = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Left, false);
        private static readonly OpenTK.Input.MouseButtonEventArgs MIDDLE_CLICK_DOWN = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Middle, true);
        private static readonly OpenTK.Input.MouseButtonEventArgs MIDDLE_CLICK_UP = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Middle, false);
        private static readonly OpenTK.Input.MouseButtonEventArgs RIGHT_CLICK_DOWN = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Right, true);
        private static readonly OpenTK.Input.MouseButtonEventArgs RIGHT_CLICK_UP = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Right, false);

        [NonSerialized]
        private Vector2 _currentMousePosition;

        [NonSerialized]
        private InputReceiverVisual _draggedElement;

        [NonSerialized]
        private Vector2 _lastMousePosition;

        [NonSerialized]
        private ModifierKeys _modifierKeys;

        /// <summary>
        /// [GET/SET] If the Receiver should always be notified of events even if there is a Focused InputVisualReceiver
        /// </summary>
        public bool AlwaysNotifyReceiver { get; set; }
        /// <summary>
        /// [GET] The currently Focused InputVisualReceiver
        /// </summary>
        public InputReceiverVisual FocusedElement { get; private set; }
        /// <summary>
        /// [GET] The currently Hovered InputVisualReceiver
        /// </summary>
        public InputReceiverVisual HoveredElement { get; private set; }
        /// <summary>
        /// [GET/SET] If the Controller should register for Keyboard events
        /// </summary>
        public bool KeyboardEnabled { get; set; }
        /// <summary>
        /// [GET/SET] The Keyboard key that will be treated as a Left Mouse click
        /// </summary>
        public OpenTK.Input.Key LeftMouseKey { get; set; }
        /// <summary>
        /// [GET/SET] The Keyboard key that will be treated as a Middle Mouse click
        /// </summary>
        public OpenTK.Input.Key MiddleMouseKey { get; set; }
        /// <summary>
        /// [GET/SET] If the Controller should register for Mouse events
        /// </summary>
        public bool MouseEnabled { get; set; }
        /// <summary>
        /// [GET/SET] The Keyboard key that will move FocusedElement to FocusedElement.NextControl, if not null
        /// </summary>
        public OpenTK.Input.Key NextControlKey { get; set; }
        /// <summary>
        /// [GET/SET] The default BaseInputReceiver
        /// </summary>
        public BaseInputReceiver Receiver { get; set; }
        /// <summary>
        /// [GET/SET] The Keyboard key that will be treated as a Right Mouse click
        /// </summary>
        public OpenTK.Input.Key RightMouseKey { get; set; }

        #region EventHandlers

        [NonSerialized]
        private EventHandler<OpenTK.Input.KeyboardKeyEventArgs> _keyDownEventHandler;

        [NonSerialized]
        private EventHandler<OpenTK.Input.KeyboardKeyEventArgs> _keyUpEventHandler;

        [NonSerialized]
        private EventHandler<OpenTK.Input.MouseButtonEventArgs> _mouseDownEventHandler;

        [NonSerialized]
        private EventHandler<OpenTK.Input.MouseMoveEventArgs> _mouseMoveEventHandler;

        [NonSerialized]
        private EventHandler<OpenTK.Input.MouseButtonEventArgs> _mouseUpEventHandler;

        [NonSerialized]
        private EventHandler<OpenTK.Input.MouseWheelEventArgs> _mouseWheelEventHandler;

        #endregion EventHandlers

        /// <summary>
        /// Constructor
        /// </summary>
        public InputController()
        {
            LeftMouseKey = OpenTK.Input.Key.Enter;
            RightMouseKey = OpenTK.Input.Key.Unknown;
            MiddleMouseKey = OpenTK.Input.Key.Unknown;
            NextControlKey = OpenTK.Input.Key.Tab;

            MouseEnabled = true;
            KeyboardEnabled = true;

            _lastMousePosition = new Vector2();
            _currentMousePosition = new Vector2();

            _mouseDownEventHandler = new EventHandler<OpenTK.Input.MouseButtonEventArgs>(Mouse_ButtonDown);
            _mouseUpEventHandler = new EventHandler<OpenTK.Input.MouseButtonEventArgs>(Mouse_ButtonUp);
            _mouseMoveEventHandler = new EventHandler<OpenTK.Input.MouseMoveEventArgs>(Mouse_Move);
            _mouseWheelEventHandler = new EventHandler<OpenTK.Input.MouseWheelEventArgs>(Mouse_WheelChanged);

            _keyUpEventHandler = new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(Keyboard_KeyUp);
            _keyDownEventHandler = new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(Keyboard_KeyDown);
        }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            if (context == InitContext.Activate)
            {
                if (MouseEnabled)
                {
                    DualityApp.Mouse.ButtonDown += _mouseDownEventHandler;
                    DualityApp.Mouse.ButtonUp += _mouseUpEventHandler;
                    DualityApp.Mouse.Move += _mouseMoveEventHandler;
                    DualityApp.Mouse.WheelChanged += _mouseWheelEventHandler;
                }

                if (KeyboardEnabled)
                {
                    DualityApp.Keyboard.KeyDown += _keyDownEventHandler;
                    DualityApp.Keyboard.KeyUp += _keyUpEventHandler;

                    DualityApp.Keyboard.KeyRepeat = false;
                }
            }
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        {
            if (context == ShutdownContext.Deactivate)
            {
                DualityApp.Mouse.ButtonDown -= _mouseDownEventHandler;
                DualityApp.Mouse.ButtonUp -= _mouseUpEventHandler;
                DualityApp.Mouse.Move -= _mouseMoveEventHandler;
                DualityApp.Mouse.WheelChanged -= _mouseWheelEventHandler;

                DualityApp.Keyboard.KeyDown -= _keyDownEventHandler;
                DualityApp.Keyboard.KeyUp -= _keyUpEventHandler;
            }
        }

        private void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.LShift)
            {
                _modifierKeys |= ModifierKeys.LShift;
            }
            if (e.Key == OpenTK.Input.Key.RShift)
            {
                _modifierKeys |= ModifierKeys.RShift;
            }
            if (e.Key == OpenTK.Input.Key.LControl)
            {
                _modifierKeys |= ModifierKeys.LControl;
            }
            if (e.Key == OpenTK.Input.Key.RControl)
            {
                _modifierKeys |= ModifierKeys.RControl;
            }
            if (e.Key == OpenTK.Input.Key.LAlt)
            {
                _modifierKeys |= ModifierKeys.LAlt;
            }
            if (e.Key == OpenTK.Input.Key.RAlt)
            {
                _modifierKeys |= ModifierKeys.RAlt;
            }

            if (FocusedElement != null)
            {
                if (FocusedElement.ReceiveMouseClicks)
                {
                    if (e.Key == LeftMouseKey)
                    {
                        FocusedElement.MouseDown(LEFT_CLICK_DOWN);
                        FocusedElement.MouseDown(LEFT_CLICK_UP);
                    }
                    else if (e.Key == RightMouseKey)
                    {
                        FocusedElement.MouseDown(RIGHT_CLICK_DOWN);
                        FocusedElement.MouseDown(RIGHT_CLICK_UP);
                    }
                    else if (e.Key == MiddleMouseKey)
                    {
                        FocusedElement.MouseDown(MIDDLE_CLICK_DOWN);
                        FocusedElement.MouseDown(MIDDLE_CLICK_UP);
                    }
                }

                if (e.Key == NextControlKey)
                {
                    if (FocusedElement.NextControl != null)
                    {
                        FocusedElement.MouseLeave();
                        FocusedElement = FocusedElement.NextControl;
                        FocusedElement.MouseEnter();
                    }
                }
                else if (FocusedElement.ReceiveKeys)
                {
                    FocusedElement.KeyDown(e, _modifierKeys);
                }
            }

            if (Receiver != null && ShouldNotifyReceiver())
            {
                Receiver.KeyDown(e, _modifierKeys);
            }
        }

        private void Keyboard_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.LShift)
            {
                _modifierKeys -= ModifierKeys.LShift;
            }
            if (e.Key == OpenTK.Input.Key.RShift)
            {
                _modifierKeys -= ModifierKeys.RShift;
            }
            if (e.Key == OpenTK.Input.Key.LControl)
            {
                _modifierKeys -= ModifierKeys.LControl;
            }
            if (e.Key == OpenTK.Input.Key.RControl)
            {
                _modifierKeys -= ModifierKeys.RControl;
            }
            if (e.Key == OpenTK.Input.Key.LAlt)
            {
                _modifierKeys -= ModifierKeys.LAlt;
            }
            if (e.Key == OpenTK.Input.Key.RAlt)
            {
                _modifierKeys -= ModifierKeys.RAlt;
            }

            if (FocusedElement != null && FocusedElement.ReceiveKeys)
            {
                FocusedElement.KeyUp(e, _modifierKeys);
            }

            if (Receiver != null && ShouldNotifyReceiver())
            {
                Receiver.KeyUp(e, _modifierKeys);
            }
        }

        private void Mouse_ButtonDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            FocusedElement = HoveredElement;

            if (FocusedElement != null && FocusedElement.Draggable)
            {
                _lastMousePosition.X = e.X;
                _lastMousePosition.Y = e.Y;

                _draggedElement = FocusedElement;
            }
            else if (FocusedElement != null && FocusedElement.ReceiveMouseClicks)
            {
                FocusedElement.MouseDown(e);
            }

            if (Receiver != null && ShouldNotifyReceiver())
            {
                Receiver.MouseDown(e);
            }
        }

        private void Mouse_ButtonUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            _draggedElement = null;

            if (FocusedElement != null && FocusedElement.ReceiveMouseClicks)
            {
                FocusedElement.MouseUp(e);
            }

            if (Receiver != null && ShouldNotifyReceiver())
            {
                Receiver.MouseUp(e);
            }
        }

        private void Mouse_Move(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            if (_draggedElement != null)
            {
                _currentMousePosition.X = e.X;
                _currentMousePosition.Y = e.Y;

                _draggedElement.Dragged(_currentMousePosition - _lastMousePosition);

                _lastMousePosition.X = e.X;
                _lastMousePosition.Y = e.Y;
            }
            else
            {
                InputReceiverVisual irv = null;
                Component cmp = (Component)GameObj.Camera.PickRendererAt(e.X, e.Y);

                if (cmp != null)
                {
                    irv = cmp.GameObj.GetComponent<InputReceiverVisual>();
                }

                if (HoveredElement != irv)
                {
                    if (HoveredElement != null)
                    {
                        HoveredElement.MouseLeave();
                    }

                    if (irv != null)
                    {
                        irv.MouseEnter();
                    }
                }

                HoveredElement = irv;
            }
        }

        private void Mouse_WheelChanged(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            if (FocusedElement != null && FocusedElement.ReceiveMouseWheel)
            {
                FocusedElement.MouseWheel(e);
            }

            if (Receiver != null && ShouldNotifyReceiver())
            {
                Receiver.MouseWheel(e);
            }
        }

        private bool ShouldNotifyReceiver()
        {
            return (FocusedElement == null || (FocusedElement != null && AlwaysNotifyReceiver));
        }
    }
}