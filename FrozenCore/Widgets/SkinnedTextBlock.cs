// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using Duality.Components;
using Duality.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FrozenCore.Widgets
{/*
    [Serializable]
    public class SkinnedTextBlock : SkinnedMultiLineWidget<BaseSkin>
    {
        public ObservableFormattedText Text
        {
            get { return _text as ObservableFormattedText; }
            set { _text = value; }
        }

        public SkinnedTextBlock()
        {
            ActiveArea = Widgets.ActiveArea.None;

            _text = new ObservableFormattedText();
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            if (Text.TextChanged)
            {
                Text.TextChanged = false; 
                UpdateWidget(true);
            }
        }
    }*/
}

/// BACKUP ONLY
/*
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
    public class SkinnedTextBlock : SkinnedWidget<BaseSkin>
    {
        #region NonSerialized fields

        [NonSerialized]
        private BatchInfo _batchInfo;

        [NonSerialized]
        private int _layerHeight;

        [NonSerialized]
        private int _layerWidth;

        [NonSerialized]
        private GameObject _scrollbar;

        [NonSerialized]
        private float _scrollbarWidth;

        [NonSerialized]
        private SkinnedScrollBar _scrollComponent;

        [NonSerialized]
        private float _uvUnit;

        #endregion NonSerialized fields

        private ContentRef<ScrollBarSkin> _scrollBarSkin;
        private int _scrollSpeed;
        private ObservableFormattedText _text;
        private ColorRgba _textColor;
        private VertexC1P3T2[] _textVertices;

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
        public ObservableFormattedText Text
        {
            get { return _text; }
            set { _text = value; }
        }
        public ColorRgba TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public SkinnedTextBlock()
        {
            ActiveArea = Widgets.ActiveArea.Center;

            _text = new ObservableFormattedText();
            _textColor = Colors.White;
            _textVertices = new VertexC1P3T2[4];
            _scrollSpeed = 5;

            _batchInfo = new BatchInfo(
                    DrawTechnique.Mask,
                    Colors.White,
                    new ContentRef<Texture>(
                        new Texture(new ContentRef<Pixmap>(new Pixmap()))
                        {
                            FilterMin = TextureMinFilter.LinearMipmapLinear,
                            FilterMag = TextureMagFilter.LinearSharpenSgis,
                            WrapX = TextureWrapMode.ClampToEdge,
                            WrapY = TextureWrapMode.ClampToEdge,
                            TexSizeMode = Texture.SizeMode.Enlarge
                        }));
        }

        protected override void Draw(IDrawDevice inDevice)
        {
            base.Draw(inDevice);

            if (_scrollbar == null || !_scrollbar.Active)
            {
                _textVertices[0].TexCoord = Vector2.Zero;
                _textVertices[1].TexCoord = Vector2.UnitX;
                _textVertices[2].TexCoord = Vector2.One;
                _textVertices[3].TexCoord = Vector2.UnitY;
            }
            else
            {
                _textVertices[0].TexCoord.X = 0;
                _textVertices[0].TexCoord.Y = _uvUnit * _scrollComponent.Value;
                _textVertices[1].TexCoord.X = 1;
                _textVertices[1].TexCoord.Y = _textVertices[0].TexCoord.Y;
                _textVertices[2].TexCoord.X = 1;
                _textVertices[2].TexCoord.Y = _uvUnit * (_scrollComponent.Value + _layerHeight);
                _textVertices[3].TexCoord.X = 0;
                _textVertices[3].TexCoord.Y = _textVertices[2].TexCoord.Y;
            }

            Vector2 uvRatio = _batchInfo.MainTexture.Res.UVRatio;

            _textVertices[0].TexCoord *= uvRatio;
            _textVertices[1].TexCoord *= uvRatio;
            _textVertices[2].TexCoord *= uvRatio;
            _textVertices[3].TexCoord *= uvRatio;

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

                Update();
            }
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            if (_text.TextChanged)
            {
                Update();
                _text.TextChanged = false;
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

        private void Update()
        {
            if (_batchInfo != null)
            {
                _layerWidth = (int)Math.Floor(Rect.W - Skin.Res.Border.X - Skin.Res.Border.W - _scrollbarWidth);
                _layerHeight = (int)Math.Floor(Rect.H - Skin.Res.Border.Y - Skin.Res.Border.Z);

                _text.MaxWidth = _layerWidth;

                bool scrollbarNeeded = _text.Size.Y > _layerHeight;

                if (_scrollbar != null)
                {
                    _scrollbar.Active = scrollbarNeeded;
                    if (scrollbarNeeded)
                    {
                        _scrollComponent.Maximum = (int)Math.Ceiling(_text.Size.Y - _layerHeight);

                        _uvUnit = 1 / _text.Size.Y;
                    }
                }

                Pixmap.Layer textLayer = new Pixmap.Layer(_text.MaxWidth, (int)Math.Max(_layerHeight, _text.Size.Y), Colors.Transparent);

                _text.RenderToBitmap(_text.SourceText, textLayer);

                Texture tx = _batchInfo.MainTexture.Res;
                tx.BasePixmap.Res.MainLayer = textLayer;
                tx.ReloadData();
            }
        }
    }
}
*/