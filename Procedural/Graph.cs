// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System.Collections.Generic;
using System.Linq;
using Duality;
using SnowyPeak.Duality.Plugin.Frozen.Core.Geometry;
using SnowyPeak.Duality.Plugin.Frozen.Procedural.Triangulation;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural
{
    /// <summary>
    ///
    /// </summary>
    public static class GraphExtensions
    {
        /// <summary>
        /// Checks if the graph is connected
        /// </summary>
        /// <param name="inGraph"></param>
        /// <returns></returns>
        public static bool IsConnected<T>(this Graph<T> inGraph) where T : class, INode
        {
            return Graph<T>.IsGraphConnected(inGraph.Links, inGraph.Nodes.Count);
        }
    }

    /// <summary>
    /// 2D Graph class modeled as a list of Nodes and a list of Directed/Undirected Links between Nodes.
    /// Node objects must implement INode interface in order to be able to assign X and Y coordinates that represent
    /// the position of the node.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Graph<T> where T : class, INode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Graph()
        {
            Nodes = new List<T>();
            Links = new List<Link<T>>();
        }

        /// <summary>
        /// The list of Links between Nodes
        /// </summary>
        public List<Link<T>> Links { get; private set; }
        /// <summary>
        /// The list of Nodes
        /// </summary>
        public List<T> Nodes { get; private set; }

        /// <summary>
        /// Checks if the Graph is connected
        /// http://en.wikipedia.org/wiki/Breadth-first_search#Finding_connected_components
        /// </summary>
        /// <param name="inGraph"></param>
        /// <param name="inNodesCount"></param>
        /// <returns></returns>
        public static bool IsGraphConnected(IEnumerable<Link<T>> inGraph, int inNodesCount)
        {
            Link<T> root = inGraph.ElementAt(0);
            List<INode> traversedNodes = new List<INode>();

            traversedNodes.Add(root.From);

            int i = 0;
            int traversedCount = 1;

            do
            {
                INode currentNode = traversedNodes[i];
                foreach (Link<T> link in inGraph.Where(l => l.From == currentNode))
                {
                    if (!traversedNodes.Contains(link.To))
                    {
                        traversedNodes.Add(link.To);
                    }
                }

                foreach (Link<T> link in inGraph.Where(l => l.To == currentNode && !l.IsDirected))
                {
                    if (!traversedNodes.Contains(link.From))
                    {
                        traversedNodes.Add(link.From);
                    }
                }

                i++;
                traversedCount = traversedNodes.Count;
            } while (i < traversedCount);

            return traversedNodes.Count == inNodesCount;
        }

        /// <summary>
        /// Executes Delaunay triangulation on the Nodes and creates the Links accordingly.
        /// Note that Links currently existing are not removed by the function.
        /// </summary>
        public void Triangulate()
        {
            Voronoi v = new Voronoi(Nodes, Rect.Empty);
            List<LineSegment> triangulation = v.DelaunayTriangulation();

            foreach (LineSegment line in triangulation)
            {
                T n1 = Nodes.Find(n => n.Position == line.P0.Value);
                T n2 = Nodes.Find(n => n.Position == line.P1.Value);

                Link<T> l = new Link<T>() { From = n1, To = n2, IsDirected = false, Weight = 1 };

                if (!Links.Contains(l))
                {
                    Links.Add(l);
                }
            }
        }
    }
}