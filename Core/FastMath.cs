// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;

namespace SnowyPeak.Duality.Plugin.Frozen.Core
{
    /// <summary>
    /// MathF extentions
    /// </summary>
    public static class FastMath
    {
        /// <summary>
        /// Fast Floor function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int FastFloor(float value)
        {
            return FastFloor((double)value);
        }

        /// <summary>
        /// Fast Floor function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int FastFloor(double value)
        {
            return value > 0 ? (int)value : (int)value - 1;
        }
    }
}