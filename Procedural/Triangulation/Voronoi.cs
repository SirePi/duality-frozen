/*
 * The author of this software is Steven Fortune.  Copyright (c) 1994 by AT&T
 * Bell Laboratories.
 * Permission to use, copy, modify, and distribute this software for any
 * purpose without fee is hereby granted, provided that this entire notice
 * is included in all copies of any software which is or includes a copy
 * or modification of this software and in all copies of the supporting
 * documentation for such software.
 * THIS SOFTWARE IS BEING PROVIDED "AS IS", WITHOUT ANY EXPRESS OR IMPLIED
 * WARRANTY.  IN PARTICULAR, NEITHER THE AUTHORS NOR AT&T MAKE ANY
 * REPRESENTATION OR WARRANTY OF ANY KIND CONCERNING THE MERCHANTABILITY
 * OF THIS SOFTWARE OR ITS FITNESS FOR ANY PARTICULAR PURPOSE.
 */

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
    public sealed class Voronoi : IDisposable
    {
        private List<Edge> _edges;

        // TODO generalize this so it doesn't have to be a rectangle;
        // then we can make the fractal voronois-within-voronois

        private SiteList _sites;
        private Dictionary<INode, Site> _sitesIndexedByLocation;
        private List<Triangle> _triangles;
        private Site fortunesAlgorithm_bottomMostSite;

        /// <summary>
        ///
        /// </summary>
        /// <param name="points"></param>
        /// <param name="plotBounds"></param>
        public Voronoi(IEnumerable<INode> points, Rect plotBounds)
        {
            _sites = new SiteList();
            _sitesIndexedByLocation = new Dictionary<INode, Site>(); // XXX: Used to be Dictionary(true) -- weak refs.

            AddSites(points);

            PlotBounds = plotBounds;
            _triangles = new List<Triangle>();
            _edges = new List<Edge>();

            FortunesAlgorithm();
        }

        /// <summary>
        ///
        /// </summary>
        public Rect PlotBounds { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static int CompareByYThenX(Site s1, Site s2)
        {
            if (s1.Position.Y < s2.Position.Y)
                return -1;
            if (s1.Position.Y > s2.Position.Y)
                return 1;
            if (s1.Position.X < s2.Position.X)
                return -1;
            if (s1.Position.X > s2.Position.X)
                return 1;
            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static int CompareByYThenX(Site s1, Vector2 s2)
        {
            if (s1.Position.Y < s2.Y)
                return -1;
            if (s1.Position.Y > s2.Y)
                return 1;
            if (s1.Position.X < s2.X)
                return -1;
            if (s1.Position.X > s2.X)
                return 1;
            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<Circle> Circles()
        {
            return _sites.Circles();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public List<LineSegment> DelaunayLinesForSite(Vector2 coord)
        {
            return DelaunayHelpers.DelaunayLinesForEdges(DelaunayHelpers.SelectEdgesForSitePoint(coord, _edges));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<LineSegment> DelaunayTriangulation()
        {
            return DelaunayHelpers.DelaunayLinesForEdges(_edges);
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            if (_sites != null)
            {
                _sites.Dispose();
                _sites = null;
            }

            if (_triangles != null)
            {
                for (int i = 0; i < _triangles.Count; ++i)
                {
                    _triangles[i].Dispose();
                }
                _triangles.Clear();
                _triangles = null;
            }

            if (_edges != null)
            {
                for (int i = 0; i < _edges.Count; ++i)
                {
                    _edges[i].Dispose();
                }

                _edges.Clear();
                _edges = null;
            }

            _sitesIndexedByLocation = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<Edge> Edges()
        {
            return _edges;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<LineSegment> Hull()
        {
            return DelaunayHelpers.DelaunayLinesForEdges(HullEdges());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<Vector2> HullPointsInOrder()
        {
            List<Edge> hullEdges = HullEdges();

            List<Vector2> points = new List<Vector2>();
            if (hullEdges.Count == 0)
            {
                return points;
            }

            EdgeReorderer reorderer = new EdgeReorderer(hullEdges, VertexOrSite.Site);
            hullEdges = reorderer.edges;
            List<Side> orientations = reorderer.edgeOrientations;
            reorderer.Dispose();

            Side orientation;

            int n = hullEdges.Count;
            for (int i = 0; i < n; ++i)
            {
                Edge edge = hullEdges[i];
                orientation = orientations[i];
                points.Add(edge.Site(orientation).Position);
            }
            return points;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Nullable<Vector2> NearestSitePoint(float x, float y)
        {
            return _sites.NearestSitePoint(x, y);
        }

        // TODO: bug: if you call this before you call region(), something goes wrong :(
        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public List<Vector2> NeighborSitesForSite(INode coord)
        {
            List<Vector2> points = new List<Vector2>();
            Site site = _sitesIndexedByLocation[coord];
            if (site == null)
            {
                return points;
            }
            List<Site> sites = site.NeighborSites();
            Site neighbor;
            for (int nIndex = 0; nIndex < sites.Count; nIndex++)
            {
                neighbor = sites[nIndex];
                points.Add(neighbor.Position);
            }
            return points;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<Vector2> Region(INode p)
        {
            Site site = _sitesIndexedByLocation[p];
            if (site == null)
            {
                return new List<Vector2>();
            }
            return site.Region(PlotBounds);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<List<Vector2>> Regions()
        {
            return _sites.Regions(PlotBounds);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<Vector2> SiteCoords()
        {
            return _sites.SiteCoords();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<LineSegment> SpanningTree(KruskalType type = KruskalType.Minimum)
        {
            List<LineSegment> segments = DelaunayHelpers.DelaunayLinesForEdges(_edges);
            return DelaunayHelpers.Kruskal(segments, type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public List<LineSegment> VoronoiBoundaryForSite(Vector2 coord)
        {
            return DelaunayHelpers.VisibleLineSegments(DelaunayHelpers.SelectEdgesForSitePoint(coord, _edges));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<LineSegment> VoronoiDiagram()
        {
            return DelaunayHelpers.VisibleLineSegments(_edges);
        }

        private void AddSite(INode p, int index, float weight)
        {
            if (_sitesIndexedByLocation.ContainsKey(p))
                return; // Prevent duplicate site! (Adapted from https://github.com/nodename/as3delaunay/issues/1)

            Site site = Site.Create(p, (uint)index, weight);
            _sites.Add(site);
            _sitesIndexedByLocation[p] = site;
        }

        private void AddSites(IEnumerable<INode> points)
        {
            int i = 0;
            foreach (INode node in points)
            {
                AddSite(node, i, i);
                i++;
            }
        }

        private void FortunesAlgorithm()
        {
            Site newSite, bottomSite, topSite, tempSite;
            Vertex v, vertex;
            Vector2 newintstar = Vector2.Zero; //Because the compiler doesn't know that it will have a value - Julian
            Side leftRight;
            Halfedge lbnd, rbnd, llbnd, rrbnd, bisector;
            Edge edge;

            Rect dataBounds = _sites.GetSitesBounds();

            int sqrt_nsites = (int)(MathF.Sqrt(_sites.Count + 4));
            HalfedgePriorityQueue heap = new HalfedgePriorityQueue(dataBounds.Y, dataBounds.H, sqrt_nsites);
            EdgeList edgeList = new EdgeList(dataBounds.X, dataBounds.W, sqrt_nsites);
            List<Halfedge> halfEdges = new List<Halfedge>();
            List<Vertex> vertices = new List<Vertex>();

            fortunesAlgorithm_bottomMostSite = _sites.Next();
            newSite = _sites.Next();

            while (true)
            {
                if (!heap.Empty())
                {
                    newintstar = heap.Min();
                }

                if (newSite != null && (heap.Empty() || CompareByYThenX(newSite, newintstar) < 0))
                {
                    /* new site is smallest */
                    //trace("smallest: new site " + newSite);

                    // Step 8:
                    lbnd = edgeList.EdgeListLeftNeighbor(newSite.Position);	// the Halfedge just to the left of newSite
                    //trace("lbnd: " + lbnd);
                    rbnd = lbnd.EdgeListRightNeighbor;		// the Halfedge just to the right
                    //trace("rbnd: " + rbnd);
                    bottomSite = FortunesAlgorithm_rightRegion(lbnd);		// this is the same as leftRegion(rbnd)
                    // this Site determines the region containing the new site
                    //trace("new Site is in region of existing site: " + bottomSite);

                    // Step 9:
                    edge = Edge.CreateBisectingEdge(bottomSite, newSite);
                    //trace("new edge: " + edge);
                    _edges.Add(edge);

                    bisector = Halfedge.Create(edge, Side.Left);
                    halfEdges.Add(bisector);
                    // inserting two Halfedges into edgeList constitutes Step 10:
                    // insert bisector to the right of lbnd:
                    edgeList.Insert(lbnd, bisector);

                    // first half of Step 11:
                    if ((vertex = Vertex.Intersect(lbnd, bisector)) != null)
                    {
                        vertices.Add(vertex);
                        heap.Remove(lbnd);
                        lbnd.Vertex = vertex;
                        lbnd.YStar = vertex.Position.Y + newSite.Dist(vertex);
                        heap.Insert(lbnd);
                    }

                    lbnd = bisector;
                    bisector = Halfedge.Create(edge, Side.Right);
                    halfEdges.Add(bisector);
                    // second Halfedge for Step 10:
                    // insert bisector to the right of lbnd:
                    edgeList.Insert(lbnd, bisector);

                    // second half of Step 11:
                    if ((vertex = Vertex.Intersect(bisector, rbnd)) != null)
                    {
                        vertices.Add(vertex);
                        bisector.Vertex = vertex;
                        bisector.YStar = vertex.Position.Y + newSite.Dist(vertex);
                        heap.Insert(bisector);
                    }

                    newSite = _sites.Next();
                }
                else if (!heap.Empty())
                {
                    /* intersection is smallest */
                    lbnd = heap.ExtractMin();
                    llbnd = lbnd.EdgeListLeftNeighbor;
                    rbnd = lbnd.EdgeListRightNeighbor;
                    rrbnd = rbnd.EdgeListRightNeighbor;
                    bottomSite = FortunesAlgorithm_leftRegion(lbnd);
                    topSite = FortunesAlgorithm_rightRegion(rbnd);
                    // these three sites define a triangle
                    // (not actually using these for anything...)
                    //_triangles.push(new Triangle(bottomSite, topSite, rightRegion(lbnd)));

                    v = lbnd.Vertex;
                    v.SetIndex();
                    lbnd.Edge.SetVertex((Side)lbnd.LeftRight, v);
                    rbnd.Edge.SetVertex((Side)rbnd.LeftRight, v);
                    edgeList.Remove(lbnd);
                    heap.Remove(rbnd);
                    edgeList.Remove(rbnd);
                    leftRight = Side.Left;
                    if (bottomSite.Position.Y > topSite.Position.Y)
                    {
                        tempSite = bottomSite;
                        bottomSite = topSite;
                        topSite = tempSite;
                        leftRight = Side.Right;
                    }
                    edge = Edge.CreateBisectingEdge(bottomSite, topSite);
                    _edges.Add(edge);
                    bisector = Halfedge.Create(edge, leftRight);
                    halfEdges.Add(bisector);
                    edgeList.Insert(llbnd, bisector);
                    edge.SetVertex(SideHelper.Other(leftRight), v);

                    if ((vertex = Vertex.Intersect(llbnd, bisector)) != null)
                    {
                        vertices.Add(vertex);
                        heap.Remove(llbnd);
                        llbnd.Vertex = vertex;
                        llbnd.YStar = vertex.Position.Y + bottomSite.Dist(vertex);
                        heap.Insert(llbnd);
                    }

                    if ((vertex = Vertex.Intersect(bisector, rrbnd)) != null)
                    {
                        vertices.Add(vertex);
                        bisector.Vertex = vertex;
                        bisector.YStar = vertex.Position.Y + bottomSite.Dist(vertex);
                        heap.Insert(bisector);
                    }
                }
                else
                {
                    break;
                }
            }

            // heap should be empty now
            heap.Dispose();
            edgeList.Dispose();

            for (int hIndex = 0; hIndex < halfEdges.Count; hIndex++)
            {
                Halfedge halfEdge = halfEdges[hIndex];
                halfEdge.ReallyDispose();
            }
            halfEdges.Clear();

            // we need the vertices to clip the edges
            for (int eIndex = 0; eIndex < _edges.Count; eIndex++)
            {
                edge = _edges[eIndex];
                edge.ClipVertices(PlotBounds);
            }
            // but we don't actually ever use them again!
            for (int vIndex = 0; vIndex < vertices.Count; vIndex++)
            {
                vertex = vertices[vIndex];
                vertex.Dispose();
            }
            vertices.Clear();
        }

        private Site FortunesAlgorithm_leftRegion(Halfedge he)
        {
            Edge edge = he.Edge;
            if (edge == null)
            {
                return fortunesAlgorithm_bottomMostSite;
            }
            return edge.Site((Side)he.LeftRight);
        }

        private Site FortunesAlgorithm_rightRegion(Halfedge he)
        {
            Edge edge = he.Edge;
            if (edge == null)
            {
                return fortunesAlgorithm_bottomMostSite;
            }
            return edge.Site(SideHelper.Other((Side)he.LeftRight));
        }

        private List<Edge> HullEdges()
        {
            return _edges.FindAll(delegate(Edge edge)
            {
                return (edge.IsPartOfConvexHull());
            });
        }
    }
}