// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using OpenTK;
using OpenTK.Input;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A SkinnedCommandGrid is a Widget that behaves like the command list of Final Fantasy battle screens.
    /// It can only be controlled by keyboard and needs to be specifically assigned Focus on the WidgetController.
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageCommandGrid)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class SkinnedCommandGrid : SkinnedWidget
    {
        #region NonSerialized fields

        [NonSerialized]
        private ushort _columns;

        [NonSerialized]
        private FormattedText _fText;

        [NonSerialized]
        private Vector2 _gridCellSize;

        [NonSerialized]
        private GameObject _highlight;

        [NonSerialized]
        private SkinnedPanel _highlightPanel;

        [NonSerialized]
        private ushort _rows;

        [NonSerialized]
        private GameObject _scrollbar;

        [NonSerialized]
        private SkinnedScrollBar _scrollComponent;

        [NonSerialized]
        private object _selectedItem;

        [NonSerialized]
        private ushort _visibleColumns;

        [NonSerialized]
        private int _visibleHeight;

        [NonSerialized]
        private int _visibleWidth;

        #endregion NonSerialized fields

        private ContentRef<WidgetSkin> _highlightSkin;
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
        private Vector2 _scrollbarButtonsSize;
        private Vector2 _scrollbarCursorSize;
        private ContentRef<WidgetSkin> _scrollbarCursorSkin;
        private ContentRef<WidgetSkin> _scrollbarDecreaseButtonSkin;
        private ContentRef<WidgetSkin> _scrollbarIncreaseButtonSkin;
        private ContentRef<WidgetSkin> _scrollbarSkin;
        private ColorRgba _textColor;
        private ContentRef<Font> _textFont;

        /// <summary>
        /// Constructor
        /// </summary>
        public SkinnedCommandGrid()
        {
            ActiveArea = Widgets.ActiveArea.Center;

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
        /// [GET / SET] the Skin used for the Highlight
        /// </summary>
        public ContentRef<WidgetSkin> HighlightSkin
        {
            get { return _highlightSkin; }
            set
            {
                _highlightSkin = value;
                _dirtyFlags |= DirtyFlags.Custom5;
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
        /// [GET / SET] the size of the Scrollbar buttons
        /// </summary>
        public Vector2 ScrollbarButtonsSize
        {
            get { return _scrollbarButtonsSize; }
            set
            {
                _scrollbarButtonsSize = value;
                _dirtyFlags |= DirtyFlags.Custom7;
            }
        }
        /// <summary>
        /// [GET / SET] the size of the Scrollbar cursor
        /// </summary>
        public Vector2 ScrollbarCursorSize
        {
            get { return _scrollbarCursorSize; }
            set
            {
                _scrollbarCursorSize = value;
                _dirtyFlags |= DirtyFlags.Custom6;
            }
        }
        /// <summary>
        /// [GET / SET] the Skin used for the Scrollbar Cursor
        /// </summary>
        public ContentRef<WidgetSkin> ScrollbarCursorSkin
        {
            get { return _scrollbarCursorSkin; }
            set
            {
                _scrollbarCursorSkin = value;
                _dirtyFlags |= DirtyFlags.Custom2;
            }
        }
        /// <summary>
        /// [GET / SET] the Skin that will be used for the Scrollbar Decrease button
        /// </summary>
        public ContentRef<WidgetSkin> ScrollbarDecreaseButtonSkin
        {
            get { return _scrollbarDecreaseButtonSkin; }
            set
            {
                _scrollbarDecreaseButtonSkin = value;
                _dirtyFlags |= DirtyFlags.Custom3;
            }
        }
        /// <summary>
        /// [GET / SET] the Skin that will be used for the Scrollbar Increase button
        /// </summary>
        public ContentRef<WidgetSkin> ScrollbarIncreaseButtonSkin
        {
            get { return _scrollbarIncreaseButtonSkin; }
            set
            {
                _scrollbarIncreaseButtonSkin = value;
                _dirtyFlags |= DirtyFlags.Custom4;
            }
        }
        /// <summary>
        /// [GET / SET] the Skin that will be used for the Scrollbar
        /// </summary>
        public ContentRef<WidgetSkin> ScrollbarSkin
        {
            get { return _scrollbarSkin; }
            set
            {
                _scrollbarSkin = value;
                _dirtyFlags |= DirtyFlags.Custom1;
            }
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
        public override void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
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
        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (Status != WidgetStatus.Disabled)
            {
                if (e.Button == OpenTK.Input.MouseButton.Right && OnRightClick.Res != null)
                {
                    OnRightClick.Res.Execute(this.GameObj, RightClickArgument);
                }
                if (e.Button == OpenTK.Input.MouseButton.Left && OnLeftClick.Res != null)
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
            if (_highlight == null && _highlightSkin != null)
            {
                AddHighlight();
            }

            if (_scrollbar == null && _scrollbarSkin != null)
            {
                AddScrollBar();
            }

            if ((_dirtyFlags & DirtyFlags.Skin) != DirtyFlags.None)
            {
                _visibleWidth = (int)Math.Floor(Rect.W - Skin.Res.Border.X - Skin.Res.Border.W);
                _visibleHeight = (int)Math.Floor(Rect.H - Skin.Res.Border.Y - Skin.Res.Border.Z);
            }
            if ((_dirtyFlags & DirtyFlags.Custom1) != DirtyFlags.None && _scrollbar != null)
            {
                _scrollbar.GetComponent<SkinnedWidget>().Skin = _scrollbarSkin;
            }
            if ((_dirtyFlags & DirtyFlags.Custom2) != DirtyFlags.None && _scrollbar != null)
            {
                _scrollbar.GetComponent<SkinnedScrollBar>().CursorSkin = _scrollbarCursorSkin;
            }
            if ((_dirtyFlags & DirtyFlags.Custom3) != DirtyFlags.None && _scrollbar != null)
            {
                _scrollbar.GetComponent<SkinnedScrollBar>().DecreaseButtonSkin = _scrollbarDecreaseButtonSkin;
            }
            if ((_dirtyFlags & DirtyFlags.Custom4) != DirtyFlags.None && _scrollbar != null)
            {
                _scrollbar.GetComponent<SkinnedScrollBar>().IncreaseButtonSkin = _scrollbarIncreaseButtonSkin;
            }
            if ((_dirtyFlags & DirtyFlags.Custom5) != DirtyFlags.None && _highlight != null)
            {
                _highlight.GetComponent<SkinnedWidget>().Skin = _highlightSkin;
            }

            if ((_dirtyFlags & DirtyFlags.Custom6) != DirtyFlags.None && _scrollbar != null)
            {
                _scrollbar.GetComponent<SkinnedScrollBar>().CursorSize = _scrollbarCursorSize;
            }
            if ((_dirtyFlags & DirtyFlags.Custom7) != DirtyFlags.None && _scrollbar != null)
            {
                _scrollbar.GetComponent<SkinnedScrollBar>().ButtonsSize = _scrollbarButtonsSize;
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
            _highlight = new GameObject("highlight", this.GameObj);

            Transform t = _highlight.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, DELTA_Z / 2);
            t.RelativeAngle = 0;

            _highlightPanel = new SkinnedPanel();
            _highlightPanel.VisibilityGroup = this.VisibilityGroup;
            _highlightPanel.Skin = HighlightSkin;
            _highlightPanel.Rect = Rect.AlignTopLeft(0, 0, 0, 0);
            _highlightPanel.OverrideAutomaticZ = true;

            _highlight.AddComponent<SkinnedPanel>(_highlightPanel);
            Scene.Current.AddObject(_highlight);
        }

        private void AddScrollBar()
        {
            _scrollbar = new GameObject("scrollbar", this.GameObj);

            float scrollbarWidth = Math.Max(ScrollbarButtonsSize.X, ScrollbarCursorSize.X);

            Transform t = _scrollbar.AddComponent<Transform>();
            t.RelativePos = new Vector3(0, Rect.H, 0);
            t.RelativeAngle = -MathF.PiOver2;

            _scrollComponent = new SkinnedScrollBar();
            _scrollComponent.VisibilityGroup = this.VisibilityGroup;
            _scrollComponent.Skin = ScrollbarSkin;
            _scrollComponent.CursorSkin = ScrollbarCursorSkin;
            _scrollComponent.DecreaseButtonSkin = ScrollbarDecreaseButtonSkin;
            _scrollComponent.IncreaseButtonSkin = ScrollbarIncreaseButtonSkin;
            _scrollComponent.ButtonsSize = ScrollbarButtonsSize;
            _scrollComponent.CursorSize = ScrollbarCursorSize;
            //_scrollComponent.OnValueChanged = InternalScripts.GetScript<InternalScripts.CommandGridScrollbarValueChanged>();

            _scrollComponent.Rect = Rect.AlignTopLeft(0, 0, scrollbarWidth, Rect.W);
            _scrollComponent.ScrollSpeed = 1;

            _scrollbar.AddComponent<SkinnedScrollBar>(_scrollComponent);
            Scene.Current.AddObject(_scrollbar);
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

                Vector3 relativePos = _highlight.Transform.RelativePos;
                relativePos.X = Skin.Res.Border.X + (_gridCellSize.X * (itemColumn - _scrollComponent.Value));
                relativePos.Y = Skin.Res.Border.Y + (_gridCellSize.Y * itemRow);

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