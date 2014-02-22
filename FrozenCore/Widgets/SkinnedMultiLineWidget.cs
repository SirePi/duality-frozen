// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.ColorFormat;
using Duality.Components;
using Duality.Resources;
using Duality.VertexFormat;
using FrozenCore.Widgets.Skin;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FrozenCore.Widgets
{
    [Serializable]
    public abstract class SkinnedMultiLineWidget<T> : SkinnedWidget<T> where T : BaseSkin
    {
        #region NonSerialized fields

        [NonSerialized]
        private BatchInfo _batchInfo;

        [NonSerialized]
        protected int _visibleHeight;

        [NonSerialized]
        protected int _visibleWidth;

        [NonSerialized]
        private GameObject _scrollbar;

        [NonSerialized]
        private float _scrollbarWidth;

        [NonSerialized]
        protected SkinnedScrollBar _scrollComponent;

        [NonSerialized]
        private Vector2 _uvUnit;

        [NonSerialized]
        private VertexC1P3T2[] _textVertices;

        [NonSerialized]
        protected bool _isScrollbarRequired;

        #endregion NonSerialized fields

        private ContentRef<ScrollBarSkin> _scrollBarSkin;
        private int _scrollSpeed;
        protected FormattedText _text;
        private ColorRgba _textColor;

        public ContentRef<ScrollBarSkin> ScrollBarSkin
        {
            get { return _scrollBarSkin; }
            set { _scrollBarSkin = value; }
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

            _textVertices[0].TexCoord.X = 0;
            _textVertices[1].TexCoord.X = _visibleWidth;
            _textVertices[2].TexCoord.X = _visibleWidth;
            _textVertices[3].TexCoord.X = 0;

            //if (_scrollbar == null || !_scrollbar.Active)
            if(!_isScrollbarRequired)
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

            if (_batchInfo != null)
            {
                Vector2 uvRatio = _batchInfo.MainTexture.Res.UVRatio;

                _textVertices[0].TexCoord *= _uvUnit * uvRatio;
                _textVertices[1].TexCoord *= _uvUnit * uvRatio;
                _textVertices[2].TexCoord *= _uvUnit * uvRatio;
                _textVertices[3].TexCoord *= _uvUnit * uvRatio;

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

        protected override void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas)
        {
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                _scrollbarWidth = 0;

                if (_scrollBarSkin.Res != null && _scrollbar == null)
                {
                    _scrollbarWidth = _scrollBarSkin.Res.GetSkinWidth();
                    AddScrollBar();
                }

                UpdateWidget(false);
            }
        }

        private void AddScrollBar()
        {
            _scrollbar = new GameObject("scrollbar", this.GameObj);

            float scrollbarWidth = _scrollBarSkin.Res.GetSkinWidth();

            Transform t = _scrollbar.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - scrollbarWidth, 0, DELTA_Z);
            t.RelativeAngle = 0;

            _scrollComponent = new SkinnedScrollBar();
            _scrollComponent.VisibilityGroup = this.VisibilityGroup;
            _scrollComponent.Skin = ScrollBarSkin;
            _scrollComponent.Rect = Rect.AlignTopLeft(0, 0, scrollbarWidth, Rect.H);
            _scrollComponent.ScrollSpeed = _scrollSpeed;

            _scrollbar.AddComponent<SkinnedScrollBar>(_scrollComponent);
            Scene.Current.AddObject(_scrollbar);
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
                            FilterMin = TextureMinFilter.LinearMipmapLinear,
                            FilterMag = TextureMagFilter.Linear,
                            WrapX = TextureWrapMode.ClampToEdge,
                            WrapY = TextureWrapMode.ClampToEdge,
                            TexSizeMode = Texture.SizeMode.Enlarge
                        }));
            }
            
            _visibleWidth = (int)Math.Floor(Rect.W - Skin.Res.Border.X - Skin.Res.Border.W - _scrollbarWidth);
            _visibleHeight = (int)Math.Floor(Rect.H - Skin.Res.Border.Y - Skin.Res.Border.Z);

            if (!inLimitTextWidth)
            {
                _text.MaxWidth = ushort.MaxValue;
                _text.MaxWidth = (int)Math.Ceiling(_text.Size.X);
            }
            else
            {
                _text.MaxWidth = _visibleWidth;
            }


            _isScrollbarRequired = _text.Size.Y > _visibleHeight;

            if (_scrollbar != null)
            {
                _scrollbar.Active = _isScrollbarRequired;
                if (_isScrollbarRequired)
                {
                    _scrollComponent.Maximum = (int)Math.Ceiling(_text.Size.Y - _visibleHeight);

                    _uvUnit.X = 1 / _text.Size.X;
                    _uvUnit.Y = 1 / _text.Size.Y;
                }
            }

            Pixmap.Layer textLayer = new Pixmap.Layer(_text.MaxWidth, (int)Math.Max(_visibleHeight, _text.Size.Y), Colors.Transparent);

            _text.RenderToBitmap(_text.SourceText, textLayer);

            Texture tx = _batchInfo.MainTexture.Res;
            tx.BasePixmap.Res.MainLayer = textLayer;
            tx.ReloadData();
        }
    }
}