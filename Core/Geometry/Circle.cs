using System;
using OpenTK;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Geometry
{
    /// <summary>
    ///
    /// </summary>
    public sealed class Circle
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        ///
        /// </summary>
        public Vector2 Center { get; private set; }
        /// <summary>
        ///
        /// </summary>
        public float Radius { get; private set; }
    }
}