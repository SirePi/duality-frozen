// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Editor;
using Duality.Input;
using SnowyPeak.Duality.Plugin.Frozen.Core.Properties;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Components
{
    /// <summary>
    /// This Component can be assigned to a Camera in order to detect and manage Input events such as Mouse and
    /// Keyboard events and redirect said events to a BaseInputReceiver.
    /// It allows to specify a default BaseInputReceiver which can receive all enabled events regardless of its
    /// current status, and also to manage InputVisualReceivers in a way that allows the implementation of a basic GUI.
    /// </summary>
    /// <seealso cref="BaseInputReceiver"/>
    [RequiredComponent(typeof(Camera))]
    [EditorHintImage(ResNames.ImageInputController)]
    [EditorHintCategory(ResNames.Category)]
    public class InputController : Component, ICmpInitializable
    {
#pragma warning disable 1591
        protected static readonly MouseButtonEventArgs LEFT_CLICK_DOWN = new MouseButtonEventArgs(DualityApp.Mouse, 0, 0, MouseButton.Left, true);

        protected static readonly MouseButtonEventArgs LEFT_CLICK_UP = new MouseButtonEventArgs(DualityApp.Mouse, 0, 0, MouseButton.Left, false);

        protected static readonly MouseButtonEventArgs MIDDLE_CLICK_DOWN = new MouseButtonEventArgs(DualityApp.Mouse, 0, 0, MouseButton.Middle, true);

        protected static readonly MouseButtonEventArgs MIDDLE_CLICK_UP = new MouseButtonEventArgs(DualityApp.Mouse, 0, 0, MouseButton.Middle, false);

        protected static readonly MouseButtonEventArgs RIGHT_CLICK_DOWN = new MouseButtonEventArgs(DualityApp.Mouse, 0, 0, MouseButton.Right, true);

        protected static readonly MouseButtonEventArgs RIGHT_CLICK_UP = new MouseButtonEventArgs(DualityApp.Mouse, 0, 0, MouseButton.Right, false);
#pragma warning restore 1591

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected Vector2 _currentMousePosition;

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected InputReceiverVisual _draggedElement;

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected Vector2 _lastMousePosition;

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected ModifierKeys _modifierKeys;

        /// <summary>
        /// Constructor
        /// </summary>
        public InputController()
        {
            LeftMouseKey = Key.Enter;
            RightMouseKey = Key.Unknown;
            MiddleMouseKey = Key.Unknown;

            MouseEnabled = true;
            KeyboardEnabled = true;

            _lastMousePosition = new Vector2();
            _currentMousePosition = new Vector2();

            _mouseDownEventHandler = new EventHandler<MouseButtonEventArgs>(Mouse_ButtonDown);
            _mouseUpEventHandler = new EventHandler<MouseButtonEventArgs>(Mouse_ButtonUp);
            _mouseMoveEventHandler = new EventHandler<MouseMoveEventArgs>(Mouse_Move);
            _mouseWheelEventHandler = new EventHandler<MouseWheelEventArgs>(Mouse_WheelChanged);

            _keyUpEventHandler = new EventHandler<KeyboardKeyEventArgs>(Keyboard_KeyUp);
            _keyDownEventHandler = new EventHandler<KeyboardKeyEventArgs>(Keyboard_KeyDown);
        }

        /// <summary>
        ///
        /// </summary>
        [Flags]
        public enum ModifierKeys
        {
#pragma warning disable 1591
            None = 0x00,
            LAlt = 0x01,
            RAlt = 0x02,
            LControl = 0x04,
            RControl = 0x08,
            LShift = 0x10,
            RShift = 0x20
#pragma warning restore 1591
        }

        /// <summary>
        /// [GET / SET] If the Receiver should always be notified of events even if there is a Focused InputVisualReceiver
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
        /// [GET / SET] If the Controller should register for Keyboard events
        /// </summary>
        public bool KeyboardEnabled { get; set; }

        /// <summary>
        /// [GET / SET] The Keyboard key that will be treated as a Left Mouse click
        /// </summary>
        public Key LeftMouseKey { get; set; }

        /// <summary>
        /// [GET / SET] The Keyboard key that will be treated as a Middle Mouse click
        /// </summary>
        public Key MiddleMouseKey { get; set; }

        /// <summary>
        /// [GET / SET] If the Controller should register for Mouse events
        /// </summary>
        public bool MouseEnabled { get; set; }

        /// <summary>
        /// [GET / SET] The default BaseInputReceiver
        /// </summary>
        public BaseInputReceiver Receiver { get; set; }

        /// <summary>
        /// [GET / SET] The Keyboard key that will be treated as a Right Mouse click
        /// </summary>
        public Key RightMouseKey { get; set; }

        #region EventHandlers

        private EventHandler<KeyboardKeyEventArgs> _keyDownEventHandler;
        private EventHandler<KeyboardKeyEventArgs> _keyUpEventHandler;
        private EventHandler<MouseButtonEventArgs> _mouseDownEventHandler;
        private EventHandler<MouseMoveEventArgs> _mouseMoveEventHandler;
        private EventHandler<MouseButtonEventArgs> _mouseUpEventHandler;
        private EventHandler<MouseWheelEventArgs> _mouseWheelEventHandler;

        #endregion EventHandlers

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="inX"></param>
        /// <param name="inY"></param>
        protected void Drag(int inX, int inY)
        {
            if (_draggedElement != null)
            {
                _currentMousePosition.X = inX;
                _currentMousePosition.Y = inY;

                _draggedElement.OnDrag(_currentMousePosition - _lastMousePosition);

                _lastMousePosition.X = inX;
                _lastMousePosition.Y = inY;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.ShiftLeft)
            {
                _modifierKeys |= ModifierKeys.LShift;
            }
            if (e.Key == Key.ShiftRight)
            {
                _modifierKeys |= ModifierKeys.RShift;
            }
            if (e.Key == Key.ControlLeft)
            {
                _modifierKeys |= ModifierKeys.LControl;
            }
            if (e.Key == Key.ControlRight)
            {
                _modifierKeys |= ModifierKeys.RControl;
            }
            if (e.Key == Key.AltLeft)
            {
                _modifierKeys |= ModifierKeys.LAlt;
            }
            if (e.Key == Key.AltRight)
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.ShiftLeft)
            {
                _modifierKeys -= ModifierKeys.LShift;
            }
            if (e.Key == Key.ShiftRight)
            {
                _modifierKeys -= ModifierKeys.RShift;
            }
            if (e.Key == Key.ControlLeft)
            {
                _modifierKeys -= ModifierKeys.LControl;
            }
            if (e.Key == Key.ControlRight)
            {
                _modifierKeys -= ModifierKeys.RControl;
            }
            if (e.Key == Key.AltLeft)
            {
                _modifierKeys -= ModifierKeys.LAlt;
            }
            if (e.Key == Key.AltRight)
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
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

            Mouse_Move(sender, new MouseMoveEventArgs(DualityApp.Mouse, e.X, e.Y, 0, 0));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
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

            Mouse_Move(sender, new MouseMoveEventArgs(DualityApp.Mouse, e.X, e.Y, 0, 0));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            Drag(e.X, e.Y);

            InputReceiverVisual irv = null;
            Component cmp = (Component)GameObj.GetComponent<Camera>().PickRendererAt(e.X, e.Y);

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Mouse_WheelChanged(object sender, MouseWheelEventArgs e)
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

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected bool ShouldNotifyReceiver()
        {
            return (FocusedElement == null || (FocusedElement != null && AlwaysNotifyReceiver));
        }
    }
}