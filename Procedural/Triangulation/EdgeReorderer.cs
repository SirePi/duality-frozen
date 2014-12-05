using System;
using System.Collections.Generic;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Triangulation
{
    /// <summary>
    ///
    /// </summary>
    public enum VertexOrSite
    {
#pragma warning disable 1591
        Vertex,
        Site
#pragma warning restore 1591
    }

    internal sealed class EdgeReorderer : IDisposable
    {
        private List<Side> _edgeOrientations;
        private List<Edge> _edges;

        /// <summary>
        ///
        /// </summary>
        /// <param name="origEdges"></param>
        /// <param name="criterion"></param>
        public EdgeReorderer(List<Edge> origEdges, VertexOrSite criterion)
        {
            _edges = new List<Edge>();
            _edgeOrientations = new List<Side>();
            if (origEdges.Count > 0)
            {
                _edges = ReorderEdges(origEdges, criterion);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public List<Side> edgeOrientations
        {
            get { return _edgeOrientations; }
        }

        /// <summary>
        ///
        /// </summary>
        public List<Edge> edges
        {
            get { return _edges; }
        }

        public void Dispose()
        {
            _edges = null;
            _edgeOrientations = null;
        }

        private List<Edge> ReorderEdges(List<Edge> origEdges, VertexOrSite criterion)
        {
            int i;
            int n = origEdges.Count;
            Edge edge;
            // we're going to reorder the edges in order of traversal
            bool[] done = new bool[n];
            int nDone = 0;
            for (int j = 0; j < n; j++)
            {
                done[j] = false;
            }
            List<Edge> newEdges = new List<Edge>(); // TODO: Switch to Deque if performance is a concern

            i = 0;
            edge = origEdges[i];
            newEdges.Add(edge);
            _edgeOrientations.Add(Side.Left);
            INode firstPoint = (criterion == VertexOrSite.Vertex) ? (INode)edge.LeftVertex : (INode)edge.LeftSite;
            INode lastPoint = (criterion == VertexOrSite.Vertex) ? (INode)edge.RightVertex : (INode)edge.RightSite;

            if (firstPoint == Vertex.VERTEX_AT_INFINITY || lastPoint == Vertex.VERTEX_AT_INFINITY)
            {
                return new List<Edge>();
            }

            done[i] = true;
            ++nDone;

            while (nDone < n)
            {
                for (i = 1; i < n; ++i)
                {
                    if (done[i])
                    {
                        continue;
                    }
                    edge = origEdges[i];
                    INode leftPoint = (criterion == VertexOrSite.Vertex) ? (INode)edge.LeftVertex : (INode)edge.LeftSite;
                    INode rightPoint = (criterion == VertexOrSite.Vertex) ? (INode)edge.RightVertex : (INode)edge.RightSite;
                    if (leftPoint == Vertex.VERTEX_AT_INFINITY || rightPoint == Vertex.VERTEX_AT_INFINITY)
                    {
                        return new List<Edge>();
                    }
                    if (leftPoint == lastPoint)
                    {
                        lastPoint = rightPoint;
                        _edgeOrientations.Add(Side.Left);
                        newEdges.Add(edge);
                        done[i] = true;
                    }
                    else if (rightPoint == firstPoint)
                    {
                        firstPoint = leftPoint;
                        _edgeOrientations.Insert(0, Side.Left); // TODO: Change datastructure if this is slow
                        newEdges.Insert(0, edge);
                        done[i] = true;
                    }
                    else if (leftPoint == firstPoint)
                    {
                        firstPoint = rightPoint;
                        _edgeOrientations.Insert(0, Side.Right);
                        newEdges.Insert(0, edge);
                        done[i] = true;
                    }
                    else if (rightPoint == lastPoint)
                    {
                        lastPoint = leftPoint;
                        _edgeOrientations.Add(Side.Right);
                        newEdges.Add(edge);
                        done[i] = true;
                    }
                    if (done[i])
                    {
                        ++nDone;
                    }
                }
            }

            return newEdges;
        }
    }
}