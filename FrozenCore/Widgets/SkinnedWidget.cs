// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Drawing;
using FrozenCore.Resources.Widgets;
using OpenTK;
using Duality.Resources;

namespace FrozenCore.Widgets
{
    [Serializable]
    [RequiredComponent(typeof(Transform))]
    public abstract class SkinnedWidget : Widget
    {
        #region NonSerialized fields

        [NonSerialized]
        protected bool _isMouseOver;

        [NonSerialized]
        private Vector2 _skinSize;

        [NonSerialized]
        private Vector2 _textureSize;

        [NonSerialized]
        private Vector2 _uvDelta;

        #endregion NonSerialized fields

        private static readonly int[] COLUMN_1 = new int[] { 0, 1, 12, 13, 24, 25 };
        private static readonly int[] COLUMN_2 = new int[] { 3, 2, 15, 14, 27, 26 };
        private static readonly int[] COLUMN_3 = new int[] { 4, 5, 16, 17, 28, 29 };
        private static readonly int[] COLUMN_4 = new int[] { 7, 6, 19, 18, 31, 30 };
        private static readonly int[] COLUMN_5 = new int[] { 8, 9, 20, 21, 32, 33 };
        private static readonly int[] COLUMN_6 = new int[] { 11, 10, 23, 22, 35, 34 };
        private static readonly int[] ROW_1 = new int[] { 0, 3, 4, 7, 8, 11 };
        private static readonly int[] ROW_2 = new int[] { 1, 2, 5, 6, 9, 10 };
        private static readonly int[] ROW_3 = new int[] { 12, 15, 16, 19, 20, 23 };
        private static readonly int[] ROW_4 = new int[] { 13, 14, 17, 18, 21, 22 };
        private static readonly int[] ROW_5 = new int[] { 24, 27, 28, 31, 32, 35 };
        private static readonly int[] ROW_6 = new int[] { 25, 26, 29, 30, 33, 34 };
        private ContentRef<WidgetSkin> _skin;

        public ContentRef<WidgetSkin> Skin
        {
            get { return _skin; }
            set
            {
                if (value == null || value.Res.Material == null)
                {
                    _skin = WidgetSkin.DEFAULT;
                }

                _skin = value;
                SkinChanged();
            }
        }

        public SkinnedWidget()
        {
            _skin = WidgetSkin.DEFAULT;
        }

        internal override void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
        }

        internal override void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
        }

        internal override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
        }

        internal override void MouseEnter()
        {
            _isMouseOver = true;

            if (Status != WidgetStatus.Disabled)
            {
                Status = WidgetStatus.Hover;
            }
        }

        internal override void MouseLeave()
        {
            _isMouseOver = false;

            if (Status != WidgetStatus.Disabled)
            {
                Status = WidgetStatus.Normal;
            }
        }

        internal override void MouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
        }

        internal override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
        }

        internal override void MouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
        }

        protected override void Draw(IDrawDevice inDevice)
        {
            /********************
             *  0    1    2    3
             *  4    5    6    7
             *  8    9   10   11
             * 12   13   14   15
             ********************/
            if (_skin.Res != null)
            {
                WidgetSkin skin = _skin.Res;

                Vector3 posTemp = GameObj.Transform.Pos;
                float scaleTemp = 1.0f;
                inDevice.PreprocessCoords(ref posTemp, ref scaleTemp);

                Vector2 xDot, yDot;
                Vector2 xDotWorld, yDotWorld;
                MathF.GetTransformDotVec(GameObj.Transform.Angle, scaleTemp, out xDot, out yDot);
                MathF.GetTransformDotVec(GameObj.Transform.Angle, GameObj.Transform.Scale, out xDotWorld, out yDotWorld);

                Rect rectTemp = Rect.Transform(GameObj.Transform.Scale, GameObj.Transform.Scale);

                Vector2 topLeft = rectTemp.TopLeft;
                Vector2 bottomRight = rectTemp.BottomRight;

                Vector2 innerTopLeft = rectTemp.TopLeft + (new Vector2(skin.Border.X, skin.Border.Y) * GameObj.Transform.Scale);
                Vector2 innerBottomRight = rectTemp.BottomRight - (new Vector2(skin.Border.Z, skin.Border.W) * GameObj.Transform.Scale);

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

                if (VisibleRect != Rect.Empty && VisibleRect != Rect)
                {
                    ApplyVisibilityRectangle();
                }

                inDevice.AddVertices(skin.Material, VertexMode.Quads, _vertices);
            }
        }

        protected override void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas)
        {
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            SkinChanged();
        }

        protected override void OnStatusChange()
        {
            if (_skin.Res != null)
            {
                switch (Status)
                {
                    case WidgetStatus.Normal:
                        SetTextureTopLeft(_skin.Res.Origin.Normal);
                        break;

                    case WidgetStatus.Hover:
                        SetTextureTopLeft(_skin.Res.Origin.Hover);
                        break;

                    case WidgetStatus.Active:
                        SetTextureTopLeft(_skin.Res.Origin.Active);
                        break;

                    case WidgetStatus.Disabled:
                        SetTextureTopLeft(_skin.Res.Origin.Disabled);
                        break;
                }
            }
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            if (_skin == null || _skin.Res == null)
            {
                Skin = WidgetSkin.DEFAULT;
            }
        }

        private void ApplyVisibilityRectangle()
        {
            WidgetSkin skin = _skin.Res;

            Vector2 topLeft = Vector2.Zero;
            Vector2 bottomRight = Rect.Size;

            Vector2 innerTopLeft = topLeft + (new Vector2(skin.Border.X, skin.Border.Y));
            Vector2 innerBottomRight = bottomRight - (new Vector2(skin.Border.Z, skin.Border.W));

            if (VisibleRect.Left.X >= innerTopLeft.X)
            {
                _vertices[0].Color = Colors.Transparent;
                _vertices[1].Color = Colors.Transparent;
                _vertices[2].Color = Colors.Transparent;
                _vertices[3].Color = Colors.Transparent;
                _vertices[12].Color = Colors.Transparent;
                _vertices[13].Color = Colors.Transparent;
                _vertices[14].Color = Colors.Transparent;
                _vertices[15].Color = Colors.Transparent;
                _vertices[24].Color = Colors.Transparent;
                _vertices[25].Color = Colors.Transparent;
                _vertices[26].Color = Colors.Transparent;
                _vertices[27].Color = Colors.Transparent;
            }

            if (VisibleRect.Right.X <= innerBottomRight.X)
            {
                _vertices[8].Color = Colors.Transparent;
                _vertices[9].Color = Colors.Transparent;
                _vertices[10].Color = Colors.Transparent;
                _vertices[11].Color = Colors.Transparent;
                _vertices[20].Color = Colors.Transparent;
                _vertices[21].Color = Colors.Transparent;
                _vertices[22].Color = Colors.Transparent;
                _vertices[23].Color = Colors.Transparent;
                _vertices[32].Color = Colors.Transparent;
                _vertices[33].Color = Colors.Transparent;
                _vertices[34].Color = Colors.Transparent;
                _vertices[35].Color = Colors.Transparent;
            }

            if ((VisibleRect.Left.X >= innerBottomRight.X) || (VisibleRect.Right.X <= innerTopLeft.X))
            {
                _vertices[4].Color = Colors.Transparent;
                _vertices[5].Color = Colors.Transparent;
                _vertices[6].Color = Colors.Transparent;
                _vertices[7].Color = Colors.Transparent;
                _vertices[16].Color = Colors.Transparent;
                _vertices[17].Color = Colors.Transparent;
                _vertices[18].Color = Colors.Transparent;
                _vertices[19].Color = Colors.Transparent;
                _vertices[28].Color = Colors.Transparent;
                _vertices[29].Color = Colors.Transparent;
                _vertices[30].Color = Colors.Transparent;
                _vertices[31].Color = Colors.Transparent;
            }

            if (VisibleRect.Top.Y >= innerTopLeft.Y)
            {
                _vertices[0].Color = Colors.Transparent;
                _vertices[1].Color = Colors.Transparent;
                _vertices[2].Color = Colors.Transparent;
                _vertices[3].Color = Colors.Transparent;
                _vertices[4].Color = Colors.Transparent;
                _vertices[5].Color = Colors.Transparent;
                _vertices[6].Color = Colors.Transparent;
                _vertices[7].Color = Colors.Transparent;
                _vertices[8].Color = Colors.Transparent;
                _vertices[9].Color = Colors.Transparent;
                _vertices[10].Color = Colors.Transparent;
                _vertices[11].Color = Colors.Transparent;
            }

            if (VisibleRect.Bottom.Y <= innerBottomRight.Y)
            {
                _vertices[24].Color = Colors.Transparent;
                _vertices[25].Color = Colors.Transparent;
                _vertices[26].Color = Colors.Transparent;
                _vertices[27].Color = Colors.Transparent;
                _vertices[28].Color = Colors.Transparent;
                _vertices[29].Color = Colors.Transparent;
                _vertices[30].Color = Colors.Transparent;
                _vertices[31].Color = Colors.Transparent;
                _vertices[32].Color = Colors.Transparent;
                _vertices[33].Color = Colors.Transparent;
                _vertices[34].Color = Colors.Transparent;
                _vertices[35].Color = Colors.Transparent;
            }

            if ((VisibleRect.Top.Y >= innerBottomRight.Y) || (VisibleRect.Bottom.Y <= innerTopLeft.Y))
            {
                _vertices[12].Color = Colors.Transparent;
                _vertices[13].Color = Colors.Transparent;
                _vertices[14].Color = Colors.Transparent;
                _vertices[15].Color = Colors.Transparent;
                _vertices[16].Color = Colors.Transparent;
                _vertices[17].Color = Colors.Transparent;
                _vertices[18].Color = Colors.Transparent;
                _vertices[19].Color = Colors.Transparent;
                _vertices[20].Color = Colors.Transparent;
                _vertices[21].Color = Colors.Transparent;
                _vertices[22].Color = Colors.Transparent;
                _vertices[23].Color = Colors.Transparent;
            }

            Vector2 centerSize = innerBottomRight - innerTopLeft;

            // Checking Left side
            if (VisibleRect.TopLeft.X < innerTopLeft.X)
            {
                float k = VisibleRect.TopLeft.X / skin.Border.X;
                FixVertices(COLUMN_1, 3, k);
            }
            else if (VisibleRect.TopLeft.X < innerBottomRight.X)
            {
                float k = (VisibleRect.TopLeft.X - innerTopLeft.X) / (centerSize.X);
                FixVertices(COLUMN_3, 7, k);
            }
            else
            {
                float k = (VisibleRect.TopLeft.X - innerBottomRight.X) / skin.Border.W;
                FixVertices(COLUMN_5, 11, k);
            }

            // Checking Right side
            if (VisibleRect.BottomRight.X > innerBottomRight.X)
            {
                float k = (bottomRight.X - VisibleRect.BottomRight.X) / skin.Border.W;
                FixVertices(COLUMN_6, 8, k);
            }
            else if (VisibleRect.BottomRight.X > innerTopLeft.X)
            {
                float k = (innerBottomRight.X - VisibleRect.BottomRight.X) / (centerSize.X);
                FixVertices(COLUMN_4, 4, k);
            }
            else
            {
                float k = (topLeft.X - VisibleRect.BottomRight.X) / skin.Border.X;
                FixVertices(COLUMN_2, 0, k);
            }

            // Checking Top side
            if (VisibleRect.TopLeft.Y < innerTopLeft.Y)
            {
                float k = VisibleRect.TopLeft.Y / skin.Border.Y;
                FixVertices(ROW_1, 1, k);
            }
            else if (VisibleRect.TopLeft.Y < innerBottomRight.Y)
            {
                float k = (VisibleRect.TopLeft.Y - innerTopLeft.Y) / (centerSize.Y);
                FixVertices(ROW_3, 13, k);
            }
            else
            {
                float k = (VisibleRect.TopLeft.Y - innerBottomRight.Y) / skin.Border.Z;
                FixVertices(ROW_5, 25, k);
            }

            // Checking Bottom side
            if (VisibleRect.BottomRight.Y > innerBottomRight.Y)
            {
                float k = (bottomRight.Y - VisibleRect.BottomRight.Y) / skin.Border.Z;
                FixVertices(ROW_6, 24, k);
            }
            else if (VisibleRect.BottomRight.Y > innerTopLeft.Y)
            {
                float k = (innerBottomRight.Y - VisibleRect.BottomRight.Y) / (centerSize.Y);
                FixVertices(ROW_4, 12, k);
            }
            else
            {
                float k = (topLeft.Y - VisibleRect.BottomRight.Y) / skin.Border.Y;
                FixVertices(ROW_2, 0, k);
            }
        }

        private void FixVertices(int[] inVertexIndexes, int inVertexComparator, float inK)
        {
            Vector3 deltaPos = (_vertices[inVertexComparator].Pos - _vertices[inVertexIndexes[0]].Pos) * inK;
            Vector2 deltaTex = (_vertices[inVertexComparator].TexCoord - _vertices[inVertexIndexes[0]].TexCoord) * inK;

            foreach (int i in inVertexIndexes)
            {
                _vertices[i].Pos += deltaPos;
                _vertices[i].TexCoord += deltaTex;
            }
        }

        private void SetTextureTopLeft(Vector2 inTopLeft)
        {
            WidgetSkin skin = _skin.Res;
            _skinSize = skin.Size;

            /**
             * Calculating UV Coordinates of graphical corners
             **/
            Vector2 k = _uvDelta / _skinSize;

            float uvLeftBorder = skin.Border.X * k.X;
            float uvRightBorder = (skin.Size.X - skin.Border.Z) * k.X;
            float uvTopBorder = skin.Border.Y * k.Y;
            float uvBottomBorder = (skin.Size.Y - skin.Border.W) * k.Y;

            Vector2 uvTopLeft = inTopLeft / _textureSize * skin.Material.Res.MainTexture.Res.UVRatio;

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

        private void SkinChanged()
        {
            if (_skin.Res != null)
            {
                Texture tx = _skin.Res.Material.Res.MainTexture.Res;

                _textureSize = tx.Size;
                _uvDelta = _skinSize / _textureSize;
                _uvDelta = _uvDelta * tx.UVRatio;

                OnStatusChange();
            }
        }
    }
}