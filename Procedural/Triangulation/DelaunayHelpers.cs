using System;
using System.Collections.Generic;
using Duality;
using SnowyPeak.Duality.Plugin.Frozen.Core.Geometry;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Triangulation
{
    /// <summary>
    ///
    /// </summary>
    public enum KruskalType
    {
#pragma warning disable 1591
        Minimum,
        Maximum
#pragma warning restore 1591
    }

    /// <summary>
    ///
    /// </summary>
    public static class DelaunayHelpers
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static List<LineSegment> DelaunayLinesForEdges(List<Edge> edges)
        {
            List<LineSegment> segments = new List<LineSegment>();
            Edge edge;
            for (int i = 0; i < edges.Count; i++)
            {
                edge = edges[i];
                segments.Add(edge.DelaunayLine());
            }
            return segments;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lineSegments"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<LineSegment> Kruskal(List<LineSegment> lineSegments, KruskalType type = KruskalType.Minimum)
        {
            Dictionary<Nullable<Vector2>, Node> nodes = new Dictionary<Nullable<Vector2>, Node>();
            List<LineSegment> mst = new List<LineSegment>();
            Stack<Node> nodePool = Node.POOL;

            switch (type)
            {
                // note that the compare functions are the reverse of what you'd expect
                // because (see below) we traverse the lineSegments in reverse order for speed
                case KruskalType.Maximum:
                    lineSegments.Sort(delegate(LineSegment l1, LineSegment l2)
                    {
                        return LineSegment.CompareLengths(l1, l2);
                    });
                    break;

                default:
                    lineSegments.Sort(delegate(LineSegment l1, LineSegment l2)
                    {
                        return LineSegment.CompareLengths_MAX(l1, l2);
                    });
                    break;
            }

            for (int i = lineSegments.Count; --i > -1; )
            {
                LineSegment lineSegment = lineSegments[i];

                Node node0 = null;
                Node rootOfSet0;
                if (!nodes.ContainsKey(lineSegment.P0))
                {
                    node0 = nodePool.Count > 0 ? nodePool.Pop() : new Node();
                    // intialize the node:
                    rootOfSet0 = node0.Parent = node0;
                    node0.TreeSize = 1;

                    nodes[lineSegment.P0] = node0;
                }
                else
                {
                    node0 = nodes[lineSegment.P0];
                    rootOfSet0 = Find(node0);
                }

                Node node1 = null;
                Node rootOfSet1;
                if (!nodes.ContainsKey(lineSegment.P1))
                {
                    node1 = nodePool.Count > 0 ? nodePool.Pop() : new Node();
                    // intialize the node:
                    rootOfSet1 = node1.Parent = node1;
                    node1.TreeSize = 1;

                    nodes[lineSegment.P1] = node1;
                }
                else
                {
                    node1 = nodes[lineSegment.P1];
                    rootOfSet1 = Find(node1);
                }

                if (rootOfSet0 != rootOfSet1)
                {	// nodes not in same set
                    mst.Add(lineSegment);

                    // merge the two sets:
                    int treeSize0 = rootOfSet0.TreeSize;
                    int treeSize1 = rootOfSet1.TreeSize;
                    if (treeSize0 >= treeSize1)
                    {
                        // set0 absorbs set1:
                        rootOfSet1.Parent = rootOfSet0;
                        rootOfSet0.TreeSize += treeSize1;
                    }
                    else
                    {
                        // set1 absorbs set0:
                        rootOfSet0.Parent = rootOfSet1;
                        rootOfSet1.TreeSize += treeSize0;
                    }
                }
            }
            foreach (Node node in nodes.Values)
            {
                nodePool.Push(node);
            }

            return mst;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="edgesToTest"></param>
        /// <returns></returns>
        public static List<Edge> SelectEdgesForSitePoint(Vector2 coord, List<Edge> edgesToTest)
        {
            return edgesToTest.FindAll(delegate(Edge edge)
            {
                return ((edge.LeftSite != null && edge.LeftSite.Position == coord)
                    || (edge.RightSite != null && edge.RightSite.Position == coord));
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static List<LineSegment> VisibleLineSegments(List<Edge> edges)
        {
            List<LineSegment> segments = new List<LineSegment>();

            for (int i = 0; i < edges.Count; i++)
            {
                Edge edge = edges[i];
                if (edge.Visible)
                {
                    Nullable<Vector2> p1 = edge.ClippedEnds[Side.Left];
                    Nullable<Vector2> p2 = edge.ClippedEnds[Side.Right];
                    segments.Add(new LineSegment(p1, p2));
                }
            }

            return segments;
        }

        private static Node Find(Node node)
        {
            if (node.Parent == node)
            {
                return node;
            }
            else
            {
                Node root = Find(node.Parent);
                // this line is just to speed up subsequent finds by keeping the tree depth low:
                node.Parent = root;
                return root;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class Node
    {
        /// <summary>
        ///
        /// </summary>
        public static Stack<Node> POOL = new Stack<Node>();

        /// <summary>
        ///
        /// </summary>
        public Node Parent { get; set; }
        /// <summary>
        ///
        /// </summary>
        public int TreeSize { get; set; }
    }
}