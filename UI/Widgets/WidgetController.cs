// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using Duality;
using Duality.Components;
using Duality.Editor;
using Duality.Resources;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.Core.Components;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
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
    [EditorHintImage(typeof(Res), ResNames.ImageWidgetController)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class WidgetController : Component, ICmpInitializable
    {
#pragma warning disable 1591
        protected static readonly OpenTK.Input.MouseButtonEventArgs LEFT_CLICK_DOWN = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Left, true);

        protected static readonly OpenTK.Input.MouseButtonEventArgs LEFT_CLICK_UP = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Left, false);

        protected static readonly OpenTK.Input.MouseButtonEventArgs MIDDLE_CLICK_DOWN = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Middle, true);

        protected static readonly OpenTK.Input.MouseButtonEventArgs MIDDLE_CLICK_UP = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Middle, false);

        protected static readonly OpenTK.Input.MouseButtonEventArgs RIGHT_CLICK_DOWN = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Right, true);

        protected static readonly OpenTK.Input.MouseButtonEventArgs RIGHT_CLICK_UP = new OpenTK.Input.MouseButtonEventArgs(0, 0, OpenTK.Input.MouseButton.Right, false);
#pragma warning restore 1591

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected Widget _currentDialog;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected Vector2 _currentMousePosition;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected InputReceiverVisual _draggedElement;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected Vector2 _lastMousePosition;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected ModifierKeys _modifierKeys;

        /// <summary>
        /// Constructor
        /// </summary>
        public WidgetController()
        {
            LeftMouseKey = OpenTK.Input.Key.Enter;
            RightMouseKey = OpenTK.Input.Key.Unknown;
            MiddleMouseKey = OpenTK.Input.Key.Unknown;

            PreviousWidgetKey = OpenTK.Input.Key.Tab;
            PreviousWidgetKeyModifier = ModifierKeys.Shift;
            NextWidgetKey = OpenTK.Input.Key.Tab;

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
            RShift = 0x20,
            Alt = LAlt | RAlt,
            Control = LControl | RControl,
            Shift = RShift | LShift,
#pragma warning restore 1591
        }

        /// <summary>
        /// [GET] The currently Focused InputVisualReceiver
        /// </summary>
        public Widget FocusedElement { get; private set; }
        /// <summary>
        /// [GET] The currently Hovered InputVisualReceiver
        /// </summary>
        public Widget HoveredElement { get; private set; }
        /// <summary>
        /// [GET / SET] If the Controller should register for Keyboard events
        /// </summary>
        public bool KeyboardEnabled { get; set; }
        /// <summary>
        /// [GET / SET] The Keyboard key that will be treated as a Left Mouse click
        /// </summary>
        public OpenTK.Input.Key LeftMouseKey { get; set; }
        /// <summary>
        ///
        /// </summary>
        public ModifierKeys LeftMouseKeyModifier { get; set; }
        /// <summary>
        /// [GET / SET] The Keyboard key that will be treated as a Middle Mouse click
        /// </summary>
        public OpenTK.Input.Key MiddleMouseKey { get; set; }
        /// <summary>
        ///
        /// </summary>
        public ModifierKeys MiddleMouseKeyModifier { get; set; }
        /// <summary>
        /// [GET / SET] If the Controller should register for Mouse events
        /// </summary>
        public bool MouseEnabled { get; set; }
        /// <summary>
        ///
        /// </summary>
        public OpenTK.Input.Key NextWidgetKey { get; set; }
        /// <summary>
        /// [GET / SET] The Keyboard key that will be used to focus on the next widget
        /// </summary>
        public ModifierKeys NextWidgetKeyModifier { get; set; }
        /// <summary>
        /// [GET / SET] The Keyboard key that will be used to focus on the previous widget
        /// </summary>
        public OpenTK.Input.Key PreviousWidgetKey { get; set; }
        /// <summary>
        ///
        /// </summary>
        public ModifierKeys PreviousWidgetKeyModifier { get; set; }
        /// <summary>
        /// [GET / SET] The Keyboard key that will be treated as a Right Mouse click
        /// </summary>
        public OpenTK.Input.Key RightMouseKey { get; set; }
        /// <summary>
        ///
        /// </summary>
        public ModifierKeys RightMouseKeyModifier { get; set; }

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
        ///
        /// </summary>
        /// <param name="inWidget"></param>
        public void FocusOn(Widget inWidget)
        {
            if (FocusedElement != null)
            {
                FocusedElement.MouseLeave();
            }

            FocusedElement = inWidget;
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

            OnInit(context);
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

            OnShutdown(context);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inWindow"></param>
        public void SetDialogWindow(Widget inWindow)
        {
            _currentDialog = null;

            if (inWindow is SkinnedWindow)
            {
                _currentDialog = inWindow;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            // Modifiers ok, testing keys
            if (e.Key == NextWidgetKey && IsModifierOk(NextWidgetKeyModifier))
            {
                if (FocusedElement == null)
                {
                    GameObject firstObject = FindFirstGameObject();

                    if (firstObject != null)
                    {
                        FocusedElement = firstObject.GetComponent<Widget>();
                        FocusedElement.MouseEnter();
                    }
                }
                else if (FocusedElement.NextWidget != null)
                {
                    FocusedElement.MouseLeave();
                    FocusedElement = FocusedElement.NextWidget;
                    FocusedElement.MouseEnter();
                }
            }
            if (e.Key == PreviousWidgetKey && IsModifierOk(PreviousWidgetKeyModifier))
            {
                if (FocusedElement == null)
                {
                    GameObject firstObject = FindFirstGameObject();

                    if (firstObject != null)
                    {
                        FocusedElement = firstObject.GetComponent<Widget>();
                        FocusedElement.MouseEnter();
                    }
                }
                else if (FocusedElement.PreviousWidget != null)
                {
                    FocusedElement.MouseLeave();
                    FocusedElement = FocusedElement.PreviousWidget;
                    FocusedElement.MouseEnter();
                }
            }

            if (FocusedElement != null)
            {
                if (e.Key == LeftMouseKey && IsModifierOk(LeftMouseKeyModifier))
                {
                    FocusedElement.MouseDown(LEFT_CLICK_DOWN);
                    FocusedElement.MouseUp(LEFT_CLICK_UP);
                }
                else if (e.Key == RightMouseKey && IsModifierOk(RightMouseKeyModifier))
                {
                    FocusedElement.MouseDown(RIGHT_CLICK_DOWN);
                    FocusedElement.MouseUp(RIGHT_CLICK_UP);
                }
                else if (e.Key == MiddleMouseKey && IsModifierOk(MiddleMouseKeyModifier))
                {
                    FocusedElement.MouseDown(MIDDLE_CLICK_DOWN);
                    FocusedElement.MouseUp(MIDDLE_CLICK_UP);
                }

                FocusedElement.KeyDown(e, _modifierKeys);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Mouse_ButtonDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (FocusedElement != HoveredElement)
            {
                if (FocusedElement != null)
                {
                    FocusedElement.Deactivate();
                }

                FocusedElement = HoveredElement;
            }

            if (FocusedElement != null)
            {
                FocusedElement.Activate();
                FocusedElement.MouseDown(e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Mouse_ButtonUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (FocusedElement != null)
            {
                FocusedElement.MouseUp(e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Mouse_Move(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            if (FocusedElement != null)
            {
                FocusedElement.MouseMove(e);
            }

            _currentMousePosition.X = e.X;
            _currentMousePosition.Y = e.Y;

            IEnumerable<Widget> activeGUIComponents = Scene.Current.ActiveObjects.GetComponents<Widget>();
            IEnumerable<Widget> hoveredGUIComponents = activeGUIComponents.Where(gc => gc.ActiveArea != ActiveArea.None && gc.GetAreaOnScreen(GameObj.Camera).Contains(_currentMousePosition));

            Widget hgc = null;

            if (hoveredGUIComponents.Count() > 0)
            {
                float closestZ = hoveredGUIComponents.Min(gc => gc.GameObj.Transform.Pos.Z);
                hgc = hoveredGUIComponents.Where(gc => gc.GameObj.Transform.Pos.Z == closestZ).FirstOrDefault();

                if (hgc != null && !hgc.GetActiveAreaOnScreen(GameObj.Camera).Contains(_currentMousePosition))
                {
                    hgc = null;
                }

                if (hgc != null && _currentDialog != null)
                {
                    if (hgc.GameObj.FindAncestorWithComponent<SkinnedWindow>() != _currentDialog.GameObj)
                    {
                        hgc = null;
                    }
                }
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Mouse_WheelChanged(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            if (FocusedElement != null)
            {
                FocusedElement.MouseWheel(e);
            }
        }

        private GameObject FindFirstGameObject()
        {
            Widget firstWidget = Scene.Current.FindComponents<Widget>().FirstOrDefault(w => w.PreviousWidget == null && w.NextWidget != null);
            return firstWidget != null ? firstWidget.GameObj : null;
        }

        private bool IsModifierOk(ModifierKeys inModifierKeys)
        {
            return (_modifierKeys == inModifierKeys || (_modifierKeys & inModifierKeys) > ModifierKeys.None);
        }

        protected virtual void OnInit(Component.InitContext context) { }
        protected virtual void OnShutdown(Component.ShutdownContext context) { }
    }
}