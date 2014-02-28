// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using OpenTK;
using Duality.Drawing;
using Duality.Editor;
using Duality.Components;
using Duality.Resources;
using System.Collections.Generic;

namespace FrozenCore.Widgets
{/*
    [Serializable]
    public class SkinnedDropDownButton : SkinnedWidget<BaseSkin>
    {
        private static readonly List<object> EMPTY_LIST = new List<object>();

        #region NonSerialized fields

        [NonSerialized]
        private GameObject _listBox;

        [NonSerialized]
        private SkinnedListBox _listBoxComponent;

        [NonSerialized]
        private FormattedText _text;

        #endregion NonSerialized fields

        private ContentRef<ScrollBarSkin> _scrollBarSkin;

        private ContentRef<ListBoxSkin> _dropDownPanelSkin;

        private ColorRgba _textColor;
        private int _dropDownHeight;

        public List<object> Items
        {
            get { return _listBoxComponent != null ? _listBoxComponent.Items : EMPTY_LIST; }
            set
            {
                if (_listBoxComponent != null)
                {
                    _listBoxComponent.Items = value;
                }
            }
        }

        public ContentRef<ScrollBarSkin> ScrollBarSkin
        {
            get { return _scrollBarSkin; }
            set { _scrollBarSkin = value; }
        }

        public ColorRgba TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public ContentRef<ListBoxSkin> DropDownPanelSkin
        {
            get { return _dropDownPanelSkin; }
            set { _dropDownPanelSkin = value; }
        }

        public int DropDownHeight
        {
            get { return _dropDownHeight; }
            set { _dropDownHeight = value; }
        }

        public SkinnedDropDownButton()
        {
            ActiveArea = Widgets.ActiveArea.LeftBorder;

            _text = new FormattedText();
            _dropDownHeight = 100;
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
            }
        }

        protected override void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas)
        {
            if (_listBoxComponent != null)
            {
                Vector3 buttonLeft = (_points[5].WorldCoords + _points[9].WorldCoords) / 2;

                _text.SourceText = _listBoxComponent.SelectedItem.ToString();

                inCanvas.PushState();
                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;
                inCanvas.DrawText(_text, buttonLeft.X, buttonLeft.Y, buttonLeft.Z + DELTA_Z, null, Alignment.Left);
                inCanvas.PopState();
            }
        }

        internal override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseDown(e);

            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                //_listBox
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
            _listBoxComponent.Skin = DropDownPanelSkin;
            _listBoxComponent.ScrollBarSkin = ScrollBarSkin;
            _listBoxComponent.Rect = Rect.AlignTopLeft(0, 0, Rect.W, _dropDownHeight);
            _listBoxComponent.Active = false;

            _listBox.AddComponent<SkinnedListBox>(_listBoxComponent);
            Scene.Current.AddObject(_listBox);
        }
    }*/
}