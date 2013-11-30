using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

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

        public Vector2 this [int i]
        {
            get { return Vertices[i]; }
            set 
            { 
                Vertices[i].X = value.X;
                Vertices[i].Y = value.Y;
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
    }
}
