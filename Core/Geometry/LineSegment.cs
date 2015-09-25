using Duality;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Geometry
{
    /// <summary>
    ///
    /// </summary>
    public sealed class LineSegment
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        public LineSegment(Nullable<Vector2> p0, Nullable<Vector2> p1)
        {
            this.P0 = p0;
            this.P1 = p1;
        }

        /// <summary>
        ///
        /// </summary>
        public float Length
        {
            get { return (P0.Value - P1.Value).Length; }
        }

        /// <summary>
        ///
        /// </summary>
        public Nullable<Vector2> P0 { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public Nullable<Vector2> P1 { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edge0"></param>
        /// <param name="edge1"></param>
        /// <returns></returns>
        public static int CompareLengths(LineSegment edge0, LineSegment edge1)
        {
            return -CompareLengths_MAX(edge0, edge1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="segment0"></param>
        /// <param name="segment1"></param>
        /// <returns></returns>
        public static int CompareLengths_MAX(LineSegment segment0, LineSegment segment1)
        {
            return segment1.Length.CompareTo(segment0.Length);
        }
    }
}