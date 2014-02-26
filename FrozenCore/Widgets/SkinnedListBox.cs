﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Resources;
using FrozenCore.Widgets.Skin;
using OpenTK;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Duality.Drawing;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedListBox : SkinnedMultiLineWidget<ListBoxSkin>
    {
        #region NonSerialized fields

        [NonSerialized]
        private Polygon _listArea;

        [NonSerialized]
        private Polygon _testPolygon;

        [NonSerialized]
        private bool _itemsAccessed;

        [NonSerialized]
        private object _selectedItem;

        [NonSerialized]
        private GameObject _highlight;

        [NonSerialized]
        private SkinnedPanel _highlightPanel;

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

        public SkinnedListBox()
        {
            _items = new List<object>();
            _text = new FormattedText();
            _listArea = new Polygon(4);
            _testPolygon = new Polygon(4);

            _text.SourceText = " ";
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            if (_itemsAccessed)
            {
                _text.SourceText = String.Join("/n", _items);
                _itemsAccessed = false;

                UpdateWidget(false);
            }

            UpdateHighlight();
        }

        protected override void Draw(IDrawDevice inDevice)
        {
            base.Draw(inDevice);

            _listArea[0] = inDevice.GetScreenCoord(_points[5].WorldCoords).Xy;
            _listArea[1] = inDevice.GetScreenCoord(_points[6].WorldCoords).Xy;
            _listArea[2] = inDevice.GetScreenCoord(_points[10].WorldCoords).Xy;
            _listArea[3] = inDevice.GetScreenCoord(_points[9].WorldCoords).Xy;
        }

        internal override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseDown(e);

            if (e.Button == OpenTK.Input.MouseButton.Left)
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

                for(int i = 0; i < _text.TextMetrics.LineBounds.Count; i++)
                {
                    Rect r = _text.TextMetrics.LineBounds[i];

                    if (!(r.Bottom.Y < top || r.Top.Y > bottom))
                    {
                        float realTop = Math.Max(r.Top.Y, top) - top;
                        float realBottom = Math.Min(r.Bottom.Y, bottom) - top;

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

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                if (_highlight == null)
                {
                    AddHighlight();
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
            _highlightPanel.Skin = Skin.Res.SelectorSkin;
            _highlightPanel.Rect = Rect.AlignTopLeft(0, 0, 0, 0);

            _highlight.AddComponent<SkinnedPanel>(_highlightPanel);
            Scene.Current.AddObject(_highlight);
        }

        private void UpdateHighlight()
        {
            if (SelectedIndex >= 0)
            {
                Rect selectionRect = _text.TextMetrics.LineBounds[SelectedIndex];

                if (_isScrollbarRequired)
                {
                    float top = _scrollComponent.Value;
                    float bottom = _scrollComponent.Value + _visibleHeight;

                    Vector3 relativePos = _highlight.Transform.RelativePos;
                    relativePos.X = Skin.Res.Border.X;
                    relativePos.Y = Skin.Res.Border.Y + selectionRect.Y - top;

                    Rect highlightRect = _highlightPanel.Rect;
                    highlightRect.H = selectionRect.H;
                    highlightRect.W = Rect.W - Skin.Res.Border.X - Skin.Res.Border.W;

                    _highlight.Transform.RelativePos = relativePos;
                    _highlightPanel.Rect = highlightRect;
                    _highlightPanel.VisibleRect = highlightRect;

                    if (selectionRect.Top.Y < top && selectionRect.Bottom.Y >= top)
                    {
                        Rect highlightVisibleRect = _highlightPanel.VisibleRect;
                        highlightVisibleRect.Y = top - selectionRect.Top.Y;

                        _highlightPanel.VisibleRect = highlightVisibleRect;
                    }

                    if (selectionRect.Bottom.Y > bottom && selectionRect.Top.Y <= bottom)
                    {
                        Rect highlightVisibleRect = _highlightPanel.VisibleRect;
                        highlightVisibleRect.H = highlightRect.H - (selectionRect.Bottom.Y - bottom);

                        _highlightPanel.VisibleRect = highlightVisibleRect;
                    }
                }
                else
                {

                }
            }
        }
    }
}