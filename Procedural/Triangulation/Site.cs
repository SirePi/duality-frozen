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
    public sealed class Site : INode, IComparable
    {
        private static readonly float EPSILON_SQUARED = MathF.Pow(.005f, 2);

        private static Stack<Site> _pool = new Stack<Site>();

        // which end of each edge hooks up with the previous edge in _edges:
        private List<Side> _edgeOrientations;

        // the edges that define this Site's Voronoi region:
        private List<Edge> _edges;

        // ordered list of points that define the region clipped to bounds:
        private List<Vector2> _region;

        private uint _siteIndex;

        private Site(INode p, uint index, float weight)
        {
            Init(p, index, weight);
        }

        /// <summary>
        ///
        /// </summary>
        [Flags]
#pragma warning disable 1591
        private enum Bounds
        {
            None = 0x00,
            Top = 0x01,
            Bottom = 0x02,
            Left = 0x04,
            Right = 0x08
#pragma warning restore 1591
        }

        /// <summary>
        ///
        /// </summary>
        public Vector2 Position { get; private set; }
        /// <summary>
        ///
        /// </summary>
        public float Weight { get; set; }

        internal List<Edge> edges
        {
            get { return _edges; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="index"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static Site Create(INode p, uint index, float weight)
        {
            if (_pool.Count > 0)
            {
                return _pool.Pop().Init(p, index, weight);
            }
            else
            {
                return new Site(p, index, weight);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdge(Edge edge)
        {
            _edges.Add(edge);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(System.Object obj) // XXX: Really, really worried about this because it depends on how sorting works in AS3 impl - Julian
        {
            Site s2 = (Site)obj;

            int returnValue = Voronoi.CompareByYThenX(this, s2);

            // swap _siteIndex values if necessary to match new ordering:
            uint tempIndex;
            if (returnValue == -1)
            {
                if (this._siteIndex > s2._siteIndex)
                {
                    tempIndex = this._siteIndex;
                    this._siteIndex = s2._siteIndex;
                    s2._siteIndex = tempIndex;
                }
            }
            else if (returnValue == 1)
            {
                if (s2._siteIndex > this._siteIndex)
                {
                    tempIndex = s2._siteIndex;
                    s2._siteIndex = this._siteIndex;
                    this._siteIndex = tempIndex;
                }
            }

            return returnValue;
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            Clear();
            _pool.Push(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public float Dist(INode p)
        {
            return (p.Position - this.Position).Length;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Edge NearestEdge()
        {
            _edges.Sort(delegate(Edge a, Edge b)
            {
                return Edge.CompareSitesDistances(a, b);
            });
            return _edges[0];
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<Site> NeighborSites()
        {
            if (_edges == null || _edges.Count == 0)
            {
                return new List<Site>();
            }
            if (_edgeOrientations == null)
            {
                ReorderEdges();
            }
            List<Site> list = new List<Site>();
            Edge edge;
            for (int i = 0; i < _edges.Count; i++)
            {
                edge = _edges[i];
                list.Add(NeighborSite(edge));
            }
            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Site " + _siteIndex.ToString() + ": " + Position.ToString();
        }

        internal static void SortSites(List<Site> sites)
        {
            sites.Sort(); // XXX: Check if this works
        }

        internal List<Vector2> Region(Rect clippingBounds)
        {
            if (_edges == null || _edges.Count == 0)
            {
                return new List<Vector2>();
            }
            if (_edgeOrientations == null)
            {
                ReorderEdges();
                _region = ClipToBounds(clippingBounds);

                if (new Polygon(_region).Winding == Polygon.PolygonWinding.Clockwise)
                {
                    _region.Reverse();
                }
            }
            return _region;
        }

        private static bool CloseEnough(Vector2 p0, Vector2 p1)
        {
            return (p0 - p1).LengthSquared < EPSILON_SQUARED;
        }

        private Bounds CheckBounds(Vector2 point, Rect bounds)
        {
            Bounds result = Bounds.None;

            if (point.X == bounds.Left.X)
                result |= Bounds.Left;

            if (point.X == bounds.Right.X)
                result |= Bounds.Right;

            if (point.Y == bounds.Top.Y)
                result |= Bounds.Top;

            if (point.Y == bounds.Bottom.Y)
                result |= Bounds.Bottom;

            return result;
        }

        private void Clear()
        {
            if (_edges != null)
            {
                _edges.Clear();
                _edges = null;
            }
            if (_edgeOrientations != null)
            {
                _edgeOrientations.Clear();
                _edgeOrientations = null;
            }
            if (_region != null)
            {
                _region.Clear();
                _region = null;
            }
        }

        private List<Vector2> ClipToBounds(Rect bounds)
        {
            List<Vector2> points = new List<Vector2>();
            int n = _edges.Count;
            int i = 0;
            Edge edge;
            while (i < n && ((_edges[i] as Edge).Visible == false))
            {
                ++i;
            }

            if (i == n)
            {
                // no edges visible
                return new List<Vector2>();
            }
            edge = _edges[i];
            Side orientation = _edgeOrientations[i];

            if (edge.ClippedEnds[orientation] == null)
            {
                Log.Game.WriteError("XXX: Null detected when there should be a Vector2!");
            }
            if (edge.ClippedEnds[SideHelper.Other(orientation)] == null)
            {
                Log.Game.WriteError("XXX: Null detected when there should be a Vector2!");
            }
            points.Add((Vector2)edge.ClippedEnds[orientation]);
            points.Add((Vector2)edge.ClippedEnds[SideHelper.Other(orientation)]);

            for (int j = i + 1; j < n; ++j)
            {
                edge = _edges[j];
                if (edge.Visible == false)
                {
                    continue;
                }
                Connect(points, j, bounds);
            }
            // close up the polygon by adding another corner point of the bounds if needed:
            Connect(points, i, bounds, true);

            return points;
        }

        private void Connect(List<Vector2> points, int j, Rect bounds, bool closingUp = false)
        {
            Vector2 rightPoint = points[points.Count - 1];
            Edge newEdge = _edges[j] as Edge;
            Side newOrientation = _edgeOrientations[j];
            // the point that  must be connected to rightPoint:
            if (newEdge.ClippedEnds[newOrientation] == null)
            {
                Log.Game.WriteError("XXX: Null detected when there should be a Vector2!");
            }
            Vector2 newPoint = (Vector2)newEdge.ClippedEnds[newOrientation];
            if (!CloseEnough(rightPoint, newPoint))
            {
                // The points do not coincide, so they must have been clipped at the bounds;
                // see if they are on the same border of the bounds:
                if (rightPoint.X != newPoint.X
                    && rightPoint.Y != newPoint.Y)
                {
                    // They are on different borders of the bounds;
                    // insert one or two corners of bounds as needed to hook them up:
                    // (NOTE this will not be correct if the region should take up more than
                    // half of the bounds rect, for then we will have gone the wrong way
                    // around the bounds and included the smaller part rather than the larger)
                    Bounds rightCheck = CheckBounds(rightPoint, bounds);
                    Bounds newCheck = CheckBounds(newPoint, bounds);

                    float px, py;
                    if ((rightCheck & Bounds.Right) != Bounds.None)
                    {
                        px = bounds.Right.X;

                        if ((newCheck & Bounds.Bottom) != Bounds.None)
                        {
                            py = bounds.Bottom.Y;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & Bounds.Top) != Bounds.None)
                        {
                            py = bounds.Top.Y;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & Bounds.Left) != Bounds.None)
                        {
                            if (rightPoint.Y - bounds.Y + newPoint.Y - bounds.Y < bounds.H)
                            {
                                py = bounds.Top.Y;
                            }
                            else
                            {
                                py = bounds.Bottom.Y;
                            }

                            points.Add(new Vector2(px, py));
                            points.Add(new Vector2(bounds.Right.X, py));
                        }
                    }
                    else if ((rightCheck & Bounds.Left) != Bounds.None)
                    {
                        px = bounds.Left.X;
                        if ((newCheck & Bounds.Bottom) != Bounds.None)
                        {
                            py = bounds.Bottom.Y;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & Bounds.Top) != Bounds.None)
                        {
                            py = bounds.Top.Y;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & Bounds.Right) != Bounds.None)
                        {
                            if (rightPoint.Y - bounds.Y + newPoint.Y - bounds.Y < bounds.H)
                            {
                                py = bounds.Top.Y;
                            }
                            else
                            {
                                py = bounds.Bottom.Y;
                            }
                            points.Add(new Vector2(px, py));
                            points.Add(new Vector2(bounds.Right.X, py));
                        }
                    }
                    else if ((rightCheck & Bounds.Top) != Bounds.None)
                    {
                        py = bounds.Top.Y;
                        if ((newCheck & Bounds.Right) != Bounds.None)
                        {
                            px = bounds.Right.X;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & Bounds.Left) != Bounds.None)
                        {
                            px = bounds.Left.X;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & Bounds.Bottom) != Bounds.None)
                        {
                            if (rightPoint.X - bounds.X + newPoint.X - bounds.X < bounds.W)
                            {
                                px = bounds.Left.X;
                            }
                            else
                            {
                                px = bounds.Right.X;
                            }
                            points.Add(new Vector2(px, py));
                            points.Add(new Vector2(px, bounds.Bottom.Y));
                        }
                    }
                    else if ((rightCheck & Bounds.Bottom) != Bounds.None)
                    {
                        py = bounds.Bottom.Y;
                        if ((newCheck & Bounds.Right) != Bounds.None)
                        {
                            px = bounds.Right.X;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & Bounds.Left) != Bounds.None)
                        {
                            px = bounds.Left.X;
                            points.Add(new Vector2(px, py));
                        }
                        else if ((newCheck & Bounds.Top) != Bounds.None)
                        {
                            if (rightPoint.X - bounds.X + newPoint.X - bounds.X < bounds.W)
                            {
                                px = bounds.Left.X;
                            }
                            else
                            {
                                px = bounds.Right.X;
                            }
                            points.Add(new Vector2(px, py));
                            points.Add(new Vector2(px, bounds.Top.Y));
                        }
                    }
                }
                if (closingUp)
                {
                    // newEdge's ends have already been added
                    return;
                }
                points.Add(newPoint);
            }

            if (newEdge.ClippedEnds[SideHelper.Other(newOrientation)] == null)
            {
                Log.Game.WriteError("XXX: Null detected when there should be a Vector2!");
            }

            Vector2 newRightPoint = (Vector2)newEdge.ClippedEnds[SideHelper.Other(newOrientation)];
            if (!CloseEnough(points[0], newRightPoint))
            {
                points.Add(newRightPoint);
            }
        }

        private Site Init(INode p, uint index, float weight)
        {
            Position = p.Position;
            Weight = weight;

            _siteIndex = index;
            _edges = new List<Edge>();
            _region = null;

            return this;
        }

        private void Move(Vector2 p)
        {
            Clear();
            Position = p;
        }

        private Site NeighborSite(Edge edge)
        {
            if (this == edge.LeftSite)
            {
                return edge.RightSite;
            }
            if (this == edge.RightSite)
            {
                return edge.LeftSite;
            }
            return null;
        }

        private void ReorderEdges()
        {
            //trace("_edges:", _edges);
            EdgeReorderer reorderer = new EdgeReorderer(_edges, VertexOrSite.Vertex);
            _edges = reorderer.edges;
            //trace("reordered:", _edges);
            _edgeOrientations = reorderer.edgeOrientations;
            reorderer.Dispose();
        }
    }
}