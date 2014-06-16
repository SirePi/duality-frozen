// This code is provided under the MIT license. Originally by Alessandro Pilati.

using OpenTK;
using Duality;
using System;

namespace FrozenCore
{
    public class Polygon
    {
        public static readonly Polygon NO_POLYGON = new Polygon(0);

        public Vector2[] Vertices { get; private set; }

        public Polygon(int inNumVertices)
        {
            Vertices = new Vector2[inNumVertices];
        }

        public Polygon(Rect inRect, Vector2 inCenter) : this(4)
        {
            Vertices[0] = inCenter + inRect.TopLeft;
            Vertices[1] = inCenter + inRect.TopRight;
            Vertices[2] = inCenter + inRect.BottomRight;
            Vertices[3] = inCenter + inRect.BottomLeft;
        }

        public Polygon(Vector2 inCenter, float inRadius)
            : this(inCenter, inRadius, 12)
        { }

        public Polygon(Vector2 inCenter, float inRadius, int inSubdivisions)
            : this(inSubdivisions)
        {
            float delta = MathF.TwoPi / Vertices.Length;

            for (int i = 0; i < Vertices.Length; i++)
            {
                float angle = delta * i;
                float x = MathF.Cos(angle) * inRadius;
                float y = MathF.Sin(angle) * inRadius;

                Vertices[i] = inCenter + new Vector2(x, y);
            }
        }

        public Vector2 this[int i]
        {
            get { return Vertices[i]; }
            set
            {
                Vertices[i] = value;
            }
        }

        public Vector2 Centroid
        {
            get
            {
                Vector2 result = Vector2.Zero;

                for (int i = 0; i < Vertices.Length; i++)
                {
                    result += Vertices[i];
                }

                return result / Vertices.Length;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <see cref="http://alienryderflex.com/polygon/"/>
        /// <param name="inPoint"></param>
        /// <returns></returns>
        public bool Contains(Vector2 inPoint)
        {
            bool oddNodes = false;
            int i = 0;
            int j = Vertices.Length - 1;

            for (i = 0; i < Vertices.Length; i++)
            {
                if ((Vertices[i].Y < inPoint.Y && Vertices[j].Y >= inPoint.Y || Vertices[j].Y < inPoint.Y && Vertices[i].Y >= inPoint.Y) && (Vertices[i].X <= inPoint.X || Vertices[j].X <= inPoint.X))
                {
                    oddNodes ^= (Vertices[i].X + (inPoint.Y - Vertices[i].Y) / (Vertices[j].Y - Vertices[i].Y) * (Vertices[j].X - Vertices[i].X) < inPoint.X);
                }

                j = i;
            }

            return oddNodes;
        }

        public void Offset(Vector2 inOffset)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] += inOffset;
            }
        }

        public void CenterOnOrigin()
        {
            Offset(-Centroid);
        }
    }
}