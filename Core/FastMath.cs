// This code is provided under the MIT license. Originally by Alessandro Pilati.

namespace SnowyPeak.Duality.Plugin.Frozen.Core
{
    /// <summary>
    /// MathF extensions
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