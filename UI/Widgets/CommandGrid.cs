// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using System;
using System.Collections.Generic;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
	/// <summary>
	/// A SkinnedCommandGrid is a Widget that behaves like the command list of Final Fantasy battle screens.
	/// It can only be controlled by keyboard and needs to be specifically assigned Focus on the WidgetController.
	/// </summary>

	[EditorHintImage(ResNames.ImageCommandGrid)]
	[EditorHintCategory(ResNames.CategoryWidgets)]
	public class CommandGrid : Widget
	{
		#region NonSerialized fields

		[DontSerialize]
		private ushort _columns;

		[DontSerialize]
		private FormattedText _fText;

		[DontSerialize]
		private Vector2 _gridCellSize;

		[DontSerialize]
		private GameObject _highlight;

		[DontSerialize]
		private Panel _highlightPanel;

		[DontSerialize]
		private ushort _rows;

		[DontSerialize]
		private GameObject _scrollbar;

		[DontSerialize]
		private ScrollBar _scrollComponent;

		[DontSerialize]
		private object _selectedItem;

		[DontSerialize]
		private ushort _visibleColumns;

		[DontSerialize]
		private int _visibleHeight;

		[DontSerialize]
		private int _visibleWidth;

		#endregion NonSerialized fields

		private Vector2 _buttonSize;
		private Vector2 _cursorSize;
		private string _customButtonMinusAppearance;
		private string _customButtonPlusAppearance;
		private string _customCursorAppearance;
		private string _customHighlightAppearance;
		private string _customScrollBarAppearance;
		private Vector4 _itemPadding;

		private List<object> _items;

		private Key _keyDown;

		private Key _keyLeft;

		private Key _keyRight;

		private Key _keyUp;

		private object _leftClickArgument;

		private ContentRef<Script> _onLeftClick;

		private ContentRef<Script> _onRightClick;

		private object _rightClickArgument;

		private ColorRgba _textColor;

		private ContentRef<Font> _textFont;

		/// <summary>
		/// Constructor
		/// </summary>
		public CommandGrid()
		{
			ActiveArea = ActiveArea.Center;

			_fText = new FormattedText();
			_textColor = Colors.White;

			_keyLeft = Key.Left;
			_keyRight = Key.Right;
			_keyUp = Key.Up;
			_keyDown = Key.Down;

			_items = new List<object>();
			_rows = 1;
			_columns = 1;

			_dirtyFlags |= DirtyFlags.Value;
		}

		/// <summary>
		/// [GET / SET] The Scrollbar button size
		/// </summary>
		public Vector2 ButtonSize
		{
			get { return _buttonSize; }
			set { _buttonSize = value; }
		}

		/// <summary>
		/// [GET / SET] The Scrollbar cursor size
		/// </summary>
		public Vector2 CursorSize
		{
			get { return _cursorSize; }
			set { _cursorSize = value; }
		}

		/// <summary>
		/// [GET / SET] The custom left button appearance
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
		/// [GET / SET] The custom right button appearance
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
		/// [GET / SET] The custom cursor appearance
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
		/// [GET / SET] The custom highlight Appearance
		/// </summary>
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
		/// [GET / SET] The custom scrollbar appearance
		/// </summary>
		public string CustomScrollBarAppearance
		{
			get { return _customScrollBarAppearance; }
			set
			{
				_customScrollBarAppearance = value;
				_dirtyFlags |= DirtyFlags.Skin;
			}
		}
		/// <summary>
		/// [GET / SET] the padding to apply to each item [x, y, z, w] => [left, top, right, bottom]
		/// </summary>
		public Vector4 ItemPadding
		{
			get { return _itemPadding; }
			set { _itemPadding = value; }
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
		/// [GET / SET] the key to press to move the selected item one place down
		/// </summary>
		public Key KeyMoveDown
		{
			get { return _keyDown; }
			set { _keyDown = value; }
		}

		/// <summary>
		/// [GET / SET] the key to press to move the selected item one place left
		/// </summary>
		public Key KeyMoveLeft
		{
			get { return _keyLeft; }
			set { _keyLeft = value; }
		}

		/// <summary>
		/// [GET / SET] the key to press to move the selected item one place right
		/// </summary>
		public Key KeyMoveRight
		{
			get { return _keyRight; }
			set { _keyRight = value; }
		}

		/// <summary>
		/// [GET / SET] the key to press to move the selected item one place up
		/// </summary>
		public Key KeyMoveUp
		{
			get { return _keyUp; }
			set { _keyUp = value; }
		}

		/// <summary>
		/// [GET / SET]
		/// </summary>
		public object LeftClickArgument
		{
			get { return _leftClickArgument; }
			set { _leftClickArgument = value; }
		}

		/// <summary>
		/// [GET / SET]
		/// </summary>
		public ContentRef<Script> OnLeftClick
		{
			get { return _onLeftClick; }
			set { _onLeftClick = value; }
		}

		/// <summary>
		/// [GET / SET]
		/// </summary>
		public ContentRef<Script> OnRightClick
		{
			get { return _onRightClick; }
			set { _onRightClick = value; }
		}

		/// <summary>
		/// [GET / SET]
		/// </summary>
		public object RightClickArgument
		{
			get { return _rightClickArgument; }
			set { _rightClickArgument = value; }
		}

		/// <summary>
		/// [GET / SET] the index of the Selected item
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public int SelectedIndex
		{
			get { return (_selectedItem != null ? _items.IndexOf(_selectedItem) : -1); }
			set
			{
				if (value >= 0 && value < _items.Count)
				{
					SelectedItem = _items[value];
				}
			}
		}

		/// <summary>
		/// [GET / SET] the Selected item
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public object SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				if (_items.Contains(value))
				{
					_selectedItem = value;
				}
				else
				{
					_selectedItem = null;
				}
			}
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
		/// <param name="k"></param>
		public override void KeyDown(KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
		{
			if (Status != WidgetStatus.Disabled)
			{
				if (e.Key == _keyLeft)
				{
					SelectedIndex = Math.Max(SelectedIndex - _rows, 0);
				}
				if (e.Key == _keyRight)
				{
					SelectedIndex = Math.Min(SelectedIndex + _rows, _items.Count - 1);
				}
				if (e.Key == _keyUp)
				{
					SelectedIndex = Math.Max(SelectedIndex - 1, 0);
				}
				if (e.Key == _keyDown)
				{
					SelectedIndex = Math.Min(SelectedIndex + 1, _items.Count - 1); ;
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="e"></param>
		public override void MouseDown(MouseButtonEventArgs e)
		{
			if (Status != WidgetStatus.Disabled)
			{
				if (e.Button == MouseButton.Right && OnRightClick.Res != null)
				{
					OnRightClick.Res.Execute(this.GameObj, RightClickArgument);
				}
				if (e.Button == MouseButton.Left && OnLeftClick.Res != null)
				{
					Status = WidgetStatus.Active;
					OnLeftClick.Res.Execute(this.GameObj, _leftClickArgument);
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="inCanvas"></param>
		protected override void DrawCanvas(Canvas inCanvas)
		{
			if (_items != null && _items.Count > 0)
			{
				inCanvas.PushState();
				inCanvas.State.ColorTint = _textColor;
				inCanvas.State.TransformAngle = GameObj.Transform.Angle;

				Vector3 origin = _points[5].WorldCoords;

				if (_rows > 0)
				{
					int lastColumn = 0;
					int itemRow = 0;
					int firstColumn = 0;

					if (_scrollComponent != null)
					{
						firstColumn = _scrollComponent.Value;
					}

					for (int i = 0; i < _items.Count; i++)
					{
						ushort itemColumn = (ushort)MathF.Floor(i / _rows);

						if (itemColumn != lastColumn)
						{
							lastColumn = itemColumn;
							itemRow = 0;
						}

						if (itemColumn >= firstColumn && itemColumn < firstColumn + _visibleColumns)
						{
							_fText.SourceText = _items[i].ToString();
							Vector3 topLeft = origin + new Vector3(
								(_gridCellSize.X * (itemColumn - firstColumn)),
								(_gridCellSize.Y * itemRow),
								DELTA_Z);

							inCanvas.DrawText(_fText, topLeft.X + _itemPadding.X, topLeft.Y + _itemPadding.Y, topLeft.Z, null, Alignment.TopLeft);
						}

						itemRow++;
					}
				}

				inCanvas.PopState();
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="inSecondsPast"></param>
		protected override void OnUpdate(float inSecondsPast)
		{
			if (_highlight == null)
			{
				AddHighlight();
			}

			if (_scrollbar == null)
			{
				AddScrollBar();
			}

			if ((_dirtyFlags & DirtyFlags.Skin) != DirtyFlags.None)
			{
				if (_scrollbar != null)
				{
					ScrollBar sb = _scrollbar.GetComponent<ScrollBar>();
					sb.CustomWidgetAppearance = _customScrollBarAppearance;
					sb.CustomButtonMinusAppearance = _customButtonMinusAppearance;
					sb.CustomButtonPlusAppearance = _customButtonPlusAppearance;
					sb.CustomCursorAppearance = _customCursorAppearance;

					sb.ButtonSize = _buttonSize;
					sb.CursorSize = _cursorSize;
				}

				if (_highlight != null)
				{
					_highlight.GetComponent<Panel>().CustomWidgetAppearance = _customHighlightAppearance;
				}

				Vector4 border = Skin.Res.WidgetAppearances[GetAppearanceName()].Border;

				_visibleWidth = (int)Math.Floor(Rect.W - border.X - border.Z);
				_visibleHeight = (int)Math.Floor(Rect.H - border.Y - border.W);
			}

			if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
			{
				UpdateGrid();
			}

			UpdateComponents();

			base.OnUpdate(inSecondsPast);
		}

		private void AddHighlight()
		{
			_highlight = new GameObject("Highlight", this.GameObj);

			Transform t = _highlight.AddComponent<Transform>();
			t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, DELTA_Z / 2);
			t.RelativeAngle = 0;

			_highlightPanel = new Panel();
			_highlightPanel.Status = Status;
			_highlightPanel.VisibilityGroup = VisibilityGroup;
			_highlightPanel.Skin = Skin;
			_highlightPanel.CustomWidgetAppearance = _customHighlightAppearance;
			_highlightPanel.Rect = Rect.Align(Alignment.TopLeft, 0, 0, 0, 0);

			_highlight.AddComponent<Panel>(_highlightPanel);
			this.GameObj.ParentScene.AddObject(_highlight);
		}

		private void AddScrollBar()
		{
			_scrollbar = new GameObject("ScrollBar", this.GameObj);

			float scrollbarWidth = Math.Max(_buttonSize.X, _cursorSize.X);

			Transform t = _scrollbar.AddComponent<Transform>();
			t.RelativePos = new Vector3(0, Rect.H, 0);
			t.RelativeAngle = -MathF.PiOver2;

			_scrollComponent = new ScrollBar();
			_scrollComponent.Status = Status;
			_scrollComponent.VisibilityGroup = VisibilityGroup;
			_scrollComponent.Skin = Skin;
			_scrollComponent.CustomWidgetAppearance = _customScrollBarAppearance;
			_scrollComponent.CustomButtonMinusAppearance = _customButtonMinusAppearance;
			_scrollComponent.CustomButtonPlusAppearance = _customButtonPlusAppearance;
			_scrollComponent.CustomCursorAppearance = _customCursorAppearance;

			_scrollComponent.ButtonSize = _buttonSize;
			_scrollComponent.CursorSize = _cursorSize;

			_scrollComponent.Rect = Rect.Align(Alignment.TopLeft, 0, 0, scrollbarWidth, Rect.W);
			_scrollComponent.ScrollSpeed = 1;

			_scrollbar.AddComponent<ScrollBar>(_scrollComponent);
			this.GameObj.ParentScene.AddObject(_scrollbar);
		}

		private void UpdateComponents()
		{
			if (SelectedIndex >= 0 && _scrollComponent != null)
			{
				_highlight.Active = true;

				int itemColumn = (int)(MathF.Floor(SelectedIndex / _rows));
				int itemRow = SelectedIndex - (itemColumn * _rows);

				if (itemColumn < _scrollComponent.Value || itemColumn > _scrollComponent.Value + _visibleColumns - 1)
				{
					_scrollComponent.Value = itemColumn - 1;
				}

				Vector4 border = Skin.Res.WidgetAppearances[GetAppearanceName()].Border;

				Vector3 relativePos = _highlight.Transform.RelativePos;
				relativePos.X = border.X + (_gridCellSize.X * (itemColumn - _scrollComponent.Value));
				relativePos.Y = border.Y + (_gridCellSize.Y * itemRow);

				_highlight.Transform.RelativePos = relativePos;
				_highlightPanel.Rect = new Rect(_gridCellSize);
			}
			else
			{
				_highlight.Active = false;
			}
		}

		private void UpdateGrid()
		{
			_gridCellSize = Vector2.Zero;

			if (_items != null)
			{
				foreach (object o in _items)
				{
					if (o != null)
					{
						_fText.SourceText = o.ToString();
						if (_textFont.Res != null && _fText.Fonts[0] != _textFont)
						{
							_fText.Fonts[0] = _textFont;
						}

						_gridCellSize.X = Math.Max(_gridCellSize.X, _fText.Size.X + _itemPadding.X + _itemPadding.Z);
						_gridCellSize.Y = Math.Max(_gridCellSize.Y, _fText.Size.Y + _itemPadding.Y + _itemPadding.W);
					}
				}

				_rows = (ushort)MathF.Floor(_visibleHeight / _gridCellSize.Y);
				if (_rows > 0)
				{
					_columns = (ushort)MathF.Ceiling((float)_items.Count / _rows);
					_visibleColumns = (ushort)MathF.Floor(_visibleWidth / _gridCellSize.X);
				}
			}
			else
			{
				_rows = 0;
				_columns = 0;
				_visibleColumns = 0;
			}

			_scrollbar.Active = _visibleColumns < _columns;
			if (_scrollbar.Active)
			{
				_scrollComponent.Minimum = 0;
				_scrollComponent.Maximum = _columns - _visibleColumns;
			}
			else
			{
				_scrollComponent.Minimum = 0;
				_scrollComponent.Maximum = 1;
				_scrollComponent.Value = 0;
			}
		}
	}
}