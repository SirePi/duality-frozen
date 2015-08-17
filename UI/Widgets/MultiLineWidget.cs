// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class MultiLineWidget : Widget
    {
        #region NonSerialized fields

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected FormattedText _fText;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected bool _isScrollbarRequired;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected ScrollBar _scrollComponent;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected int _visibleHeight;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected int _visibleWidth;

        [NonSerialized]
        private BatchInfo _batchInfo;

        [NonSerialized]
        private GameObject _scrollbar;

        [NonSerialized]
        private VertexC1P3T2[] _textVertices;

        [NonSerialized]
        private Vector2 _uvUnit;

        #endregion NonSerialized fields

        protected ContentRef<MultiLineAppearance> _multiAppearance;
        private int _scrollSpeed;
        private ColorRgba _textColor;
        private ContentRef<Font> _textFont;

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiLineWidget()
        {
            ActiveArea = ActiveArea.Center;

            _fText = new FormattedText();
            _textColor = Colors.White;
            _textVertices = new VertexC1P3T2[4];
            _scrollSpeed = 5;

            _dirtyFlags |= DirtyFlags.Value;
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
        /// <param name="inDevice"></param>
        protected override void DrawCustom(IDrawDevice device)
        {
            if (!String.IsNullOrWhiteSpace(_fText.SourceText) && _batchInfo != null)
            {
                _textVertices[0].TexCoord.X = 0;
                _textVertices[1].TexCoord.X = _visibleWidth;
                _textVertices[2].TexCoord.X = _visibleWidth;
                _textVertices[3].TexCoord.X = 0;

                //if (_scrollbar == null || !_scrollbar.Active)
                if (!_isScrollbarRequired)
                {
                    _textVertices[0].TexCoord.Y = 0;
                    _textVertices[1].TexCoord.Y = 0;
                    _textVertices[2].TexCoord.Y = 1;
                    _textVertices[3].TexCoord.Y = 1;
                }
                else
                {
                    _textVertices[0].TexCoord.Y = _scrollComponent.Value;
                    _textVertices[1].TexCoord.Y = _textVertices[0].TexCoord.Y;
                    _textVertices[2].TexCoord.Y = (_scrollComponent.Value + _visibleHeight);
                    _textVertices[3].TexCoord.Y = _textVertices[2].TexCoord.Y;
                }

                Vector2 uvRatio = _batchInfo.MainTexture.Res.UVRatio;

                _textVertices[0].TexCoord *= uvRatio * _uvUnit;
                _textVertices[1].TexCoord *= uvRatio * _uvUnit;
                _textVertices[2].TexCoord *= uvRatio * _uvUnit;
                _textVertices[3].TexCoord *= uvRatio * _uvUnit;

                _textVertices[0].Color = _textColor;
                _textVertices[0].Pos = _points[5].SceneCoords;
                _textVertices[0].Pos.Z += DELTA_Z;

                _textVertices[1].Color = _textColor;
                _textVertices[1].Pos = _points[6].SceneCoords;
                _textVertices[1].Pos.Z += DELTA_Z;

                _textVertices[2].Color = _textColor;
                _textVertices[2].Pos = _points[10].SceneCoords;
                _textVertices[2].Pos.Z += DELTA_Z;

                _textVertices[3].Color = _textColor;
                _textVertices[3].Pos = _points[9].SceneCoords;
                _textVertices[3].Pos.Z += DELTA_Z;

                device.AddVertices(_batchInfo, VertexMode.Quads, _textVertices);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnStatusChange()
        {
            base.OnStatusChange();

            if (_scrollComponent != null)
            {
                _scrollComponent.Status = Status;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        protected override void OnUpdate(float inSecondsPast)
        {
            if (_scrollbar == null && !_multiAppearance.Res.ScrollBar.IsExplicitNull)
            {
                AddScrollBar();
            }

            if ((_dirtyFlags & DirtyFlags.Appearance) != DirtyFlags.None)
            {
                ContentRef<Appearance> sba = _multiAppearance.Res.Widget;

                _visibleWidth = (int)Math.Floor(Rect.W - sba.Res.Border.X - sba.Res.Border.W);
                _visibleHeight = (int)Math.Floor(Rect.H - sba.Res.Border.Y - sba.Res.Border.Z);
            }

            if (_scrollbar != null)
            {
                if ((_dirtyFlags & DirtyFlags.Appearance) != DirtyFlags.None)
                {
                    _scrollbar.GetComponent<ScrollBar>().Appearance = _multiAppearance.Res.ScrollBar;
                }

                _scrollbar.GetComponent<Widget>().Status = Status;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inLimitTextWidth"></param>
        protected void UpdateWidget(bool inLimitTextWidth)
        {
            if (_batchInfo == null)
            {
                _batchInfo = new BatchInfo(
                    DrawTechnique.Mask,
                    Colors.White,
                    new ContentRef<Texture>(
                        new Texture(new ContentRef<Pixmap>(new Pixmap()))
                        {
                            FilterMin = TextureMinFilter.NearestMipmapLinear,
                            FilterMag = TextureMagFilter.Nearest,
                            WrapX = TextureWrapMode.ClampToEdge,
                            WrapY = TextureWrapMode.ClampToEdge,
                            TexSizeMode = Texture.SizeMode.Enlarge
                        }));
            }

            _isScrollbarRequired = false;

            if (!String.IsNullOrWhiteSpace(_fText.SourceText))
            {
                if (_textFont.Res != null && _fText.Fonts[0] != _textFont)
                {
                    _fText.Fonts[0] = _textFont;
                }

                int textWidth = _visibleWidth;

                if (!inLimitTextWidth)
                {
                    _fText.MaxWidth = ushort.MaxValue;

                    textWidth = (int)Math.Ceiling(_fText.Size.X) + 10;
                }

                _fText.MaxWidth = textWidth;

                _isScrollbarRequired = _fText.Size.Y > _visibleHeight;

                _uvUnit.X = 1f / textWidth;
                _uvUnit.Y = 1;

                if (_scrollbar != null)
                {
                    _scrollbar.Active = _isScrollbarRequired;
                    if (_isScrollbarRequired)
                    {
                        _scrollComponent.Maximum = (int)Math.Ceiling(_fText.Size.Y - _visibleHeight);

                        _uvUnit.X = 1f / textWidth;
                        _uvUnit.Y = 1f / _fText.Size.Y;
                    }
                }

                Pixmap.Layer textLayer = new Pixmap.Layer(_fText.MaxWidth, (int)Math.Max(_visibleHeight, _fText.Size.Y), Colors.Transparent);

                _fText.RenderToBitmap(_fText.SourceText, textLayer);

                Texture tx = _batchInfo.MainTexture.Res;
                tx.BasePixmap.Res.MainLayer = textLayer;
                tx.ReloadData();
            }
        }

        private void AddScrollBar()
        {
            _scrollbar = new GameObject("scrollbar", this.GameObj);

            ContentRef<ScrollBarAppearance> sba = _multiAppearance.Res.ScrollBar;

            float scrollbarWidth = Math.Max(sba.Res.ButtonSize.X, sba.Res.ButtonSize.Y);

            Transform t = _scrollbar.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - scrollbarWidth, 0, 0);
            t.RelativeAngle = 0;

            _scrollComponent = new ScrollBar();
            _scrollComponent.VisibilityGroup = this.VisibilityGroup;
            _scrollComponent.Appearance = sba;

            _scrollComponent.Rect = Rect.AlignTopLeft(0, 0, scrollbarWidth, Rect.H);
            _scrollComponent.ScrollSpeed = _scrollSpeed;

            _scrollbar.AddComponent<ScrollBar>(_scrollComponent);
            Scene.Current.AddObject(_scrollbar);
        }

        protected override Appearance GetBaseAppearance()
        {
            return _multiAppearance.Res.Widget.Res;
        }
    }
}