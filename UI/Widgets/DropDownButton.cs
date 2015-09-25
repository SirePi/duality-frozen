// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;

using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using Duality.Input;

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
        private ContentRef<DropDownAppearance> _dropAppearance;
        private List<object> _items;
        private int _scrollSpeed;
        private ColorRgba _textColor;
        private ContentRef<Font> _textFont;

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
        /// [GET / SET] the Skin used for the dropdown Panel
        /// </summary>
        public ContentRef<DropDownAppearance> Appearance
        {
            get { return _dropAppearance; }
            set
            {
                _dropAppearance = value;
                _dirtyFlags |= DirtyFlags.Appearance;
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
            if (_listBox == null && !_dropAppearance.IsExplicitNull)
            {
                AddListBox();
            }

            if ((_dirtyFlags & DirtyFlags.Appearance) != DirtyFlags.None)
            {
                if (_listBox != null)
                    _listBoxComponent.Appearance = _dropAppearance.Res.ListBox;
            }

            if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
            {
                _listBoxComponent.Items = Items;
            }
        }

        private void AddListBox()
        {
            _listBox = new GameObject("listBox", this.GameObj);

            Transform t = _listBox.AddComponent<Transform>();
            t.RelativePos = new Vector3(0, Rect.H, 0);
            t.RelativeAngle = 0;

            _listBoxComponent = new ListBox();
            _listBoxComponent.VisibilityGroup = this.VisibilityGroup;
            _listBoxComponent.Appearance = _dropAppearance.Res.ListBox;
            _listBoxComponent.Rect = Rect.Align(Alignment.TopLeft, 0, 0, Rect.W, _dropDownHeight);
            _listBoxComponent.TextFont = TextFont;

            _listBox.AddComponent<ListBox>(_listBoxComponent);
            _listBox.Active = false;

            _listBoxComponent.SelectedItem = null;

            Scene.Current.AddObject(_listBox);
        }

        protected override Appearance GetBaseAppearance()
        {
            return _dropAppearance.Res.Widget.Res;
        }
    }
}