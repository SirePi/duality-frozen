// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Core.Geometry;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using System;
using System.Collections.Generic;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A ListBox Widget
    /// </summary>

    [EditorHintImage(ResNames.ImageListBox)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class ListBox : MultiLineWidget
    {
        #region NonSerialized fields

        [DontSerialize]
        private GameObject _highlight;

        [DontSerialize]
        private Panel _highlightPanel;

        [DontSerialize]
        private Polygon _listArea;

        [DontSerialize]
        private object _selectedItem;

        [DontSerialize]
        private Polygon _testPolygon;

        #endregion NonSerialized fields

        private List<object> _items;

        private ContentRef<ListBoxAppearance> _listAppearance;

        public ContentRef<ListBoxAppearance> Appearance
        {
            get { return _listAppearance; }
            set
            {
                _listAppearance = value;
                _dirtyFlags |= DirtyFlags.Appearance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ListBox()
        {
            _items = new List<object>();
            _fText = new FormattedText();
            _listArea = new Polygon(4);
            _testPolygon = new Polygon(4);

            Appearance = DefaultGradientSkin.LISTBOX;
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
        /// [GET / SET] the index of the Selected item
        /// </summary>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public override void MouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                float top = 0;
                float bottom = _visibleHeight;
                Vector2 point = new Vector2(e.X, e.Y);

                if (_isScrollbarRequired)
                {
                    top = _scrollComponent.Value;
                    bottom = _scrollComponent.Value + _visibleHeight;
                }

                float delta = bottom - top;
                Vector2 deltaLeft = (_activeAreaOnScreen[3] - _activeAreaOnScreen[0]) / delta;
                Vector2 deltaRight = (_activeAreaOnScreen[2] - _activeAreaOnScreen[1]) / delta;

                for (int i = 0; i < _fText.TextMetrics.LineBounds.Count; i++)
                {
                    Rect r = _fText.TextMetrics.LineBounds[i];

                    if (!(r.BottomY < top || r.TopY > bottom))
                    {
                        float realTop = Math.Max(r.TopY, top) - top;
                        float realBottom = Math.Min(r.BottomY, bottom) - top;

                        _testPolygon[0] = _activeAreaOnScreen[0] + (deltaLeft * realTop);
                        _testPolygon[1] = _activeAreaOnScreen[1] + (deltaRight * realTop);
                        _testPolygon[2] = _activeAreaOnScreen[1] + (deltaRight * realBottom);
                        _testPolygon[3] = _activeAreaOnScreen[0] + (deltaLeft * realBottom);

                        if (_testPolygon.Contains(point))
                        {
                            SelectedItem = _items[i];
                            break;
                        }
                    }
                }
            }
        }

        /*
        /// <summary>
        ///
        /// </summary>
        /// <param name="inDevice"></param>
        protected override void DrawCustom(IDrawDevice inDevice)
        {
            _listArea[0] = inDevice.GetScreenCoord(_points[5].WorldCoords).Xy;
            _listArea[1] = inDevice.GetScreenCoord(_points[6].WorldCoords).Xy;
            _listArea[2] = inDevice.GetScreenCoord(_points[10].WorldCoords).Xy;
            _listArea[3] = inDevice.GetScreenCoord(_points[9].WorldCoords).Xy;
        }*/

        protected override Appearance GetBaseAppearance()
        {
            return _listAppearance.Res.Widget.Res;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        protected override void OnUpdate(float inSecondsPast)
        {
            _multiAppearance.Res = _listAppearance.Res;

            base.OnUpdate(inSecondsPast);

            if (_highlight == null && !_listAppearance.Res.Highlight.IsExplicitNull)
            {
                AddHighlight();
            }

            if ((_dirtyFlags & DirtyFlags.Appearance) != DirtyFlags.None)
            {
                if (_highlight != null)
                {
                    _highlight.GetComponent<Panel>().Appearance = AppearanceManager.RequestAppearanceContentRef(_listAppearance.Res.Highlight);
                }
            }

            if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
            {
                _fText.SourceText = String.Join("/n", _items);
                UpdateWidget(false);
            }

            UpdateHighlight();
        }

        private void AddHighlight()
        {
            _highlight = new GameObject("highlight", this.GameObj);

            Transform t = _highlight.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, DELTA_Z / 2);
            t.RelativeAngle = 0;

            _highlightPanel = new Panel();
            _highlightPanel.VisibilityGroup = this.VisibilityGroup;
            _highlightPanel.Appearance = AppearanceManager.RequestAppearanceContentRef(_listAppearance.Res.Highlight); ;
            _highlightPanel.Rect = Rect.Align(Alignment.TopLeft, 0, 0, 0, 0);

            _highlight.AddComponent<Panel>(_highlightPanel);
			this.GameObj.ParentScene.AddObject(_highlight);
        }

        private void UpdateHighlight()
        {
            if (SelectedIndex >= 0)
            {
                Rect selectionRect = _fText.TextMetrics.LineBounds[SelectedIndex];

                float top = _scrollComponent.Value;
                float bottom = _scrollComponent.Value + _visibleHeight;

                ContentRef<Appearance> app = _listAppearance.Res.Widget;

                Vector3 relativePos = _highlight.Transform.RelativePos;
                relativePos.X = app.Res.Border.X;
                relativePos.Y = app.Res.Border.Y + selectionRect.Y - top;

                Rect highlightRect = _highlightPanel.Rect;
                highlightRect.H = selectionRect.H;
                highlightRect.W = Rect.W - app.Res.Border.X - app.Res.Border.W;

                _highlight.Transform.RelativePos = relativePos;
                _highlightPanel.Rect = highlightRect;
                _highlightPanel.ClippingRect = highlightRect;

                if (_isScrollbarRequired)
                {
                    _highlightPanel.Active = !(selectionRect.BottomY <= top || selectionRect.TopY >= bottom);

                    if (selectionRect.TopY < top && selectionRect.BottomY >= top)
                    {
                        Rect highlightVisibleRect = _highlightPanel.ClippingRect;
                        highlightVisibleRect.Y = top - selectionRect.TopY;

                        _highlightPanel.ClippingRect = highlightVisibleRect;
                    }

                    if (selectionRect.BottomY > bottom && selectionRect.TopY <= bottom)
                    {
                        Rect highlightVisibleRect = _highlightPanel.ClippingRect;
                        highlightVisibleRect.H = highlightRect.H - (selectionRect.BottomY - bottom);

                        _highlightPanel.ClippingRect = highlightVisibleRect;
                    }
                }
            }
        }
    }
}