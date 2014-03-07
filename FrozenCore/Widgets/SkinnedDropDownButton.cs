// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Resources;
using FrozenCore.Resources.Widgets;
using OpenTK;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedDropDownButton : SkinnedWidget
    {
        protected FormattedText _text;

        #region NonSerialized fields

        [NonSerialized]
        private bool _itemsAccessed;

        [NonSerialized]
        private GameObject _listBox;

        [NonSerialized]
        private SkinnedListBox _listBoxComponent;

        #endregion NonSerialized fields

        private int _dropDownHeight;
        private ContentRef<WidgetSkin> _dropdownSkin;
        private ContentRef<WidgetSkin> _highlightSkin;
        private List<object> _items;
        private Vector2 _scrollbarButtonsSize;
        private Vector2 _scrollbarCursorSize;
        private ContentRef<WidgetSkin> _scrollbarCursorSkin;
        private ContentRef<WidgetSkin> _scrollbarDecreaseButtonSkin;
        private ContentRef<WidgetSkin> _scrollbarIncreaseButtonSkin;
        private ContentRef<WidgetSkin> _scrollbarSkin;
        private int _scrollSpeed;
        private ColorRgba _textColor;

        public int DropDownHeight
        {
            get { return _dropDownHeight; }
            set
            {
                _dropDownHeight = value;
                if (_listBoxComponent != null)
                {
                    _listBoxComponent.Rect = Rect.AlignTopLeft(0, 0, Rect.W, _dropDownHeight);
                }
            }
        }

        public ContentRef<WidgetSkin> DropdownSkin
        {
            get { return _dropdownSkin; }
            set { _dropdownSkin = value; }
        }

        public ContentRef<WidgetSkin> HighlightSkin
        {
            get { return _highlightSkin; }
            set { _highlightSkin = value; }
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

        public int ScrollSpeed
        {
            get { return _scrollSpeed; }
            set { _scrollSpeed = value; }
        }

        public ColorRgba TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public SkinnedDropDownButton()
        {
            ActiveArea = Widgets.ActiveArea.RightBorder;

            _items = new List<object>();
            _text = new FormattedText();
            _dropDownHeight = 100;
            _scrollSpeed = 5;
        }

        internal override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseDown(e);

            if (Status != WidgetStatus.Disabled)
            {
                if (e.Button == OpenTK.Input.MouseButton.Left)
                {
                    _listBox.Active = !_listBox.Active;
                }
            }
        }

        protected override void OnStatusChange()
        {
            base.OnStatusChange();

            if (Status == WidgetStatus.Disabled)
            {
                _listBox.Active = false;
            }
        }

        protected override void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas)
        {
            if (_listBoxComponent != null)
            {
                Vector3 buttonLeft = (_points[5].WorldCoords + _points[9].WorldCoords) / 2;

                _text.SourceText = String.Empty;

                if (_listBoxComponent.SelectedItem != null)
                {
                    _text.SourceText = _listBoxComponent.SelectedItem.ToString();
                }

                inCanvas.PushState();
                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;
                inCanvas.DrawText(_text, buttonLeft.X, buttonLeft.Y, buttonLeft.Z + DELTA_Z, null, Alignment.Left);
                inCanvas.PopState();
            }
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                if (_listBox == null)
                {
                    AddListBox();
                }

                _itemsAccessed = true;
            }
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            base.OnUpdate(inSecondsPast);
            if (_itemsAccessed)
            {
                _listBoxComponent.Items = Items;
                _itemsAccessed = false;
            }
        }

        private void AddListBox()
        {
            _listBox = new GameObject("listBox", this.GameObj);

            Transform t = _listBox.AddComponent<Transform>();
            t.RelativePos = new Vector3(0, Rect.H, 0);
            t.RelativeAngle = 0;

            _listBoxComponent = new SkinnedListBox();
            _listBoxComponent.VisibilityGroup = this.VisibilityGroup;
            _listBoxComponent.Skin = DropdownSkin;
            _listBoxComponent.HighlightSkin = HighlightSkin;
            _listBoxComponent.ScrollbarSkin = ScrollbarSkin;
            _listBoxComponent.ScrollbarCursorSkin = ScrollbarCursorSkin;
            _listBoxComponent.ScrollbarDecreaseButtonSkin = ScrollbarDecreaseButtonSkin;
            _listBoxComponent.ScrollbarIncreaseButtonSkin = ScrollbarIncreaseButtonSkin;
            _listBoxComponent.ScrollbarButtonsSize = ScrollbarButtonsSize;
            _listBoxComponent.ScrollbarCursorSize = ScrollbarCursorSize;
            _listBoxComponent.Rect = Rect.AlignTopLeft(0, 0, Rect.W, _dropDownHeight);

            _listBox.AddComponent<SkinnedListBox>(_listBoxComponent);
            _listBox.Active = false;

            _listBoxComponent.SelectedItem = null;

            Scene.Current.AddObject(_listBox);
        }
    }
}