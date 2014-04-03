// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using Duality.Editor;
using OpenTK;
using FrozenCore.Resources.Widgets;
using System.Collections.Generic;
using OpenTK.Input;
using Duality.Resources;
using Duality.Components;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedCommandGrid : SkinnedWidget
    {
        #region NonSerialized fields

        [NonSerialized]
        private bool _itemsAccessed;

        [NonSerialized]
        protected int _visibleHeight;

        [NonSerialized]
        protected int _visibleWidth;

        [NonSerialized]
        private ushort _leftmostColumn;

        [NonSerialized]
        private Vector2 _gridCellSize;

        [NonSerialized]
        private object _selectedItem;

        [NonSerialized]
        private FormattedText _fText;

        [NonSerialized]
        private ushort _rows;

        [NonSerialized]
        private ushort _columns;

        [NonSerialized]
        private GameObject _highlight;

        [NonSerialized]
        private SkinnedPanel _highlightPanel;

        [NonSerialized]
        private GameObject _scrollbar;

        [NonSerialized]
        protected SkinnedScrollBar _scrollComponent;

        #endregion NonSerialized fields

        private ContentRef<Script> _onLeftClick;
        private ContentRef<Script> _onRightClick;

        private ColorRgba _textColor;
        private object _leftClickArgument;
        private object _rightClickArgument;

        private Vector2 _scrollbarButtonsSize;
        private Vector2 _scrollbarCursorSize;
        private ContentRef<WidgetSkin> _highlightSkin;
        private ContentRef<WidgetSkin> _scrollbarCursorSkin;
        private ContentRef<WidgetSkin> _scrollbarDecreaseButtonSkin;
        private ContentRef<WidgetSkin> _scrollbarIncreaseButtonSkin;
        private ContentRef<WidgetSkin> _scrollbarSkin;

        private Key _keyUp;
        private Key _keyDown;
        private Key _keyLeft;
        private Key _keyRight;

        private List<object> _items;
        private ContentRef<Font> _textFont;

        private Vector4 _itemPadding;

        public Vector4 ItemPadding
        {
            get { return _itemPadding; }
            set { _itemPadding = value; }
        }

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

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_items.Contains(value) && _selectedItem != value)
                {
                    _selectedItem = value;
                }
                else
                {
                    _selectedItem = null;
                }
            }
        }
        public Vector2 ScrollbarButtonsSize
        {
            get { return _scrollbarButtonsSize; }
            set { _scrollbarButtonsSize = value; }
        }

        public Vector2 ScrollbarCursorSize
        {
            get { return _scrollbarCursorSize; }
            set { _scrollbarCursorSize = value; }
        }
        public List<object> Items
        {
            get
            {
                _itemsAccessed = true;
                return _items;
            }
            set
            {
                _itemsAccessed = true;
                _items = value;
            }
        }

        public ContentRef<WidgetSkin> HighlightSkin
        {
            get { return _highlightSkin; }
            set { _highlightSkin = value; }
        }

        public ContentRef<WidgetSkin> ScrollbarCursorSkin
        {
            get { return _scrollbarCursorSkin; }
            set { _scrollbarCursorSkin = value; }
        }

        public ContentRef<WidgetSkin> ScrollbarDecreaseButtonSkin
        {
            get { return _scrollbarDecreaseButtonSkin; }
            set { _scrollbarDecreaseButtonSkin = value; }
        }

        public ContentRef<WidgetSkin> ScrollbarIncreaseButtonSkin
        {
            get { return _scrollbarIncreaseButtonSkin; }
            set { _scrollbarIncreaseButtonSkin = value; }
        }

        public ContentRef<WidgetSkin> ScrollbarSkin
        {
            get { return _scrollbarSkin; }
            set { _scrollbarSkin = value; }
        }

        public object LeftClickArgument
        {
            get { return _leftClickArgument; }
            set { _leftClickArgument = value; }
        }

        public ContentRef<Script> OnLeftClick
        {
            get { return _onLeftClick; }
            set { _onLeftClick = value; }
        }

        public ContentRef<Script> OnRightClick
        {
            get { return _onRightClick; }
            set { _onRightClick = value; }
        }

        public object RightClickArgument
        {
            get { return _rightClickArgument; }
            set { _rightClickArgument = value; }
        }

        public ColorRgba TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public ContentRef<Font> TextFont
        {
            get { return _textFont; }
            set { _textFont = value; }
        }

        public Key KeyMoveUp
        {
            get { return _keyUp; }
            set { _keyUp = value; }
        }

        public Key KeyMoveDown
        {
            get { return _keyDown; }
            set { _keyDown = value; }
        }

        public Key KeyMoveLeft
        {
            get { return _keyLeft; }
            set { _keyLeft = value; }
        }

        public Key KeyMoveRight
        {
            get { return _keyRight; }
            set { _keyRight = value; }
        }

        public SkinnedCommandGrid()
        {
            ActiveArea = Widgets.ActiveArea.None;

            _fText = new FormattedText();
            _textColor = Colors.White;

            _keyLeft = Key.Left;
            _keyRight = Key.Right;
            _keyUp = Key.Up;
            _keyDown = Key.Down;

            _items = new List<object>();
            _rows = 1;
            _columns = 1;
        }

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

        private void AddScrollBar()
        {
            _scrollbar = new GameObject("scrollbar", this.GameObj);

            float scrollbarWidth = Math.Max(ScrollbarButtonsSize.X, ScrollbarCursorSize.X);

            Transform t = _scrollbar.AddComponent<Transform>();
            t.RelativePos = new Vector3(0, Rect.H, 0);
            t.RelativeAngle = - MathF.PiOver2;

            _scrollComponent = new SkinnedScrollBar();
            _scrollComponent.VisibilityGroup = this.VisibilityGroup;
            _scrollComponent.Skin = ScrollbarSkin;
            _scrollComponent.CursorSkin = ScrollbarCursorSkin;
            _scrollComponent.DecreaseButtonSkin = ScrollbarDecreaseButtonSkin;
            _scrollComponent.IncreaseButtonSkin = ScrollbarIncreaseButtonSkin;
            _scrollComponent.ButtonsSize = ScrollbarButtonsSize;
            _scrollComponent.CursorSize = ScrollbarCursorSize;

            _scrollComponent.Rect = Rect.AlignTopLeft(0, 0, scrollbarWidth, Rect.W);
            _scrollComponent.ScrollSpeed = 1;

            _scrollbar.AddComponent<SkinnedScrollBar>(_scrollComponent);
            Scene.Current.AddObject(_scrollbar);
        }

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

                    for (int i = 0; i < _items.Count; i++)
                    {
                        ushort itemColumn = (ushort)MathF.Floor(i / _rows);

                        if (itemColumn != lastColumn)
                        {
                            lastColumn = itemColumn;
                            itemRow = 0;
                        }

                        if (itemColumn >= _leftmostColumn)
                        {
                            _fText.SourceText = _items[i].ToString();
                            Vector3 topLeft = origin + new Vector3(
                                (_gridCellSize.X * (itemColumn - _leftmostColumn)), 
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

        protected override void OnUpdate(float inSecondsPast)
        {
            base.OnUpdate(inSecondsPast);

            if (_itemsAccessed)
            {
                UpdateGrid();
            }

            UpdateHighlight();
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                if (_highlight == null)
                {
                    AddHighlight();
                    AddScrollBar();
                }

                _visibleWidth = (int)Math.Floor(Rect.W - Skin.Res.Border.X - Skin.Res.Border.W);
                _visibleHeight = (int)Math.Floor(Rect.H - Skin.Res.Border.Y - Skin.Res.Border.Z);

                UpdateGrid();
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

                _highlightPanel.Rect = new Rect(_gridCellSize);

                _rows = (ushort)MathF.Floor(_visibleHeight / _gridCellSize.Y);
                if (_rows > 0)
                {
                    _columns = (ushort)MathF.Ceiling(_items.Count / _rows);
                }
            }
            else
            {
                _rows = 0;
                _columns = 0;
            }
        }

        public override void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
            if (Status != WidgetStatus.Disabled)
            {
                if (e.Key == _keyLeft)
                {
                    SelectedIndex -= _rows;
                }
                if (e.Key == _keyRight)
                {
                    SelectedIndex += _rows;
                }
                if (e.Key == _keyUp)
                {
                    SelectedIndex--;
                }
                if (e.Key == _keyDown)
                {
                    SelectedIndex++;
                }
            }
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

        private void UpdateHighlight()
        {
            if (SelectedIndex >= 0)
            {
                int itemColumn = (int)(MathF.Floor(SelectedIndex / _rows));
                int itemRow = SelectedIndex - (itemColumn * _rows);

                Vector3 relativePos = _highlight.Transform.RelativePos;
                relativePos.X = Skin.Res.Border.X + (_gridCellSize.X * itemColumn);
                relativePos.Y = Skin.Res.Border.Y + (_gridCellSize.Y * itemRow);

                _highlight.Transform.RelativePos = relativePos;
            }
        }
    }
}