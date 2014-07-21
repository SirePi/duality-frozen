// This code is provided under the MIT license. Originally by Alessandro Pilati.

using OpenTK;
using Duality;
using System;

namespace FrozenCore
{
    /// <summary>
    /// Models a Polygon as an ordered array of 2D, coplanar vertices
    /// </summary>
    public class Polygon
    {
        public static readonly Polygon NO_POLYGON = new Polygon(0);

        public Vector2[] Vertices { get; private set; }

        /// <summary>
        /// Constructs an N-sided Polygon as an empty array of N elements
        /// </summary>
        /// <param name="inNumVertices"></param>
        public Polygon(int inNumVertices)
        {
            Vertices = new Vector2[inNumVertices];
        }

        /// <summary>
        /// Constructs a Polygon starting from a Rect, centered around a point
        /// </summary>
        /// <param name="inRect"></param>
        /// <param name="inCenter"></param>
        public Polygon(Rect inRect, Vector2 inCenter) : this(4)
        {
            Vertices[0] = inCenter + inRect.TopLeft;
            Vertices[1] = inCenter + inRect.TopRight;
            Vertices[2] = inCenter + inRect.BottomRight;
            Vertices[3] = inCenter + inRect.BottomLeft;
        }

        /// <summary>
        /// Constructs a regular Polygon centered around a point, with a defined radius
        /// </summary>
        /// <param name="inCenter"></param>
        /// <param name="inRadius"></param>
        public Polygon(Vector2 inCenter, float inRadius)
            : this(inCenter, inRadius, 12)
        { }

        /// <summary>
        /// Constructs a regular Polygon centered around a point, with a defined radius, and a defined number of
        /// subdivisions
        /// </summary>
        /// <param name="inCenter"></param>
        /// <param name="inRadius"></param>
        /// <param name="inSubdivisions"></param>
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

        /// <summary>
        /// Returns the i-index vertex of the Polygon
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Vector2 this[int i]
        {
            get { return Vertices[i]; }
            set
            {
                Vertices[i] = value;
            }
        }

        /// <summary>
        /// Returns the Centroid of the Polygon, as the point where its extents are halved on each direction
        /// </summary>
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
        /// Determines if a Vector2 is inside or outside the Polygon
        /// </summary>
        /// <see cref="http://alienryderflex.com/polygon/"/>
        /// <param name="inPoint"></param>
        /// <returns>True if the point is inside, false otherwise.</returns>
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

        /// <summary>
        /// Offsets each vertex in the polygon by the desired amouns
        /// </summary>
        /// <param name="inOffset">The offset to apply</param>
        public void Offset(Vector2 inOffset)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] += inOffset;
            }
        }

        /// <summary>
        /// Centers the Polygon on its Centroid, effectively making it Zero
        /// </summary>
        public void CenterOnOrigin()
        {
            Offset(-Centroid);
        }
    }
}