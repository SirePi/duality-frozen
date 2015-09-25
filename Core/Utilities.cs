// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Drawing;

namespace SnowyPeak.Duality.Plugin.Frozen.Core
{
    /// <summary>
    /// A collection of general-purpose methods, mainly used inside SnowyPeak.Duality.Plugin.Frozen.Core.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        ///
        /// </summary>
        public static bool IsDualityEditor
        {
            get { return (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Editor && DualityApp.ExecContext == DualityApp.ExecutionContext.Editor); }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        /// <param name="inPosition"></param>
        /// <param name="inRadius"></param>
        public static void DrawCross(Canvas inCanvas, Vector3 inPosition, float inRadius)
        {
            inCanvas.DrawLine(inPosition.X - inRadius, inPosition.Y, inPosition.X + inRadius, inPosition.Y);
            inCanvas.DrawLine(inPosition.X, inPosition.Y - inRadius, inPosition.X, inPosition.Y + inRadius);
        }
    }
}