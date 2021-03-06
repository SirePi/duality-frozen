﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
	/// <summary>
	/// A Scrollbar Widget
	/// </summary>

	[EditorHintImage(ResNames.ImageScrollBar)]
	[EditorHintCategory(ResNames.CategoryWidgets)]
	public class ScrollBar : Widget
	{
		#region NonSerialized fields

		[DontSerialize]
		private GameObject _cursor;

		[DontSerialize]
		private GameObject _decreaseButton;

		[DontSerialize]
		private GameObject _increaseButton;

		#endregion NonSerialized fields

		private Vector2 _buttonSize;
		private Vector2 _cursorSize;
		private string _customButtonMinusAppearance;
		private string _customButtonPlusAppearance;
		private string _customCursorAppearance;
		private int _max;

		private int _min;

		private ContentRef<Script> _onValueChanged;

		private int _scrollSpeed;

		private int _value;

		private object _valueChangedArgument;

		/// <summary>
		/// Constructor
		/// </summary>
		public ScrollBar()
		{
			ActiveArea = ActiveArea.None;

			_min = 0;
			_max = 10;
			_value = _min;
			_scrollSpeed = 1;

			_buttonSize = new Vector2(32);
			_cursorSize = new Vector2(32);
		}

		/// <summary>
		/// [GET / SET] The size of the buttons
		/// </summary>
		public Vector2 ButtonSize
		{
			get { return _buttonSize; }
			set { _buttonSize = value; }
		}

		/// <summary>
		/// [GET / SET] The size of the cursor
		/// </summary>
		public Vector2 CursorSize
		{
			get { return _cursorSize; }
			set { _cursorSize = value; }
		}

		/// <summary>
		/// [GET / SET] The custom appearance of the decrease button
		/// </summary>
		public string CustomButtonMinusAppearance
		{
			get { return _customButtonMinusAppearance; }
			set
			{
				_customButtonMinusAppearance = value;
				_dirtyFlags |= DirtyFlags.Skin;
			}
		}

		/// <summary>
		/// [GET / SET] The custom appearance of the increase button
		/// </summary>
		public string CustomButtonPlusAppearance
		{
			get { return _customButtonPlusAppearance; }
			set
			{
				_customButtonPlusAppearance = value;
				_dirtyFlags |= DirtyFlags.Skin;
			}
		}

		/// <summary>
		/// [GET / SET] The custom appearance of the cursor
		/// </summary>
		public string CustomCursorAppearance
		{
			get { return _customCursorAppearance; }
			set
			{
				_customCursorAppearance = value;
				_dirtyFlags |= DirtyFlags.Skin;
			}
		}
		/// <summary>
		/// [GET / SET] the Maximum value
		/// </summary>
		public int Maximum
		{
			get { return _max; }
			set
			{
				_max = value;
				_dirtyFlags |= DirtyFlags.Value;
			}
		}

		/// <summary>
		/// [GET / SET] the Minimum value
		/// </summary>
		public int Minimum
		{
			get { return _min; }
			set
			{
				_min = value;
				_dirtyFlags |= DirtyFlags.Value;
			}
		}

		/// <summary>
		///
		/// </summary>
		public ContentRef<Script> OnValueChanged
		{
			get { return _onValueChanged; }
			set { _onValueChanged = value; }
		}

		/// <summary>
		/// [GET / SET] the speed, in pixels/second of scrolling
		/// </summary>
		public int ScrollSpeed
		{
			get { return _scrollSpeed; }
			set { _scrollSpeed = value; }
		}

		/// <summary>
		/// [GET / SET] the current Value
		/// </summary>
		public int Value
		{
			get { return _value; }
			set
			{
				if (_value != value)
				{
					_value = value;
					_dirtyFlags |= DirtyFlags.Value;

					if (_onValueChanged.Res != null)
					{
						_onValueChanged.Res.Execute(GameObj, _valueChangedArgument);
					}
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		public object ValueChangedArgument
		{
			private get { return _valueChangedArgument; }
			set { _valueChangedArgument = value; }
		}

		/// <summary>
		///
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		internal float ValueDelta
		{
			get
			{
				float cursorY = _cursorSize.Y;

				float length = Rect.H - (cursorY * 2) - (cursorY);
				return length / (Maximum - Minimum);
			}
		}

		/// <summary>
		///
		/// </summary>
		protected override void OnStatusChange()
		{
			base.OnStatusChange();

			if (_cursor != null)
			{
				_cursor.GetComponent<Widget>().Status = Status;
			}
			if (_decreaseButton != null)
			{
				_decreaseButton.GetComponent<Widget>().Status = Status;
			}
			if (_increaseButton != null)
			{
				_increaseButton.GetComponent<Widget>().Status = Status;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="inSecondsPast"></param>
		protected override void OnUpdate(float inSecondsPast)
		{
			if (_cursor == null)
			{
				AddScrollCursor();
				_dirtyFlags |= DirtyFlags.Value;
			}
			if (_increaseButton == null)
			{
				AddScrollIncreaseButton();
			}
			if (_decreaseButton == null)
			{
				AddScrollDecreaseButton();
			}

			if ((_dirtyFlags & DirtyFlags.Skin) != DirtyFlags.None)
			{
				if (_cursor != null)
				{
					_cursor.GetComponent<ScrollCursor>().Skin = Skin;
					_cursor.GetComponent<ScrollCursor>().CustomWidgetAppearance = _customCursorAppearance;
				}
				if (_decreaseButton != null)
				{
					_decreaseButton.GetComponent<ScrollDecreaseButton>().Skin = Skin;
					_decreaseButton.GetComponent<ScrollDecreaseButton>().CustomWidgetAppearance = _customButtonMinusAppearance;
				}
				if (_increaseButton != null)
				{
					_increaseButton.GetComponent<ScrollIncreaseButton>().Skin = Skin;
					_increaseButton.GetComponent<ScrollIncreaseButton>().CustomWidgetAppearance = _customButtonPlusAppearance;
				}
			}

			if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
			{
				UpdateCursor();
			}

			base.OnUpdate(inSecondsPast);
		}

		private void AddScrollCursor()
		{
			_cursor = new GameObject("ScrollBarCursor", this.GameObj);

			Transform t = _cursor.AddComponent<Transform>();
			t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, 0);
			t.RelativeAngle = 0;

			ScrollCursor sc = new ScrollCursor();
			sc.Status = Status;
			sc.VisibilityGroup = VisibilityGroup;
			sc.Skin = Skin;
			sc.CustomWidgetAppearance = _customCursorAppearance;
			sc.Rect = Rect.Align(Alignment.Center, 0, 0, _cursorSize.X, _cursorSize.Y);

			_cursor.AddComponent<ScrollCursor>(sc);
			this.GameObj.ParentScene.AddObject(_cursor);
		}

		private void AddScrollDecreaseButton()
		{
			_decreaseButton = new GameObject("ScrollBarDecreaseButton", this.GameObj);

			Transform t = _decreaseButton.AddComponent<Transform>();
			t.RelativePos = new Vector3(Rect.W / 2, _buttonSize.Y / 2, 0);
			t.RelativeAngle = 0;

			ScrollDecreaseButton sdb = new ScrollDecreaseButton();
			sdb.Status = Status;
			sdb.VisibilityGroup = VisibilityGroup;
			sdb.Skin = Skin;
			sdb.CustomWidgetAppearance = _customButtonMinusAppearance;
			sdb.Rect = Rect.Align(Alignment.Center, 0, 0, _buttonSize.X, _buttonSize.Y);
			sdb.LeftClickArgument = _scrollSpeed;

			_decreaseButton.AddComponent<ScrollDecreaseButton>(sdb);
			this.GameObj.ParentScene.AddObject(_decreaseButton);
		}

		private void AddScrollIncreaseButton()
		{
			_increaseButton = new GameObject("ScrollBarIncreaseButton", this.GameObj);

			Transform t = _increaseButton.AddComponent<Transform>();
			t.RelativePos = new Vector3(Rect.W / 2, Rect.H - _buttonSize.Y / 2, 0);
			t.RelativeAngle = 0;

			ScrollIncreaseButton sib = new ScrollIncreaseButton();
			sib.Status = Status;
			sib.VisibilityGroup = VisibilityGroup;
			sib.Skin = Skin;
			sib.CustomWidgetAppearance = _customButtonPlusAppearance;
			sib.Rect = Rect.Align(Alignment.Center, 0, 0, _buttonSize.X, _buttonSize.Y);
			sib.LeftClickArgument = _scrollSpeed;

			_increaseButton.AddComponent<ScrollIncreaseButton>(sib);
			this.GameObj.ParentScene.AddObject(_increaseButton);
		}

		private void UpdateCursor()
		{
			if (_cursor != null)
			{
				_value = Math.Min(Value, _max);
				_value = Math.Max(Value, _min);

				float length = Rect.H - (_buttonSize.Y * 2) - (_cursorSize.Y);
				Vector3 direction = _increaseButton.Transform.Pos - _decreaseButton.Transform.Pos;

				Vector3 origin = _decreaseButton.Transform.Pos + (direction / 2) - (direction.Normalized * length / 2);

				_cursor.Transform.Pos = origin + (direction.Normalized * (Value - Minimum) * length / (Maximum - Minimum));
			}
		}
	}
}