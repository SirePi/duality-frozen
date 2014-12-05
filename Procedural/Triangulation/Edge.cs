// https://github.com/jceipek/Unity-delaunay

using System;
using System.Collections.Generic;
using Duality;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.Core.Geometry;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Triangulation
{
    /// <summary>
    ///
    /// </summary>
    public sealed class Edge
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly Edge DELETED = new Edge();

        private static int _nedges = 0;
        private static Stack<Edge> _pool = new Stack<Edge>();

        // Once clipVertices() is called, this Dictionary will hold two Points
        // representing the clipped coordinates of the left and right ends...
        private Dictionary<Side, Nullable<Vector2>> _clippedVertices;

        private int _edgeIndex;

        // the two Voronoi vertices that the edge connects
        //		(if one of them is null, the edge extends to infinity)
        private Vertex _leftVertex;

        private Vertex _rightVertex;

        // the two input Sites for which this Edge is a bisector:
        private Dictionary<Side, Site> _sites;

        private Edge()
        {
            _edgeIndex = _nedges++;
            Init();
        }

        // the equation of the edge: ax + by = c
        /// <summary>
        ///
        /// </summary>
        public float A { get; set; }
        /// <summary>
        ///
        /// </summary>
        public float B { get; set; }
        /// <summary>
        ///
        /// </summary>
        public float C { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Dictionary<Side, Nullable<Vector2>> ClippedEnds
        {
            get { return _clippedVertices; }
        }

        /// <summary>
        ///
        /// </summary>
        public Site LeftSite
        {
            get { return _sites[Side.Left]; }
            set { _sites[Side.Left] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Vertex LeftVertex
        {
            get { return _leftVertex; }
        }

        /// <summary>
        ///
        /// </summary>
        public Site RightSite
        {
            get { return _sites[Side.Right]; }
            set { _sites[Side.Right] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Vertex RightVertex
        {
            get { return _rightVertex; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool Visible
        {
            get { return _clippedVertices != null; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edge0"></param>
        /// <param name="edge1"></param>
        /// <returns></returns>
        public static int CompareSitesDistances(Edge edge0, Edge edge1)
        {
            return -CompareSitesDistances_MAX(edge0, edge1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edge0"></param>
        /// <param name="edge1"></param>
        /// <returns></returns>
        public static int CompareSitesDistances_MAX(Edge edge0, Edge edge1)
        {
            float length0 = edge0.SitesDistance();
            float length1 = edge1.SitesDistance();
            if (length0 < length1)
            {
                return 1;
            }
            if (length0 > length1)
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="site0"></param>
        /// <param name="site1"></param>
        /// <returns></returns>
        public static Edge CreateBisectingEdge(Site site0, Site site1)
        {
            float dx, dy, absdx, absdy;
            float a, b, c;

            dx = site1.Position.X - site0.Position.X;
            dy = site1.Position.Y - site0.Position.Y;
            absdx = dx > 0 ? dx : -dx;
            absdy = dy > 0 ? dy : -dy;
            c = site0.Position.X * dx + site0.Position.Y * dy + (dx * dx + dy * dy) * 0.5f;

            if (absdx > absdy)
            {
                a = 1.0f;
                b = dy / dx;
                c /= dx;
            }
            else
            {
                b = 1.0f;
                a = dx / dy;
                c /= dy;
            }

            Edge edge = Edge.Create();

            edge.LeftSite = site0;
            edge.RightSite = site1;
            site0.AddEdge(edge);
            site1.AddEdge(edge);

            edge._leftVertex = null;
            edge._rightVertex = null;

            edge.A = a;
            edge.B = b;
            edge.C = c;

            return edge;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bounds"></param>
        public void ClipVertices(Rect bounds)
        {
            float xmin = bounds.Left.X;
            float ymin = bounds.Top.Y;
            float xmax = bounds.Right.X;
            float ymax = bounds.Bottom.Y;

            Vertex vertex0, vertex1;
            float x0, x1, y0, y1;

            if (A == 1.0 && B >= 0.0)
            {
                vertex0 = _rightVertex;
                vertex1 = _leftVertex;
            }
            else
            {
                vertex0 = _leftVertex;
                vertex1 = _rightVertex;
            }

            if (A == 1.0)
            {
                y0 = ymin;
                if (vertex0 != null && vertex0.Position.Y > ymin)
                {
                    y0 = vertex0.Position.Y;
                }
                if (y0 > ymax)
                {
                    return;
                }
                x0 = C - B * y0;

                y1 = ymax;
                if (vertex1 != null && vertex1.Position.Y < ymax)
                {
                    y1 = vertex1.Position.Y;
                }
                if (y1 < ymin)
                {
                    return;
                }
                x1 = C - B * y1;

                if ((x0 > xmax && x1 > xmax) || (x0 < xmin && x1 < xmin))
                {
                    return;
                }

                if (x0 > xmax)
                {
                    x0 = xmax;
                    y0 = (C - x0) / B;
                }
                else if (x0 < xmin)
                {
                    x0 = xmin;
                    y0 = (C - x0) / B;
                }

                if (x1 > xmax)
                {
                    x1 = xmax;
                    y1 = (C - x1) / B;
                }
                else if (x1 < xmin)
                {
                    x1 = xmin;
                    y1 = (C - x1) / B;
                }
            }
            else
            {
                x0 = xmin;
                if (vertex0 != null && vertex0.Position.X > xmin)
                {
                    x0 = vertex0.Position.X;
                }
                if (x0 > xmax)
                {
                    return;
                }
                y0 = C - A * x0;

                x1 = xmax;
                if (vertex1 != null && vertex1.Position.X < xmax)
                {
                    x1 = vertex1.Position.X;
                }
                if (x1 < xmin)
                {
                    return;
                }
                y1 = C - A * x1;

                if ((y0 > ymax && y1 > ymax) || (y0 < ymin && y1 < ymin))
                {
                    return;
                }

                if (y0 > ymax)
                {
                    y0 = ymax;
                    x0 = (C - y0) / A;
                }
                else if (y0 < ymin)
                {
                    y0 = ymin;
                    x0 = (C - y0) / A;
                }

                if (y1 > ymax)
                {
                    y1 = ymax;
                    x1 = (C - y1) / A;
                }
                else if (y1 < ymin)
                {
                    y1 = ymin;
                    x1 = (C - y1) / A;
                }
            }

            //			_clippedVertices = new Dictionary(true); // XXX: Weak ref'd dict might be a problem to use standard
            _clippedVertices = new Dictionary<Side, Nullable<Vector2>>();
            if (vertex0 == _leftVertex)
            {
                _clippedVertices[Side.Left] = new Vector2(x0, y0);
                _clippedVertices[Side.Right] = new Vector2(x1, y1);
            }
            else
            {
                _clippedVertices[Side.Right] = new Vector2(x0, y0);
                _clippedVertices[Side.Left] = new Vector2(x1, y1);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public LineSegment DelaunayLine()
        {
            // draw a line connecting the input Sites for which the edge is a bisector:
            return new LineSegment(LeftSite.Position, RightSite.Position);
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            _leftVertex = null;
            _rightVertex = null;

            if (_clippedVertices != null)
            {
                _clippedVertices[Side.Left] = null;
                _clippedVertices[Side.Right] = null;
                _clippedVertices = null;
            }

            _sites[Side.Left] = null;
            _sites[Side.Right] = null;
            _sites = null;

            _pool.Push(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool IsPartOfConvexHull()
        {
            return (_leftVertex == null || _rightVertex == null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="leftRight"></param>
        /// <param name="v"></param>
        public void SetVertex(Side leftRight, Vertex v)
        {
            if (leftRight == Side.Left)
            {
                _leftVertex = v;
            }
            else
            {
                _rightVertex = v;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="leftRight"></param>
        /// <returns></returns>
        public Site Site(Side leftRight)
        {
            return _sites[leftRight];
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public float SitesDistance()
        {
            return (LeftSite.Position - RightSite.Position).Length;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Edge " + _edgeIndex.ToString() + "; sites " + _sites[Side.Left].ToString() + ", " + _sites[Side.Right].ToString()
                + "; endVertices " + ((_leftVertex != null) ? _leftVertex.VertexIndex.ToString() : "null") + ", "
                + ((_rightVertex != null) ? _rightVertex.VertexIndex.ToString() : "null") + "::";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="leftRight"></param>
        /// <returns></returns>
        public Vertex Vertex(Side leftRight)
        {
            return (leftRight == Side.Left) ? _leftVertex : _rightVertex;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public LineSegment VoronoiEdge()
        {
            if (!Visible)
                return new LineSegment(null, null);

            return new LineSegment(_clippedVertices[Side.Left], _clippedVertices[Side.Right]);
        }

        private static Edge Create()
        {
            Edge edge;
            if (_pool.Count > 0)
            {
                edge = _pool.Pop();
                edge.Init();
            }
            else
            {
                edge = new Edge();
            }
            return edge;
        }

        private void Init()
        {
            _sites = new Dictionary<Side, Site>();
        }
    }
}