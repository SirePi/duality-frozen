using Duality;
using Duality.Drawing;

namespace SnowyPeak.Duality.Plugin.Frozen.Core
{
    /// <summary>
    /// ColorRgba Extensions
    /// </summary>
    public static class ColorRgbaExtensions
    {
        /// <summary>
        /// Converts a ColorRgba to float array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float[] ToFloatArray(this ColorRgba value)
        {
            return new float[] { value.R / 255f, value.G / 255f, value.B / 255f, value.A / 255f };
        }

        /// <summary>
        /// Converts a ColorRgba to Vector4 (R, G, B, A) = (X, Y, Z, W)
        /// </summary>
        /// <param name="value">The Color</param>
        /// <returns></returns>
        public static Vector4 ToVector4(this ColorRgba value)
        {
            return new Vector4(value.R, value.G, value.B, value.A);
        }
    }
}