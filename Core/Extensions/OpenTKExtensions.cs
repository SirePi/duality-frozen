// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Core
{
    /// <summary>
    /// OpenTK Extensions
    /// </summary>
    public static class RectExtensions
    {
        /// <summary>
        /// Expands the Rect, on each side, by an amount
        /// </summary>
        /// <param name="inRect">The caller</param>
        /// <param name="inExpansion">The amount to expand each side</param>
        /// <returns></returns>
        public static Rect Expand(this Rect inRect, float inExpansion)
        {
            return new Rect(inRect.X - inExpansion, inRect.Y - inExpansion, inRect.W + (inExpansion * 2), inRect.H + (inExpansion * 2));
        }
    }
}