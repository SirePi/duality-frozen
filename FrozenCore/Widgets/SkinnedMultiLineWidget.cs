// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Resources;
using FrozenCore.Resources.Widgets;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FrozenCore.Widgets
{
    [Serializable]
    public abstract class SkinnedMultiLineWidget : SkinnedWidget
    {
        #region NonSerialized fields

        [NonSerialized]
        protected bool _isScrollbarRequired;

        [NonSerialized]
        protected SkinnedScrollBar _scrollComponent;

        [NonSerialized]
        protected int _visibleHeight;

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

        protected FormattedText _text;
        private Vector2 _scrollbarButtonsSize;
        private Vector2 _scrollbarCursorSize;
        private ContentRef<WidgetSkin> _scrollbarCursorSkin;
        private ContentRef<WidgetSkin> _scrollbarDecreaseButtonSkin;
        private ContentRef<WidgetSkin> _scrollbarIncreaseButtonSkin;
        private ContentRef<WidgetSkin> _scrollbarSkin;
        private ContentRef<Font> _font;
        private int _scrollSpeed;
        private ColorRgba _textColor;

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

        public ContentRef<Font> TextFont
        {
            get { return _font; }
            set { _font = value; }
        }

        public SkinnedMultiLineWidget()
        {
            ActiveArea = Widgets.ActiveArea.Center;

            _textColor = Colors.White;
            _textVertices = new VertexC1P3T2[4];
            _scrollSpeed = 5;
        }

        protected override void Draw(IDrawDevice inDevice)
        {
            base.Draw(inDevice);

            if (!String.IsNullOrWhiteSpace(_text.SourceText) && _batchInfo != null)
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

                inDevice.AddVertices(_batchInfo, VertexMode.Quads, _textVertices);
            }
        }

        protected override void DrawCanvas(Canvas inCanvas)
        {
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                if (_scrollbar == null)
                {
                    AddScrollBar();
                }

                _visibleWidth = (int)Math.Floor(Rect.W - Skin.Res.Border.X - Skin.Res.Border.W);
                _visibleHeight = (int)Math.Floor(Rect.H - Skin.Res.Border.Y - Skin.Res.Border.Z);

                UpdateWidget(true);
            }
        }

        protected override void OnStatusChange()
        {
            base.OnStatusChange();

            if (_scrollComponent != null)
            {
                _scrollComponent.Status = Status;
            }
        }

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

            bool isTextEmpty = String.IsNullOrWhiteSpace(_text.SourceText);
            _isScrollbarRequired = false;

            if (!isTextEmpty)
            {
                if (_font != null && _text.Fonts[0] != _font)
                {
                    _text.Fonts[0] = _font;
                }

                int textWidth = _visibleWidth;

                if (!inLimitTextWidth)
                {
                    _text.MaxWidth = ushort.MaxValue;

                    textWidth = (int)Math.Ceiling(_text.Size.X) + 10;
                }

                _text.MaxWidth = textWidth;

                _isScrollbarRequired = _text.Size.Y > _visibleHeight;

                _uvUnit.X = 1f / textWidth;
                _uvUnit.Y = 1;

                if (_scrollbar != null)
                {
                    _scrollbar.Active = _isScrollbarRequired;
                    if (_isScrollbarRequired)
                    {
                        _scrollComponent.Maximum = (int)Math.Ceiling(_text.Size.Y - _visibleHeight);

                        _uvUnit.X = 1f / textWidth;
                        _uvUnit.Y = 1f / _text.Size.Y;
                    }
                }

                Pixmap.Layer textLayer = new Pixmap.Layer(_text.MaxWidth, (int)Math.Max(_visibleHeight, _text.Size.Y), Colors.Transparent);

                _text.RenderToBitmap(_text.SourceText, textLayer);

                Texture tx = _batchInfo.MainTexture.Res;
                tx.BasePixmap.Res.MainLayer = textLayer;
                tx.ReloadData();
            }
        }

        private void AddScrollBar()
        {
            _scrollbar = new GameObject("scrollbar", this.GameObj);

            float scrollbarWidth = Math.Max(ScrollbarButtonsSize.X, ScrollbarCursorSize.X);

            Transform t = _scrollbar.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - scrollbarWidth, 0, 0);
            t.RelativeAngle = 0;

            _scrollComponent = new SkinnedScrollBar();
            _scrollComponent.VisibilityGroup = this.VisibilityGroup;
            _scrollComponent.Skin = ScrollbarSkin;
            _scrollComponent.CursorSkin = ScrollbarCursorSkin;
            _scrollComponent.DecreaseButtonSkin = ScrollbarDecreaseButtonSkin;
            _scrollComponent.IncreaseButtonSkin = ScrollbarIncreaseButtonSkin;
            _scrollComponent.ButtonsSize = ScrollbarButtonsSize;
            _scrollComponent.CursorSize = ScrollbarCursorSize;

            _scrollComponent.Rect = Rect.AlignTopLeft(0, 0, scrollbarWidth, Rect.H);
            _scrollComponent.ScrollSpeed = _scrollSpeed;

            _scrollbar.AddComponent<SkinnedScrollBar>(_scrollComponent);
            Scene.Current.AddObject(_scrollbar);
        }
    }
}