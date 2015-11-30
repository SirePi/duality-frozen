// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Editor;
using Duality.Input;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.Core.Components;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
	/// <summary>
	/// This Component can be assigned to a Camera in order to detect and manage Input events such as Mouse and
	/// Keyboard events and redirect said events to a BaseInputReceiver.
	/// It allows to specify a default BaseInputReceiver which can receive all enabled events regardless of its
	/// current status, and also to manage InputVisualReceivers in a way that allows the implementation of a basic GUI.
	/// </summary>
	/// <seealso cref="BaseInputReceiver"/>

	[RequiredComponent(typeof(Camera))]
	[EditorHintImage(ResNames.ImageWidgetController)]
	[EditorHintCategory(ResNames.CategoryWidgets)]
	public class WidgetController : Component, ICmpInitializable
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
		protected Widget _currentDialog;

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

		[DontSerialize]
		private Camera _camera;

		/// <summary>
		/// Constructor
		/// </summary>
		public WidgetController()
		{
			LeftMouseKey = Key.Enter;
			RightMouseKey = Key.Unknown;
			MiddleMouseKey = Key.Unknown;

			PreviousWidgetKey = Key.Tab;
			PreviousWidgetKeyModifier = ModifierKeys.Shift;
			NextWidgetKey = Key.Tab;

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
		public Key LeftMouseKey { get; set; }

		/// <summary>
		///
		/// </summary>
		public ModifierKeys LeftMouseKeyModifier { get; set; }

		/// <summary>
		/// [GET / SET] The Keyboard key that will be treated as a Middle Mouse click
		/// </summary>
		public Key MiddleMouseKey { get; set; }

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
		public Key NextWidgetKey { get; set; }

		/// <summary>
		/// [GET / SET] The Keyboard key that will be used to focus on the next widget
		/// </summary>
		public ModifierKeys NextWidgetKeyModifier { get; set; }

		/// <summary>
		/// [GET / SET] The Keyboard key that will be used to focus on the previous widget
		/// </summary>
		public Key PreviousWidgetKey { get; set; }

		/// <summary>
		///
		/// </summary>
		public ModifierKeys PreviousWidgetKeyModifier { get; set; }

		/// <summary>
		/// [GET / SET] The Keyboard key that will be treated as a Right Mouse click
		/// </summary>
		public Key RightMouseKey { get; set; }

		/// <summary>
		///
		/// </summary>
		public ModifierKeys RightMouseKeyModifier { get; set; }

		#region EventHandlers

		[DontSerialize]
		private EventHandler<KeyboardKeyEventArgs> _keyDownEventHandler;

		[DontSerialize]
		private EventHandler<KeyboardKeyEventArgs> _keyUpEventHandler;

		[DontSerialize]
		private EventHandler<MouseButtonEventArgs> _mouseDownEventHandler;

		[DontSerialize]
		private EventHandler<MouseMoveEventArgs> _mouseMoveEventHandler;

		[DontSerialize]
		private EventHandler<MouseButtonEventArgs> _mouseUpEventHandler;

		[DontSerialize]
		private EventHandler<MouseWheelEventArgs> _mouseWheelEventHandler;

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

			if (inWindow is Window)
			{
				_currentDialog = inWindow;
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
		protected virtual void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
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
		protected virtual void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
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
		protected virtual void Mouse_Move(object sender, MouseMoveEventArgs e)
		{
			if (_camera == null)
			{
				_camera = this.GameObj.GetComponent<Camera>();
			}

			if (FocusedElement != null)
			{
				FocusedElement.MouseMove(e);
			}

			_currentMousePosition.X = e.X;
			_currentMousePosition.Y = e.Y;

			IEnumerable<Widget> activeGUIComponents = this.GameObj.ParentScene.ActiveObjects.GetComponents<Widget>();
			IEnumerable<Widget> hoveredGUIComponents = activeGUIComponents.Where(gc => gc.ActiveArea != ActiveArea.None && gc.GetAreaOnScreen(_camera).Contains(_currentMousePosition));

			Widget hgc = null;

			if (hoveredGUIComponents.Count() > 0)
			{
				//hoveredGUIComponents.Min(gc => gc.GameObj.Transform.Pos.Z);
				foreach (Widget w in hoveredGUIComponents
					.OrderBy(gc => gc.IsInOverlay ? 0 : 1)
					.ThenBy(gc => gc.GameObj.Transform.Pos.Z + gc.ZOffset)
					)
				{
					if (hgc == null && w.GetActiveAreaOnScreen(_camera).Contains(_currentMousePosition))
					{
						hgc = w;
					}
				}

				if (hgc != null && _currentDialog != null)
				{
					if (hgc.GameObj.FindAncestorWithComponent<Window>() != _currentDialog.GameObj)
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
		protected virtual void Mouse_WheelChanged(object sender, MouseWheelEventArgs e)
		{
			if (FocusedElement != null)
			{
				FocusedElement.MouseWheel(e);
			}
		}

		private GameObject FindFirstGameObject()
		{
			Widget firstWidget = this.GameObj.ParentScene.FindComponents<Widget>().FirstOrDefault(w => w.PreviousWidget == null && w.NextWidget != null);
			return firstWidget != null ? firstWidget.GameObj : null;
		}

		private bool IsModifierOk(ModifierKeys inModifierKeys)
		{
			return (_modifierKeys == inModifierKeys || (_modifierKeys & inModifierKeys) > ModifierKeys.None);
		}

		protected virtual void OnInit(Component.InitContext context)
		{
		}

		protected virtual void OnShutdown(Component.ShutdownContext context)
		{
		}
	}
}