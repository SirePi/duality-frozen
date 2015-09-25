// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural
{
    /// <summary>
    /// Interface that should be implemented by a class in order to be used as Node in a Graph
    /// </summary>
    public interface INode
    {
        /// <summary>
        ///
        /// </summary>
        Vector2 Position { get; }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class DefaultNode : INode
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="v"></param>
        public DefaultNode(Vector2 v)
        {
            Position = v;
        }

        /// <summary>
        ///
        /// </summary>
        public Vector2 Position { get; set; }
    }
}