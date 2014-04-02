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
        private FormattedText _fText;

        [NonSerialized]
        private ushort _rows;

        [NonSerialized]
        private ushort _columns;

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
            if (_items.Count > 0)
            {
                inCanvas.PushState();
                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;

                Vector3 origin = _points[5].WorldCoords;

                int row = 0;
                for (int i = 0; i < _items.Count; i++ )
                {
                    ushort itemColumn = (ushort)MathF.Ceiling(i / _rows);

                    if (itemColumn >= _leftmostColumn)
                    {
                        _fText.SourceText = _items[i].ToString();
                        Vector3 topLeft = origin + new Vector3(_gridCellSize.X * row, _gridCellSize.Y * (itemColumn - _leftmostColumn), DELTA_Z);

                        inCanvas.DrawText(_fText, topLeft.X, topLeft.Y, topLeft.Z, null, Alignment.TopLeft);
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
                _gridCellSize = Vector2.Zero;

                foreach (object o in _items)
                {
                    if (o != null)
                    {
                        _fText.SourceText = o.ToString();

                        _gridCellSize.X = Math.Max(_gridCellSize.X, _fText.Size.X);
                        _gridCellSize.Y = Math.Max(_gridCellSize.Y, _fText.Size.Y);
                    }
                }

                // calculate rows and columns
                _rows = (ushort)MathF.Floor((_points[9].WorldCoords - _points[5].WorldCoords).Length / _gridCellSize.Y);
                _columns = (ushort)MathF.Ceiling(_items.Count / _rows);
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
    }
}