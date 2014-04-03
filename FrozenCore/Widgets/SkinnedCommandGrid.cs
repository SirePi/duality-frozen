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

        #endregion NonSerialized fields

        private ContentRef<Script> _onLeftClick;
        private ContentRef<Script> _onRightClick;

        private ColorRgba _textColor;
        private object _leftClickArgument;
        private object _rightClickArgument;

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

        protected override void DrawCanvas(Canvas inCanvas)
        {
            if (_items != null && _items.Count > 0)
            {
                inCanvas.PushState();
                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;

                UpdateGrid();

                Vector3 origin = _points[5].WorldCoords;

                // calculate rows and columns
                _rows = (ushort)MathF.Ceiling((_points[9].WorldCoords - _points[5].WorldCoords).Length / _gridCellSize.Y);
                if (_rows == 0)
                {
                    _rows = 1;
                }
                _columns = (ushort)MathF.Ceiling(_items.Count / _rows);

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
                        Vector3 topLeft = origin + new Vector3(_gridCellSize.X * (itemColumn - _leftmostColumn), _gridCellSize.Y * itemRow, DELTA_Z);

                        inCanvas.DrawText(_fText, topLeft.X, topLeft.Y, topLeft.Z, null, Alignment.TopLeft);
                    }

                    itemRow++;
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
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);
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

                        _gridCellSize.X = Math.Max(_gridCellSize.X, _fText.Size.X);
                        _gridCellSize.Y = Math.Max(_gridCellSize.Y, _fText.Size.Y);
                    }
                }
            }
        }

        public override void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
            if (Status != WidgetStatus.Disabled)
            {
                if (e.Key == _keyLeft)
                {

                }
                if (e.Key == _keyRight)
                {

                }
                if (e.Key == _keyUp)
                {

                }
                if (e.Key == _keyDown)
                {

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
                Rect selectionRect = _fText.TextMetrics.LineBounds[SelectedIndex];

                Vector3 relativePos = _highlight.Transform.RelativePos;
                relativePos.X = Skin.Res.Border.X;
                relativePos.Y = Skin.Res.Border.Y + selectionRect.Y;

                Rect highlightRect = _highlightPanel.Rect;
                highlightRect.H = selectionRect.H;
                highlightRect.W = Rect.W - Skin.Res.Border.X - Skin.Res.Border.W;

                _highlight.Transform.RelativePos = relativePos;
                _highlightPanel.Rect = highlightRect;
                _highlightPanel.VisibleRect = highlightRect;
            }
        }
    }
}