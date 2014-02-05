using System;
using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.VertexFormat;
using FrozenCore.Widgets.Skin;
using OpenTK;
using Duality.EditorHints;

namespace FrozenCore.Widgets
{
    [Serializable]
    [RequiredComponent(typeof(Transform))]
    public abstract class Widget : Component, ICmpRenderer, ICmpUpdatable, ICmpInitializable
    {
        [NonSerialized]
        public static readonly Rect NO_ACTIVE_AREA;

        [NonSerialized]
        protected static readonly float DELTA_Z = .001f;

        [NonSerialized]
        protected Rect _activeArea;

        [NonSerialized]
        protected Polygon _activeAreaOnScreen;

        [NonSerialized]
        protected MultiSpacePoint[] _points;

        [NonSerialized]
        private BaseSkin _baseSkinRes;

        [NonSerialized]
        protected VertexC1P3T2[] _vertices;

        [NonSerialized]
        protected bool _widgetEnabled;

        [NonSerialized]
        private BatchInfo _batchInfo;

        [NonSerialized]
        private Vector2 _skinSize;

        [NonSerialized]
        private Vector2 _textureSize;

        [NonSerialized]
        private bool _uvCalculated;

        [NonSerialized]
        private Vector2 _uvDelta;

        [EditorHintFlags(MemberFlags.Invisible)]
        float ICmpRenderer.BoundRadius
        {
            get { return 0; } //Size.BoundingRadius; }
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public bool IsWidgetEnabled
        {
            get { return _widgetEnabled; }
            set { _widgetEnabled = value; }
        }

        protected BaseSkin BaseSkinRes
        {
            set
            {
                _baseSkinRes = value;
                _uvCalculated = false;
            }
        }

        private Rect _rect;
        /// <summary>
        /// [GET/SET] The Previous InputReceiverVisual in the GUI (not used)
        /// </summary>
        [EditorHintDecimalPlaces(1)]
        public Rect Rect 
        {
            get { return _rect; }
            set { _rect = value; }
        }

        private VisibilityFlag _visiblityFlag;

        public VisibilityFlag VisibilityGroup
        {
            get { return _visiblityFlag; }
            set { _visiblityFlag = value; }
        }

        public Widget()
        {
            _activeArea = NO_ACTIVE_AREA;
            _activeAreaOnScreen = new Polygon(4);
            _widgetEnabled = true;

            _vertices = new VertexC1P3T2[36];
            _points = new MultiSpacePoint[16];

            for (int i = 0; i < _vertices.Length; i++)
            {
                _vertices[i] = new VertexC1P3T2();
            }
        }

        public void Close()
        {
            Scene.Current.RemoveObject(this.GameObj);
        }

        public abstract Polygon GetActiveAreaOnScreen(Camera inCamera);

        void ICmpRenderer.Draw(IDrawDevice device)
        {
            if (_baseSkinRes != null)
            {
                _skinSize = _baseSkinRes.Size;

                if (!_uvCalculated)
                {
                    if (_baseSkinRes.Texture.Res != null)
                    {
                        _batchInfo = new BatchInfo(DrawTechnique.Mask, Colors.White, _baseSkinRes.Texture);
                        _textureSize = _baseSkinRes.Texture.Res.Size;
                        _uvDelta = _skinSize / _textureSize;

                        /********************
                         *  0    1    2    3
                         *  4    5    6    7
                         *  8    9   10   11
                         * 12   13   14   15
                         ********************/

                        /*****************************
                         *  0     3| 4     7| 8    11
                         *
                         *  1     2| 5     6| 9    10
                         * --    --+--    --+--    --
                         * 12    15|16    19|20    23
                         *
                         * 13    14|17    18|21    22
                         * --    --+--    --+--    --
                         * 24    27|28    31|32    35
                         *
                         * 25    26|29    30|33    34
                         *****************************/

                        SetTextureTopLeft(_baseSkinRes.Origin.Normal);
                    }

                    _uvCalculated = true;
                }

                if (_uvCalculated)
                {
                    Vector3 posTemp = GameObj.Transform.Pos;
                    float scaleTemp = 1.0f;
                    device.PreprocessCoords(ref posTemp, ref scaleTemp);

                    Vector2 xDot, yDot;
                    Vector2 xDotWorld, yDotWorld;
                    MathF.GetTransformDotVec(GameObj.Transform.Angle, scaleTemp, out xDot, out yDot);
                    MathF.GetTransformDotVec(GameObj.Transform.Angle, GameObj.Transform.Scale, out xDotWorld, out yDotWorld);

                    Rect rectTemp = Rect.Transform(GameObj.Transform.Scale, GameObj.Transform.Scale);

                    Vector2 topLeft = rectTemp.TopLeft;
                    Vector2 bottomRight = rectTemp.BottomRight;

                    Vector2 innerTopLeft = rectTemp.TopLeft + (new Vector2(_baseSkinRes.Border.X, _baseSkinRes.Border.Y) * GameObj.Transform.Scale);
                    Vector2 innerBottomRight = rectTemp.BottomRight - (new Vector2(_baseSkinRes.Border.Z, _baseSkinRes.Border.W) * GameObj.Transform.Scale);

                    _points[0].SceneCoords.X = topLeft.X;
                    _points[0].SceneCoords.Y = topLeft.Y;
                    _points[1].SceneCoords.X = innerTopLeft.X;
                    _points[1].SceneCoords.Y = topLeft.Y;
                    _points[2].SceneCoords.X = innerBottomRight.X;
                    _points[2].SceneCoords.Y = topLeft.Y;
                    _points[3].SceneCoords.X = bottomRight.X;
                    _points[3].SceneCoords.Y = topLeft.Y;
                    _points[4].SceneCoords.X = topLeft.X;
                    _points[4].SceneCoords.Y = innerTopLeft.Y;
                    _points[5].SceneCoords.X = innerTopLeft.X;
                    _points[5].SceneCoords.Y = innerTopLeft.Y;
                    _points[6].SceneCoords.X = innerBottomRight.X;
                    _points[6].SceneCoords.Y = innerTopLeft.Y;
                    _points[7].SceneCoords.X = bottomRight.X;
                    _points[7].SceneCoords.Y = innerTopLeft.Y;
                    _points[8].SceneCoords.X = topLeft.X;
                    _points[8].SceneCoords.Y = innerBottomRight.Y;
                    _points[9].SceneCoords.X = innerTopLeft.X;
                    _points[9].SceneCoords.Y = innerBottomRight.Y;
                    _points[10].SceneCoords.X = innerBottomRight.X;
                    _points[10].SceneCoords.Y = innerBottomRight.Y;
                    _points[11].SceneCoords.X = bottomRight.X;
                    _points[11].SceneCoords.Y = innerBottomRight.Y;
                    _points[12].SceneCoords.X = topLeft.X;
                    _points[12].SceneCoords.Y = bottomRight.Y;
                    _points[13].SceneCoords.X = innerTopLeft.X;
                    _points[13].SceneCoords.Y = bottomRight.Y;
                    _points[14].SceneCoords.X = innerBottomRight.X;
                    _points[14].SceneCoords.Y = bottomRight.Y;
                    _points[15].SceneCoords.X = bottomRight.X;
                    _points[15].SceneCoords.Y = bottomRight.Y;

                    for (int i = 0; i < _points.Length; i++)
                    {
                        _points[i].SceneCoords.Z = 0;
                        _points[i].WorldCoords = _points[i].SceneCoords;

                        MathF.TransformDotVec(ref _points[i].SceneCoords, ref xDot, ref yDot);
                        MathF.TransformDotVec(ref _points[i].WorldCoords, ref xDotWorld, ref yDotWorld);

                        _points[i].SceneCoords += posTemp;
                        _points[i].WorldCoords += GameObj.Transform.Pos;
                    }

                    /**
                     * Repositioning the pieces
                     **/
                    _vertices[0].Pos = _points[0].SceneCoords;
                    _vertices[0].TexCoord = _points[0].UVCoords;
                    _vertices[0].Color = Colors.White;

                    _vertices[1].Pos = _points[4].SceneCoords;
                    _vertices[1].TexCoord = _points[4].UVCoords;
                    _vertices[1].Color = Colors.White;

                    _vertices[2].Pos = _points[5].SceneCoords;
                    _vertices[2].TexCoord = _points[5].UVCoords;
                    _vertices[2].Color = Colors.White;

                    _vertices[3].Pos = _points[1].SceneCoords;
                    _vertices[3].TexCoord = _points[1].UVCoords;
                    _vertices[3].Color = Colors.White;

                    _vertices[4].Pos = _points[1].SceneCoords;
                    _vertices[4].TexCoord = _points[1].UVCoords;
                    _vertices[4].Color = Colors.White;

                    _vertices[5].Pos = _points[5].SceneCoords;
                    _vertices[5].TexCoord = _points[5].UVCoords;
                    _vertices[5].Color = Colors.White;

                    _vertices[6].Pos = _points[6].SceneCoords;
                    _vertices[6].TexCoord = _points[6].UVCoords;
                    _vertices[6].Color = Colors.White;

                    _vertices[7].Pos = _points[2].SceneCoords;
                    _vertices[7].TexCoord = _points[2].UVCoords;
                    _vertices[7].Color = Colors.White;

                    _vertices[8].Pos = _points[2].SceneCoords;
                    _vertices[8].TexCoord = _points[2].UVCoords;
                    _vertices[8].Color = Colors.White;

                    _vertices[9].Pos = _points[6].SceneCoords;
                    _vertices[9].TexCoord = _points[6].UVCoords;
                    _vertices[9].Color = Colors.White;

                    _vertices[10].Pos = _points[7].SceneCoords;
                    _vertices[10].TexCoord = _points[7].UVCoords;
                    _vertices[10].Color = Colors.White;

                    _vertices[11].Pos = _points[3].SceneCoords;
                    _vertices[11].TexCoord = _points[3].UVCoords;
                    _vertices[11].Color = Colors.White;

                    _vertices[12].Pos = _points[4].SceneCoords;
                    _vertices[12].TexCoord = _points[4].UVCoords;
                    _vertices[12].Color = Colors.White;

                    _vertices[13].Pos = _points[8].SceneCoords;
                    _vertices[13].TexCoord = _points[8].UVCoords;
                    _vertices[13].Color = Colors.White;

                    _vertices[14].Pos = _points[9].SceneCoords;
                    _vertices[14].TexCoord = _points[9].UVCoords;
                    _vertices[14].Color = Colors.White;

                    _vertices[15].Pos = _points[5].SceneCoords;
                    _vertices[15].TexCoord = _points[5].UVCoords;
                    _vertices[15].Color = Colors.White;

                    _vertices[16].Pos = _points[5].SceneCoords;
                    _vertices[16].TexCoord = _points[5].UVCoords;
                    _vertices[16].Color = Colors.White;

                    _vertices[17].Pos = _points[9].SceneCoords;
                    _vertices[17].TexCoord = _points[9].UVCoords;
                    _vertices[17].Color = Colors.White;

                    _vertices[18].Pos = _points[10].SceneCoords;
                    _vertices[18].TexCoord = _points[10].UVCoords;
                    _vertices[18].Color = Colors.White;

                    _vertices[19].Pos = _points[6].SceneCoords;
                    _vertices[19].TexCoord = _points[6].UVCoords;
                    _vertices[19].Color = Colors.White;

                    _vertices[20].Pos = _points[6].SceneCoords;
                    _vertices[20].TexCoord = _points[6].UVCoords;
                    _vertices[20].Color = Colors.White;

                    _vertices[21].Pos = _points[10].SceneCoords;
                    _vertices[21].TexCoord = _points[10].UVCoords;
                    _vertices[21].Color = Colors.White;

                    _vertices[22].Pos = _points[11].SceneCoords;
                    _vertices[22].TexCoord = _points[11].UVCoords;
                    _vertices[22].Color = Colors.White;

                    _vertices[23].Pos = _points[7].SceneCoords;
                    _vertices[23].TexCoord = _points[7].UVCoords;
                    _vertices[23].Color = Colors.White;

                    _vertices[24].Pos = _points[8].SceneCoords;
                    _vertices[24].TexCoord = _points[8].UVCoords;
                    _vertices[24].Color = Colors.White;

                    _vertices[25].Pos = _points[12].SceneCoords;
                    _vertices[25].TexCoord = _points[12].UVCoords;
                    _vertices[25].Color = Colors.White;

                    _vertices[26].Pos = _points[13].SceneCoords;
                    _vertices[26].TexCoord = _points[13].UVCoords;
                    _vertices[26].Color = Colors.White;

                    _vertices[27].Pos = _points[9].SceneCoords;
                    _vertices[27].TexCoord = _points[9].UVCoords;
                    _vertices[27].Color = Colors.White;

                    _vertices[28].Pos = _points[9].SceneCoords;
                    _vertices[28].TexCoord = _points[9].UVCoords;
                    _vertices[28].Color = Colors.White;

                    _vertices[29].Pos = _points[13].SceneCoords;
                    _vertices[29].TexCoord = _points[13].UVCoords;
                    _vertices[29].Color = Colors.White;

                    _vertices[30].Pos = _points[14].SceneCoords;
                    _vertices[30].TexCoord = _points[14].UVCoords;
                    _vertices[30].Color = Colors.White;

                    _vertices[31].Pos = _points[10].SceneCoords;
                    _vertices[31].TexCoord = _points[10].UVCoords;
                    _vertices[31].Color = Colors.White;

                    _vertices[32].Pos = _points[10].SceneCoords;
                    _vertices[32].TexCoord = _points[10].UVCoords;
                    _vertices[32].Color = Colors.White;

                    _vertices[33].Pos = _points[14].SceneCoords;
                    _vertices[33].TexCoord = _points[14].UVCoords;
                    _vertices[33].Color = Colors.White;

                    _vertices[34].Pos = _points[15].SceneCoords;
                    _vertices[34].TexCoord = _points[15].UVCoords;
                    _vertices[34].Color = Colors.White;

                    _vertices[35].Pos = _points[11].SceneCoords;
                    _vertices[35].TexCoord = _points[11].UVCoords;
                    _vertices[35].Color = Colors.White;

                    device.AddVertices(_batchInfo, VertexMode.Quads, _vertices);
                }

                Draw(device, new Canvas(device));
            }
        }

        bool ICmpRenderer.IsVisible(IDrawDevice device)
        {
            bool result = true;

            // Differing ScreenOverlay flag? Don't render!
            if ((device.VisibilityMask & VisibilityFlag.ScreenOverlay) != (VisibilityGroup & VisibilityFlag.ScreenOverlay))
            {
                result = false;
            }
            // No match in any VisibilityGroup? Don't render!
            if ((VisibilityGroup & device.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None)
            {
                result = false;
            }

            return result;
        }

        void ICmpUpdatable.OnUpdate()
        {
        }

        public virtual void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
        }

        public virtual void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
        }

        public virtual void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
        }

        public virtual void MouseEnter()
        {
        }

        public virtual void MouseLeave()
        {
        }

        public virtual void MouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
        }

        public virtual void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
        }

        public virtual void MouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
        }

        public void SetEnabled(bool inEnabled)
        {
            _widgetEnabled = inEnabled;

            if (!_widgetEnabled && _baseSkinRes != null)
            {
                SetTextureTopLeft(_baseSkinRes.Origin.Disabled);
            }
        }

        protected abstract void Draw(IDrawDevice inDevice, Canvas inCanvas);

        protected abstract void Initialize(Component.InitContext inContext);

        protected void SetTextureTopLeft(Vector2 inTopLeft)
        {
            /**
             * Calculating UV Coordinates of graphical corners
             **/
            if (_baseSkinRes != null)
            {
                Vector2 k = _uvDelta / _skinSize;

                float uvLeftBorder = _baseSkinRes.Border.X * k.X;
                float uvRightBorder = (_baseSkinRes.Size.X - _baseSkinRes.Border.Z) * k.X;
                float uvTopBorder = _baseSkinRes.Border.Y * k.Y;
                float uvBottomBorder = (_baseSkinRes.Size.Y - _baseSkinRes.Border.W) * k.Y;

                Vector2 uvTopLeft = inTopLeft / _textureSize;

                _points[0].UVCoords.X = uvTopLeft.X;
                _points[0].UVCoords.Y = uvTopLeft.Y;

                _points[1].UVCoords.X = uvTopLeft.X + uvLeftBorder;
                _points[1].UVCoords.Y = uvTopLeft.Y;

                _points[2].UVCoords.X = uvTopLeft.X + uvRightBorder;
                _points[2].UVCoords.Y = uvTopLeft.Y;

                _points[3].UVCoords.X = uvTopLeft.X + _uvDelta.X;
                _points[3].UVCoords.Y = uvTopLeft.Y;

                _points[4].UVCoords.X = uvTopLeft.X;
                _points[4].UVCoords.Y = uvTopLeft.Y + uvTopBorder;

                _points[5].UVCoords.X = uvTopLeft.X + uvLeftBorder;
                _points[5].UVCoords.Y = uvTopLeft.Y + uvTopBorder;

                _points[6].UVCoords.X = uvTopLeft.X + uvRightBorder;
                _points[6].UVCoords.Y = uvTopLeft.Y + uvTopBorder;

                _points[7].UVCoords.X = uvTopLeft.X + _uvDelta.X;
                _points[7].UVCoords.Y = uvTopLeft.Y + uvTopBorder;

                _points[8].UVCoords.X = uvTopLeft.X;
                _points[8].UVCoords.Y = uvTopLeft.Y + uvBottomBorder;

                _points[9].UVCoords.X = uvTopLeft.X + uvLeftBorder;
                _points[9].UVCoords.Y = uvTopLeft.Y + uvBottomBorder;

                _points[10].UVCoords.X = uvTopLeft.X + uvRightBorder;
                _points[10].UVCoords.Y = uvTopLeft.Y + uvBottomBorder;

                _points[11].UVCoords.X = uvTopLeft.X + _uvDelta.X;
                _points[11].UVCoords.Y = uvTopLeft.Y + uvBottomBorder;

                _points[12].UVCoords.X = uvTopLeft.X;
                _points[12].UVCoords.Y = uvTopLeft.Y + _uvDelta.Y;

                _points[13].UVCoords.X = uvTopLeft.X + uvLeftBorder;
                _points[13].UVCoords.Y = uvTopLeft.Y + _uvDelta.Y;

                _points[14].UVCoords.X = uvTopLeft.X + uvRightBorder;
                _points[14].UVCoords.Y = uvTopLeft.Y + _uvDelta.Y;

                _points[15].UVCoords.X = uvTopLeft.X + _uvDelta.X;
                _points[15].UVCoords.Y = uvTopLeft.Y + _uvDelta.Y;
            }
        }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            Initialize(context);
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        {
            
        }
    }
}