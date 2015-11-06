// This code is provided under the MIT license. Originally by Alessandro Pilati.

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

		/// <summary>
		/// Multiplies the Rect by the amount specified in a Vector2. 
		/// Useful to pass from UV coordinates to Pixmap coordinates
		/// </summary>
		/// <param name="inRect"></param>
		/// <param name="inVector"></param>
		/// <returns></returns>
		public static Rect Multiply(this Rect inRect, Vector2 inVector)
		{
			Vector2 topLeft = inRect.TopLeft * inVector;
			Vector2 size = inRect.Size * inVector;

			return new Rect(topLeft.X, topLeft.Y, size.X, size.Y);
		}

		/// <summary>
		/// Divides the Rect by the amount specified in a Vector2.
		/// Useful to pass from Pixmap coordinates to UV coordinates 
		/// </summary>
		/// <param name="inRect"></param>
		/// <param name="inVector"></param>
		/// <returns></returns>
		public static Rect Divide(this Rect inRect, Vector2 inVector)
		{
			Vector2 topLeft = inRect.TopLeft / inVector;
			Vector2 size = inRect.Size / inVector;

			return new Rect(topLeft.X, topLeft.Y, size.X, size.Y);
		}
	}

    /// <summary>
    /// Vector4 Extensions
    /// </summary>
    public static class Vector4Extensions
    {
        /// <summary>
        /// Returns the XY components as a Vector2
        /// </summary>
        /// <param name="inVector"></param>
        /// <returns></returns>
        public static Vector2 XY(this Vector4 inVector)
        {
            return new Vector2(inVector.X, inVector.Y);
        }

        /// <summary>
        /// Returns the ZW components as a Vector2
        /// </summary>
        /// <param name="inVector"></param>
        /// <returns></returns>
        public static Vector2 ZW(this Vector4 inVector)
        {
            return new Vector2(inVector.Z, inVector.W);
        }
    }
}