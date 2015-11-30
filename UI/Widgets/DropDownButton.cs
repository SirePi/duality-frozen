// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using System.Collections.Generic;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
	/// <summary>
	/// A DropdownButton (combobox) Widget
	/// </summary>

	[EditorHintImage(ResNames.ImageDropDownButton)]
	[EditorHintCategory(ResNames.CategoryWidgets)]
	public class DropDownButton : Widget
	{
		#region NonSerialized fields

		[DontSerialize]
		private GameObject _listBox;

		[DontSerialize]
		private ListBox _listBoxComponent;

		[DontSerialize]
		private FormattedText _text;

		#endregion NonSerialized fields

		private int _dropDownHeight;
		private List<object> _items;
		private int _scrollSpeed;
		private ColorRgba _textColor;
		private ContentRef<Font> _textFont;

		private string _customDropDownAppearance;
		private string _customScrollBarAppearance;
		private string _customCursorAppearance;
		private string _customButtonMinusAppearance;
		private string _customButtonPlusAppearance;
		private Vector2 _buttonSize;
		private Vector2 _cursorSize;

		public string CustomDropDownAppearance
		{
			get { return _customDropDownAppearance; }
			set
			{
				_customDropDownAppearance = value;
				_dirtyFlags |= DirtyFlags.Skin;
			}
		}

		public string CustomScrollBarAppearance
		{
			get { return _customScrollBarAppearance; }
			set
			{
				_customScrollBarAppearance = value;
				_dirtyFlags |= DirtyFlags.Skin;
			}
		}

		public string CustomCursorAppearance
		{
			get { return _customCursorAppearance; }
			set
			{
				_customCursorAppearance = value;
				_dirtyFlags |= DirtyFlags.Skin;
			}
		}

		public string CustomButtonMinusAppearance
		{
			get { return _customButtonMinusAppearance; }
			set
			{
				_customButtonMinusAppearance = value;
				_dirtyFlags |= DirtyFlags.Skin;
			}
		}

		public string CustomButtonPlusAppearance
		{
			get { return _customButtonPlusAppearance; }
			set
			{
				_customButtonPlusAppearance = value;
				_dirtyFlags |= DirtyFlags.Skin;
			}
		}

		public Vector2 ButtonSize
		{
			get { return _buttonSize; }
			set { _buttonSize = value; }
		}

		public Vector2 CursorSize
		{
			get { return _cursorSize; }
			set { _cursorSize = value; }
		}

		private string _customHighlightAppearance;

		public string CustomHighlightAppearance
		{
			get { return _customHighlightAppearance; }
			set
			{
				_customHighlightAppearance = value;
				_dirtyFlags |= DirtyFlags.Skin;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public DropDownButton()
		{
			ActiveArea = ActiveArea.RightBorder;

			_items = new List<object>();
			_text = new FormattedText();
			_dropDownHeight = 100;
			_scrollSpeed = 5;

			_dirtyFlags |= DirtyFlags.Value;

			_textColor = Colors.White;
		}

		/// <summary>
		/// [GET / SET] the height of the dropdown Panel when open
		/// </summary>
		public int DropDownHeight
		{
			get { return _dropDownHeight; }
			set
			{
				_dropDownHeight = value;
				if (_listBoxComponent != null)
				{
					_listBoxComponent.Rect = Rect.Align(Alignment.TopLeft, 0, 0, Rect.W, _dropDownHeight);
				}
			}
		}

		/// <summary>
		/// [GET / SET] the list of items that will be added to the Widget
		/// </summary>
		public List<object> Items
		{
			get
			{
				_dirtyFlags |= DirtyFlags.Value;
				return _items;
			}
			set
			{
				_items = value;
				_dirtyFlags |= DirtyFlags.Value;
			}
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
		/// [GET / SET] the Color of the Text
		/// </summary>
		public ColorRgba TextColor
		{
			get { return _textColor; }
			set { _textColor = value; }
		}

		/// <summary>
		/// [GET / SET] the Font of the Text
		/// </summary>
		public ContentRef<Font> TextFont
		{
			get { return _textFont; }
			set { _textFont = value; }
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="e"></param>
		public override void MouseDown(MouseButtonEventArgs e)
		{
			if (Status != WidgetStatus.Disabled)
			{
				if (e.Button == MouseButton.Left)
				{
					_listBox.Active = !_listBox.Active;
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="inCanvas"></param>
		protected override void DrawCanvas(Canvas inCanvas)
		{
			if (_listBoxComponent != null)
			{
				Vector3 buttonLeft = (_points[5].WorldCoords + _points[9].WorldCoords) / 2;

				if (_listBoxComponent.SelectedItem != null)
				{
					if (_textFont.Res != null && _text.Fonts[0] != _textFont)
					{
						_text.Fonts[0] = _textFont;
					}

					_text.SourceText = _listBoxComponent.SelectedItem.ToString();
					inCanvas.PushState();
					inCanvas.State.ColorTint = _textColor;
					inCanvas.State.TransformAngle = GameObj.Transform.Angle;
					inCanvas.DrawText(_text, buttonLeft.X, buttonLeft.Y, buttonLeft.Z + DELTA_Z, null, Alignment.Left);
					inCanvas.PopState();
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		protected override void OnStatusChange()
		{
			base.OnStatusChange();

			if (Status == WidgetStatus.Disabled)
			{
				_listBox.Active = false;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="inSecondsPast"></param>
		protected override void OnUpdate(float inSecondsPast)
		{
			if (_listBox == null)
			{
				AddListBox();
			}

			if ((_dirtyFlags & DirtyFlags.Skin) != DirtyFlags.None)
			{
				if (_listBox != null)
				{
					_listBoxComponent.Skin = Skin;
					_listBoxComponent.CustomWidgetAppearance = _customDropDownAppearance;
					_listBoxComponent.CustomButtonMinusAppearance = _customButtonMinusAppearance;
					_listBoxComponent.CustomButtonPlusAppearance = _customButtonPlusAppearance;
					_listBoxComponent.CustomCursorAppearance = _customCursorAppearance;
					_listBoxComponent.CustomHighlightAppearance = _customHighlightAppearance;
					_listBoxComponent.CustomScrollBarAppearance = _customScrollBarAppearance;
				}
			}

			if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
			{
				_listBoxComponent.Items = Items;
			}
		}

		private void AddListBox()
		{
			_listBox = new GameObject("ListBox", this.GameObj);

			Transform t = _listBox.AddComponent<Transform>();
			t.RelativePos = new Vector3(0, Rect.H, 0);
			t.RelativeAngle = 0;

			_listBoxComponent = new ListBox();
			_listBoxComponent.VisibilityGroup = VisibilityGroup;
			_listBoxComponent.Skin = Skin;
			_listBoxComponent.CustomWidgetAppearance = _customDropDownAppearance;
			_listBoxComponent.CustomButtonMinusAppearance = _customButtonMinusAppearance;
			_listBoxComponent.CustomButtonPlusAppearance = _customButtonPlusAppearance;
			_listBoxComponent.CustomCursorAppearance = _customCursorAppearance;
			_listBoxComponent.CustomHighlightAppearance = _customHighlightAppearance;
			_listBoxComponent.CustomScrollBarAppearance = _customScrollBarAppearance;
			_listBoxComponent.Rect = Rect.Align(Alignment.TopLeft, 0, 0, Rect.W, _dropDownHeight);
			_listBoxComponent.TextFont = TextFont;

			_listBox.AddComponent<ListBox>(_listBoxComponent);
			_listBox.Active = false;

			_listBoxComponent.SelectedItem = null;

			this.GameObj.ParentScene.AddObject(_listBox);
		}
	}
}