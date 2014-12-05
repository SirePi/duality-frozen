using System;
using System.Collections.Generic;
using OpenTK;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Triangulation
{
    /// <summary>
    ///
    /// </summary>
    public sealed class Vertex : INode
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly Vertex VERTEX_AT_INFINITY = new Vertex(float.NaN, float.NaN);

        private static int _nvertices = 0;
        private static Stack<Vertex> _pool = new Stack<Vertex>();

        private int _vertexIndex;

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vertex(float x, float y)
        {
            Init(x, y);
        }

        /// <summary>
        ///
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public int VertexIndex
        {
            get { return _vertexIndex; }
        }

        /// <summary>
        /// This is the only way to make a Vertex
        /// </summary>
        /// <param name="halfedge0"></param>
        /// <param name="halfedge1"></param>
        /// <returns></returns>
        public static Vertex Intersect(Halfedge halfedge0, Halfedge halfedge1)
        {
            Edge edge0, edge1, edge;
            Halfedge halfedge;
            float determinant, intersectionX, intersectionY;
            bool rightOfSite;

            edge0 = halfedge0.Edge;
            edge1 = halfedge1.Edge;
            if (edge0 == null || edge1 == null)
            {
                return null;
            }
            if (edge0.RightSite == edge1.RightSite)
            {
                return null;
            }

            determinant = edge0.A * edge1.B - edge0.B * edge1.A;
            if (-1.0e-10 < determinant && determinant < 1.0e-10)
            {
                // the edges are parallel
                return null;
            }

            intersectionX = (edge0.C * edge1.B - edge1.C * edge0.B) / determinant;
            intersectionY = (edge1.C * edge0.A - edge0.C * edge1.A) / determinant;

            if (Voronoi.CompareByYThenX(edge0.RightSite, edge1.RightSite) < 0)
            {
                halfedge = halfedge0;
                edge = edge0;
            }
            else
            {
                halfedge = halfedge1;
                edge = edge1;
            }

            rightOfSite = intersectionX >= edge.RightSite.Position.X;
            if ((rightOfSite && halfedge.LeftRight == Side.Left) || (!rightOfSite && halfedge.LeftRight == Side.Right))
            {
                return null;
            }

            return Vertex.Create(intersectionX, intersectionY);
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            _pool.Push(this);
        }

        /// <summary>
        ///
        /// </summary>
        public void SetIndex()
        {
            _vertexIndex = _nvertices++;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Vertex (" + _vertexIndex + ")";
        }

        private static Vertex Create(float x, float y)
        {
            if (float.IsNaN(x) || float.IsNaN(y))
            {
                return VERTEX_AT_INFINITY;
            }
            if (_pool.Count > 0)
            {
                return _pool.Pop().Init(x, y);
            }
            else
            {
                return new Vertex(x, y);
            }
        }

        private Vertex Init(float x, float y)
        {
            Position = new Vector2(x, y);
            return this;
        }
    }
}