// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System.Collections.Generic;
using System.Linq;

namespace FrozenCore.Data.Graph
{
    /// <summary>
    /// Graph class modeled as a list of Nodes and a list of Directed/Undirected Links between Nodes.
    /// Node objects must implement INode interface in order to be able to assign X and Y coordinates that represent
    /// the position of the node.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Graph<T> where T : class, INode
    {
        public List<Link<T>> Links { get; private set; }
        public List<T> Nodes { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Graph()
        {
            Nodes = new List<T>();
            Links = new List<Link<T>>();
        }

        /// <summary>
        /// Checks if the Graph is connected
        /// http://en.wikipedia.org/wiki/Breadth-first_search#Finding_connected_components
        /// </summary>
        /// <param name="inGraph"></param>
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
        /// Executes Delaunay triangulation on the Nodes and populates the Links accordingly.
        /// Note that Links currently existing are not removed by the function.
        /// </summary>
        public void Triangulate()
        {
            // Triangulate and extract links
            List<Triangulator.Geometry.Point> nodesToTriangulate = new List<Triangulator.Geometry.Point>();
            foreach (var node in Nodes)
            {
                nodesToTriangulate.Add(new Triangulator.Geometry.Point(node.NodeX, node.NodeY));
            }

            List<Triangulator.Geometry.Triangle> triangles = Triangulator.Delauney.Triangulate(nodesToTriangulate);

            foreach (Triangulator.Geometry.Triangle triangle in triangles)
            {
                Triangulator.Geometry.Point pt1 = nodesToTriangulate[triangle.p1];
                Triangulator.Geometry.Point pt2 = nodesToTriangulate[triangle.p2];
                Triangulator.Geometry.Point pt3 = nodesToTriangulate[triangle.p3];

                T n1 = Nodes.Find(n => n.NodeX == pt1.X && n.NodeY == pt1.Y);
                T n2 = Nodes.Find(n => n.NodeX == pt2.X && n.NodeY == pt2.Y);
                T n3 = Nodes.Find(n => n.NodeX == pt3.X && n.NodeY == pt3.Y);

                Link<T> l1 = new Link<T>() { From = n1, To = n2, IsDirected = false, Weight = 1 };
                Link<T> l2 = new Link<T>() { From = n2, To = n3, IsDirected = false, Weight = 1 };
                Link<T> l3 = new Link<T>() { From = n3, To = n1, IsDirected = false, Weight = 1 };

                if (!Links.Contains(l1))
                {
                    Links.Add(l1);
                }

                if (!Links.Contains(l2))
                {
                    Links.Add(l2);
                }

                if (!Links.Contains(l3))
                {
                    Links.Add(l3);
                }
            }
        }
    }
}