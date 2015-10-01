// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Drawing;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Core;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    /// <summary>
    ///
    /// </summary>
    public abstract class Skin : Resource
    {
        [DontSerialize]
        protected static readonly int[] COLUMN_1 = new int[] { 0, 1, 12, 13, 24, 25 };

        [DontSerialize]
        protected static readonly int[] COLUMN_2 = new int[] { 3, 2, 15, 14, 27, 26 };

        [DontSerialize]
        protected static readonly int[] COLUMN_3 = new int[] { 4, 5, 16, 17, 28, 29 };

        [DontSerialize]
        protected static readonly int[] COLUMN_4 = new int[] { 7, 6, 19, 18, 31, 30 };

        [DontSerialize]
        protected static readonly int[] COLUMN_5 = new int[] { 8, 9, 20, 21, 32, 33 };

        [DontSerialize]
        protected static readonly int[] COLUMN_6 = new int[] { 11, 10, 23, 22, 35, 34 };

        [DontSerialize]
        protected static readonly int[] ROW_1 = new int[] { 0, 3, 4, 7, 8, 11 };

        [DontSerialize]
        protected static readonly int[] ROW_2 = new int[] { 1, 2, 5, 6, 9, 10 };

        [DontSerialize]
        protected static readonly int[] ROW_3 = new int[] { 12, 15, 16, 19, 20, 23 };

        [DontSerialize]
        protected static readonly int[] ROW_4 = new int[] { 13, 14, 17, 18, 21, 22 };

        [DontSerialize]
        protected static readonly int[] ROW_5 = new int[] { 24, 27, 28, 31, 32, 35 };

        [DontSerialize]
        protected static readonly int[] ROW_6 = new int[] { 25, 26, 29, 30, 33, 34 };

        protected ContentRef<Material> _material;

        protected Skin()
        {
            _material = Material.SolidWhite;
        }

        public virtual void PrepareVertices(ref MultiSpacePoint[] vertices, Vector4 border, Rect widgetArea, float scale)
        {
            Rect waTemp = widgetArea.Scaled(scale, scale);

            Vector2 topLeft = waTemp.TopLeft;
            Vector2 bottomRight = waTemp.BottomRight;

            Vector2 innerTopLeft = topLeft + (border.XY() * scale);
            Vector2 innerBottomRight = bottomRight - (border.ZW() * scale);

            vertices[0].SceneCoords.X = topLeft.X;
            vertices[0].SceneCoords.Y = topLeft.Y;

            vertices[1].SceneCoords.X = innerTopLeft.X;
            vertices[1].SceneCoords.Y = topLeft.Y;

            vertices[2].SceneCoords.X = innerBottomRight.X;
            vertices[2].SceneCoords.Y = topLeft.Y;

            vertices[3].SceneCoords.X = bottomRight.X;
            vertices[3].SceneCoords.Y = topLeft.Y;

            vertices[4].SceneCoords.X = topLeft.X;
            vertices[4].SceneCoords.Y = innerTopLeft.Y;

            vertices[5].SceneCoords.X = innerTopLeft.X;
            vertices[5].SceneCoords.Y = innerTopLeft.Y;

            vertices[6].SceneCoords.X = innerBottomRight.X;
            vertices[6].SceneCoords.Y = innerTopLeft.Y;

            vertices[7].SceneCoords.X = bottomRight.X;
            vertices[7].SceneCoords.Y = innerTopLeft.Y;

            vertices[8].SceneCoords.X = topLeft.X;
            vertices[8].SceneCoords.Y = innerBottomRight.Y;

            vertices[9].SceneCoords.X = innerTopLeft.X;
            vertices[9].SceneCoords.Y = innerBottomRight.Y;

            vertices[10].SceneCoords.X = innerBottomRight.X;
            vertices[10].SceneCoords.Y = innerBottomRight.Y;

            vertices[11].SceneCoords.X = bottomRight.X;
            vertices[11].SceneCoords.Y = innerBottomRight.Y;

            vertices[12].SceneCoords.X = topLeft.X;
            vertices[12].SceneCoords.Y = bottomRight.Y;

            vertices[13].SceneCoords.X = innerTopLeft.X;
            vertices[13].SceneCoords.Y = bottomRight.Y;

            vertices[14].SceneCoords.X = innerBottomRight.X;
            vertices[14].SceneCoords.Y = bottomRight.Y;

            vertices[15].SceneCoords.X = bottomRight.X;
            vertices[15].SceneCoords.Y = bottomRight.Y;

            UVAndColor(ref vertices, border);
        }

        protected abstract void UVAndColor(ref MultiSpacePoint[] vertices, Vector4 border);

        internal void Clip(ref VertexC1P3T2[] vertices, Vector4 border, Rect widgetArea, Rect clipArea)
        {
            Vector2 topLeft = widgetArea.TopLeft;
            Vector2 bottomRight = widgetArea.BottomRight;

            Vector2 innerTopLeft = topLeft + (border.XY());
            Vector2 innerBottomRight = bottomRight - (border.ZW());

            //applying clipping area
            if (clipArea != Rect.Empty && clipArea != widgetArea)
            {
                if (clipArea.LeftX >= innerTopLeft.X)
                {
                    vertices[0].Color = Colors.Transparent;
                    vertices[1].Color = Colors.Transparent;
                    vertices[2].Color = Colors.Transparent;
                    vertices[3].Color = Colors.Transparent;
                    vertices[12].Color = Colors.Transparent;
                    vertices[13].Color = Colors.Transparent;
                    vertices[14].Color = Colors.Transparent;
                    vertices[15].Color = Colors.Transparent;
                    vertices[24].Color = Colors.Transparent;
                    vertices[25].Color = Colors.Transparent;
                    vertices[26].Color = Colors.Transparent;
                    vertices[27].Color = Colors.Transparent;
                }

                if (clipArea.RightX <= innerBottomRight.X)
                {
                    vertices[8].Color = Colors.Transparent;
                    vertices[9].Color = Colors.Transparent;
                    vertices[10].Color = Colors.Transparent;
                    vertices[11].Color = Colors.Transparent;
                    vertices[20].Color = Colors.Transparent;
                    vertices[21].Color = Colors.Transparent;
                    vertices[22].Color = Colors.Transparent;
                    vertices[23].Color = Colors.Transparent;
                    vertices[32].Color = Colors.Transparent;
                    vertices[33].Color = Colors.Transparent;
                    vertices[34].Color = Colors.Transparent;
                    vertices[35].Color = Colors.Transparent;
                }

                if ((clipArea.LeftX >= innerBottomRight.X) || (clipArea.RightX <= innerTopLeft.X))
                {
                    vertices[4].Color = Colors.Transparent;
                    vertices[5].Color = Colors.Transparent;
                    vertices[6].Color = Colors.Transparent;
                    vertices[7].Color = Colors.Transparent;
                    vertices[16].Color = Colors.Transparent;
                    vertices[17].Color = Colors.Transparent;
                    vertices[18].Color = Colors.Transparent;
                    vertices[19].Color = Colors.Transparent;
                    vertices[28].Color = Colors.Transparent;
                    vertices[29].Color = Colors.Transparent;
                    vertices[30].Color = Colors.Transparent;
                    vertices[31].Color = Colors.Transparent;
                }

                if (clipArea.TopY >= innerTopLeft.Y)
                {
                    vertices[0].Color = Colors.Transparent;
                    vertices[1].Color = Colors.Transparent;
                    vertices[2].Color = Colors.Transparent;
                    vertices[3].Color = Colors.Transparent;
                    vertices[4].Color = Colors.Transparent;
                    vertices[5].Color = Colors.Transparent;
                    vertices[6].Color = Colors.Transparent;
                    vertices[7].Color = Colors.Transparent;
                    vertices[8].Color = Colors.Transparent;
                    vertices[9].Color = Colors.Transparent;
                    vertices[10].Color = Colors.Transparent;
                    vertices[11].Color = Colors.Transparent;
                }

                if (clipArea.BottomY <= innerBottomRight.Y)
                {
                    vertices[24].Color = Colors.Transparent;
                    vertices[25].Color = Colors.Transparent;
                    vertices[26].Color = Colors.Transparent;
                    vertices[27].Color = Colors.Transparent;
                    vertices[28].Color = Colors.Transparent;
                    vertices[29].Color = Colors.Transparent;
                    vertices[30].Color = Colors.Transparent;
                    vertices[31].Color = Colors.Transparent;
                    vertices[32].Color = Colors.Transparent;
                    vertices[33].Color = Colors.Transparent;
                    vertices[34].Color = Colors.Transparent;
                    vertices[35].Color = Colors.Transparent;
                }

                if ((clipArea.TopY >= innerBottomRight.Y) || (clipArea.BottomY <= innerTopLeft.Y))
                {
                    vertices[12].Color = Colors.Transparent;
                    vertices[13].Color = Colors.Transparent;
                    vertices[14].Color = Colors.Transparent;
                    vertices[15].Color = Colors.Transparent;
                    vertices[16].Color = Colors.Transparent;
                    vertices[17].Color = Colors.Transparent;
                    vertices[18].Color = Colors.Transparent;
                    vertices[19].Color = Colors.Transparent;
                    vertices[20].Color = Colors.Transparent;
                    vertices[21].Color = Colors.Transparent;
                    vertices[22].Color = Colors.Transparent;
                    vertices[23].Color = Colors.Transparent;
                }

                Vector2 centerSize = innerBottomRight - innerTopLeft;

                // Checking Left side
                if (clipArea.TopLeft.X < innerTopLeft.X)
                {
                    float k = clipArea.TopLeft.X / border.X;
                    FixVertices(ref vertices, COLUMN_1, 3, k);
                }
                else if (clipArea.TopLeft.X < innerBottomRight.X)
                {
                    float k = (clipArea.TopLeft.X - innerTopLeft.X) / (centerSize.X);
                    FixVertices(ref vertices, COLUMN_3, 7, k);
                }
                else
                {
                    float k = (clipArea.TopLeft.X - innerBottomRight.X) / border.W;
                    FixVertices(ref vertices, COLUMN_5, 11, k);
                }

                // Checking Right side
                if (clipArea.BottomRight.X > innerBottomRight.X)
                {
                    float k = (bottomRight.X - clipArea.BottomRight.X) / border.W;
                    FixVertices(ref vertices, COLUMN_6, 8, k);
                }
                else if (clipArea.BottomRight.X > innerTopLeft.X)
                {
                    float k = (innerBottomRight.X - clipArea.BottomRight.X) / (centerSize.X);
                    FixVertices(ref vertices, COLUMN_4, 4, k);
                }
                else
                {
                    float k = (topLeft.X - clipArea.BottomRight.X) / border.X;
                    FixVertices(ref vertices, COLUMN_2, 0, k);
                }

                // Checking Top side
                if (clipArea.TopLeft.Y < innerTopLeft.Y)
                {
                    float k = clipArea.TopLeft.Y / border.Y;
                    FixVertices(ref vertices, ROW_1, 1, k);
                }
                else if (clipArea.TopLeft.Y < innerBottomRight.Y)
                {
                    float k = (clipArea.TopLeft.Y - innerTopLeft.Y) / (centerSize.Y);
                    FixVertices(ref vertices, ROW_3, 13, k);
                }
                else
                {
                    float k = (clipArea.TopLeft.Y - innerBottomRight.Y) / border.Z;
                    FixVertices(ref vertices, ROW_5, 25, k);
                }

                // Checking Bottom side
                if (clipArea.BottomRight.Y > innerBottomRight.Y)
                {
                    float k = (bottomRight.Y - clipArea.BottomRight.Y) / border.Z;
                    FixVertices(ref vertices, ROW_6, 24, k);
                }
                else if (clipArea.BottomRight.Y > innerTopLeft.Y)
                {
                    float k = (innerBottomRight.Y - clipArea.BottomRight.Y) / (centerSize.Y);
                    FixVertices(ref vertices, ROW_4, 12, k);
                }
                else
                {
                    float k = (topLeft.Y - clipArea.BottomRight.Y) / border.Y;
                    FixVertices(ref vertices, ROW_2, 0, k);
                }
            }
        }

        private void FixVertices(ref VertexC1P3T2[] vertices, int[] inVertexIndexes, int inVertexComparator, float inK)
        {
            Vector3 deltaPos = (vertices[inVertexComparator].Pos - vertices[inVertexIndexes[0]].Pos) * inK;
            Vector2 deltaTex = (vertices[inVertexComparator].TexCoord - vertices[inVertexIndexes[0]].TexCoord) * inK;
            Vector4 deltaColor = (vertices[inVertexComparator].Color.ToVector4() - vertices[inVertexIndexes[0]].Color.ToVector4()) * inK;

            foreach (int i in inVertexIndexes)
            {
                vertices[i].Pos += deltaPos;
                vertices[i].TexCoord += deltaTex;
                vertices[i].Color = Colors.FromBase255Vector4(vertices[i].Color.ToVector4() + deltaColor);
            }
        }

        public void Draw(IDrawDevice device, VertexC1P3T2[] vertices)
        {
            device.AddVertices(_material, VertexMode.Quads, vertices);
        }
    }
}