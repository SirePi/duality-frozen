// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality;
using OpenTK;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Geometry
{
    /// <summary>
    /// Models a Polygon as an ordered array of 2D, coplanar vertices
    /// </summary>
    public class Polygon
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly Polygon NO_POLYGON = new Polygon(0);

        /// <summary>
        /// Constructs an N-sided Polygon as an empty array of N elements
        /// </summary>
        /// <param name="inNumVertices"></param>
        public Polygon(int inNumVertices)
        {
            Vertices = new Vector2[inNumVertices];
        }

        /// <summary>
        /// Constructs a Polygon starting from a list of vertices
        /// </summary>
        /// <param name="vertices"></param>
        public Polygon(IEnumerable<Vector2> vertices)
        {
            Vertices = new List<Vector2>(vertices).ToArray();
        }

        /// <summary>
        /// Constructs a Polygon starting from a Rect
        /// </summary>
        /// <param name="inRect"></param>
        public Polygon(Rect inRect)
            : this(4)
        {
            Vertices[0] = inRect.TopLeft;
            Vertices[1] = inRect.TopRight;
            Vertices[2] = inRect.BottomRight;
            Vertices[3] = inRect.BottomLeft;
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
        ///
        /// </summary>
        public enum PolygonWinding
        {
            /// <summary>
            ///
            /// </summary>
            None = 0,

            /// <summary>
            ///
            /// </summary>
            Clockwise,

            /// <summary>
            ///
            /// </summary>
            Counterclockwise
        }

        /// <summary>
        /// Returns the area of the Polygon
        /// </summary>
        public float Area
        {
            get { return MathF.Abs(SignedDoubleArea() * 0.5f); }
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
        /// The vertices of the Polygon
        /// </summary>
        public Vector2[] Vertices { get; private set; }
        /// <summary>
        /// Returns the Winding (Clockwise/Counterclockwise) of the Polygon's vertices
        /// </summary>
        public PolygonWinding Winding
        {
            get
            {
                float signedDoubleArea = SignedDoubleArea();
                if (signedDoubleArea < 0)
                {
                    return PolygonWinding.Clockwise;
                }
                if (signedDoubleArea > 0)
                {
                    return PolygonWinding.Counterclockwise;
                }
                return PolygonWinding.None;
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
        /// Determines if a Vector2 is inside or outside the Polygon
        /// </summary>
        /// <param name="inPoint"></param>
        /// <returns>True if the point is inside, false otherwise.</returns>
        public bool Contains(Vector2 inPoint)
        {
            // http://alienryderflex.com/polygon/
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
        public void OriginOnCentroid()
        {
            Offset(-Centroid);
        }

        private float SignedDoubleArea()
        {
            int index, nextIndex;
            int n = Vertices.Length;
            Vector2 point, next;
            float signedDoubleArea = 0;

            for (index = 0; index < n; ++index)
            {
                nextIndex = (index + 1) % n;
                point = Vertices[index];
                next = Vertices[nextIndex];
                signedDoubleArea += point.X * next.Y - next.X * point.Y;
            }

            return signedDoubleArea;
        }

        /// <summary>
        /// Rotates the Polygon of a specified amount
        /// </summary>
        public void Rotate(float inAngle, bool inRotateAroundCentroid)
        {
            float ca = MathF.Cos(inAngle);
            float sa = MathF.Sin(inAngle);

            Vector2 originalCentroid = Centroid;
            if (inRotateAroundCentroid && Centroid != Vector2.Zero)
            {
                OriginOnCentroid();
            }

            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector2 newVertex = new Vector2(
                    Vertices[i].X * ca - Vertices[i].Y * sa,
                    Vertices[i].X * sa + Vertices[i].Y * ca);

                Vertices[i] = newVertex;
            }

            if (inRotateAroundCentroid)
            {
                Offset(originalCentroid);
            }
        }
    }
}