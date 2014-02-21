// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.ColorFormat;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Resources;
using FrozenCore.Widgets.Skin;
using OpenTK;
using System.Collections.Generic;
using Duality.VertexFormat;
using OpenTK.Graphics.OpenGL;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedListBox : SkinnedMultiLineWidget<ListBoxSkin>
    {
        #region NonSerialized fields

        [NonSerialized]
        private Polygon _areaOnScreen;

        [NonSerialized]
        private bool _itemsAccessed;

        #endregion NonSerialized fields

        private List<object> _items;

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

        public SkinnedListBox()
        {
            _items = new List<object>();
            _text = new FormattedText();
            _areaOnScreen = new Polygon(4);
        }

        bool x;

        protected override void OnUpdate(float inSecondsPast)
        {
            if (!x)
            {
                Items.Add(new List<int>());
                Items.Add(new List<int>());
                Items.Add(new List<int>());
                Items.Add(new List<int>());
                Items.Add(new List<int>());
                Items.Add(new List<int>());
                Items.Add(new List<int>());
                Items.Add(new List<int>());
                Items.Add(new List<int>());
                Items.Add(new List<int>());
                x = true;
            }

            if (_itemsAccessed)
            {
                _text.SourceText = String.Join("/n", _items);
                _itemsAccessed = false;

                UpdateWidget(false);
            }
        }

        protected override void Draw(IDrawDevice inDevice)
        {
            base.Draw(inDevice);

            _areaOnScreen[0] = inDevice.GetScreenCoord(_points[5].WorldCoords).Xy;
            _areaOnScreen[1] = inDevice.GetScreenCoord(_points[6].WorldCoords).Xy;
            _areaOnScreen[2] = inDevice.GetScreenCoord(_points[10].WorldCoords).Xy;
            _areaOnScreen[3] = inDevice.GetScreenCoord(_points[9].WorldCoords).Xy;
        }

        internal override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseDown(e);

            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                float top = 0;
                float bottom = _visibleHeight;

                if (!_isScrollbarRequired)
                {
                    top = _scrollComponent.Value;
                    bottom = _scrollComponent.Value + _visibleHeight;
                }

                foreach (Rect r in _text.TextMetrics.LineBounds)
                {
                    if (!(r.Bottom.Y < top || r.Top.Y > bottom))
                    {
                        int a = 0;
                    }
                }
            }
        }
    }
}