using System;
using System.Collections.Generic;
using OpenTK;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Triangulation
{
    /// <summary>
    ///
    /// </summary>
    public sealed class Halfedge : IDisposable
    {
        private static Stack<Halfedge> _pool = new Stack<Halfedge>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="lr"></param>
        public Halfedge(Edge edge = null, Nullable<Side> lr = null)
        {
            Init(edge, lr);
        }

        /// <summary>
        ///
        /// </summary>
        public Edge Edge { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Halfedge EdgeListLeftNeighbor { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Halfedge EdgeListRightNeighbor { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Nullable<Side> LeftRight { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Halfedge NextInPriorityQueue { get; set; }
        /// <summary>
        ///
        /// </summary>
        public Vertex Vertex { get; set; }

        /// <summary>
        /// the vertex's y-coordinate in the transformed Voronoi space V*
        /// </summary>
        public float YStar { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="lr"></param>
        /// <returns></returns>
        public static Halfedge Create(Edge edge, Nullable<Side> lr)
        {
            if (_pool.Count > 0)
            {
                return _pool.Pop().Init(edge, lr);
            }
            else
            {
                return new Halfedge(edge, lr);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static Halfedge CreateDummy()
        {
            return Create(null, null);
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            if (EdgeListLeftNeighbor != null || EdgeListRightNeighbor != null)
            {
                // still in EdgeList
                return;
            }
            if (NextInPriorityQueue != null)
            {
                // still in PriorityQueue
                return;
            }
            Edge = null;
            LeftRight = null;
            Vertex = null;
            _pool.Push(this);
        }

        /// <summary>
        ///
        /// </summary>
        public void ReallyDispose()
        {
            EdgeListLeftNeighbor = null;
            EdgeListRightNeighbor = null;
            NextInPriorityQueue = null;
            Edge = null;
            LeftRight = null;
            Vertex = null;
            _pool.Push(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Halfedge (leftRight: " + LeftRight.ToString() + "; vertex: " + Vertex.ToString() + ")";
        }

        internal bool IsLeftOf(Vector2 p)
        {
            Site topSite;
            bool rightOfSite, above, fast;
            float dxp, dyp, dxs, t1, t2, t3, yl;

            topSite = Edge.RightSite;
            rightOfSite = p.X > topSite.Position.X;
            if (rightOfSite && this.LeftRight == Side.Left)
            {
                return true;
            }
            if (!rightOfSite && this.LeftRight == Side.Right)
            {
                return false;
            }

            if (Edge.A == 1.0)
            {
                dyp = p.Y - topSite.Position.Y;
                dxp = p.X - topSite.Position.X;
                fast = false;
                if ((!rightOfSite && Edge.B < 0.0) || (rightOfSite && Edge.B >= 0.0))
                {
                    above = dyp >= Edge.B * dxp;
                    fast = above;
                }
                else
                {
                    above = p.X + p.Y * Edge.B > Edge.C;
                    if (Edge.B < 0.0)
                    {
                        above = !above;
                    }
                    if (!above)
                    {
                        fast = true;
                    }
                }
                if (!fast)
                {
                    dxs = topSite.Position.X - Edge.LeftSite.Position.X;
                    above = Edge.B * (dxp * dxp - dyp * dyp) <
                        dxs * dyp * (1.0 + 2.0 * dxp / dxs + Edge.B * Edge.B);
                    if (Edge.B < 0.0)
                    {
                        above = !above;
                    }
                }
            }
            else
            {  /* edge.b == 1.0 */
                yl = Edge.C - Edge.A * p.X;
                t1 = p.Y - yl;
                t2 = p.X - topSite.Position.X;
                t3 = yl - topSite.Position.Y;
                above = t1 * t1 > t2 * t2 + t3 * t3;
            }
            return this.LeftRight == Side.Left ? above : !above;
        }

        private Halfedge Init(Edge edge, Nullable<Side> lr)
        {
            this.Edge = edge;
            LeftRight = lr;
            NextInPriorityQueue = null;
            Vertex = null;
            return this;
        }
    }
}