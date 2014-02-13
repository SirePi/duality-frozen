// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Linq;
using Duality;
using Duality.Components;
using OpenTK;
using FrozenCore.Components;
using Duality.Resources;
using System.Collections.Generic;
using FrozenCore.Widgets.Skin;

namespace FrozenCore.Widgets
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
    public class WidgetController : Component, ICmpInitializable
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

        protected static readonly OpenTK.Input.MouseButtonEventArgs LEFT_CLICK_DOWN = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Left, true);
        protected static readonly OpenTK.Input.MouseButtonEventArgs LEFT_CLICK_UP = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Left, false);
        protected static readonly OpenTK.Input.MouseButtonEventArgs MIDDLE_CLICK_DOWN = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Middle, true);
        protected static readonly OpenTK.Input.MouseButtonEventArgs MIDDLE_CLICK_UP = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Middle, false);
        protected static readonly OpenTK.Input.MouseButtonEventArgs RIGHT_CLICK_DOWN = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Right, true);
        protected static readonly OpenTK.Input.MouseButtonEventArgs RIGHT_CLICK_UP = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Right, false);

        [NonSerialized]
        protected Vector2 _currentMousePosition;

        [NonSerialized]
        protected InputReceiverVisual _draggedElement;

        [NonSerialized]
        protected Vector2 _lastMousePosition;

        [NonSerialized]
        protected ModifierKeys _modifierKeys;

        /// <summary>
        /// [GET] The currently Focused InputVisualReceiver
        /// </summary>
        public Widget FocusedElement { get; private set; }
        /// <summary>
        /// [GET] The currently Hovered InputVisualReceiver
        /// </summary>
        public Widget HoveredElement { get; private set; }
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
        public WidgetController()
        {
            LeftMouseKey = OpenTK.Input.Key.Enter;
            RightMouseKey = OpenTK.Input.Key.Unknown;
            MiddleMouseKey = OpenTK.Input.Key.Unknown;

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
            if (context == Component.InitContext.Activate)
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
            if (context == Component.ShutdownContext.Deactivate)
            {
                DualityApp.Mouse.ButtonDown -= _mouseDownEventHandler;
                DualityApp.Mouse.ButtonUp -= _mouseUpEventHandler;
                DualityApp.Mouse.Move -= _mouseMoveEventHandler;
                DualityApp.Mouse.WheelChanged -= _mouseWheelEventHandler;

                DualityApp.Keyboard.KeyDown -= _keyDownEventHandler;
                DualityApp.Keyboard.KeyUp -= _keyUpEventHandler;
            }
        }

        protected virtual void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
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

                FocusedElement.KeyDown(e, _modifierKeys);
            }
        }

        protected virtual void Keyboard_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
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

            if (FocusedElement != null)
            {
                FocusedElement.KeyUp(e, _modifierKeys);
            }
        }

        protected virtual void Mouse_ButtonDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            FocusedElement = HoveredElement;

            if (FocusedElement != null)
            {
                FocusedElement.MouseDown(e);
            }

            Mouse_Move(sender, new OpenTK.Input.MouseMoveEventArgs(e.X, e.Y, 0, 0));
        }

        protected virtual void Mouse_ButtonUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (FocusedElement != null)
            {
                FocusedElement.MouseUp(e);
            }

            Mouse_Move(sender, new OpenTK.Input.MouseMoveEventArgs(e.X, e.Y, 0, 0));
        }


        protected virtual void Mouse_WheelChanged(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            if (FocusedElement != null)
            {
                FocusedElement.MouseWheel(e);
            }
        }

        protected void Mouse_Move(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            if (FocusedElement != null)
            {
                FocusedElement.MouseMove(e);
            }

            _currentMousePosition.X = e.X;
            _currentMousePosition.Y = e.Y;

            //_currentMousePosition = GameObj.Camera.GetSpaceCoord(_currentMousePosition).Xy;

            IEnumerable<Widget> activeGUIComponents = Scene.Current.ActiveObjects.GetComponents<Widget>();
            IEnumerable<Widget> hoveredGUIComponents = activeGUIComponents.Where(gc => gc.GetActiveAreaOnScreen(GameObj.Camera).Contains(_currentMousePosition));

            Widget hgc = null;

            if (hoveredGUIComponents.Count() > 0)
            {
                float closestZ = hoveredGUIComponents.Min(gc => gc.GameObj.Transform.Pos.Z);
                hgc = hoveredGUIComponents.Where(gc => gc.GameObj.Transform.Pos.Z == closestZ).FirstOrDefault();
            }

            if (HoveredElement != hgc)
            {
                if (HoveredElement != null)
                {
                    HoveredElement.MouseLeave();
                }

                if (hgc != null)
                {
                    hgc.MouseEnter();
                }
            }

            HoveredElement = hgc;
        }
    }
}