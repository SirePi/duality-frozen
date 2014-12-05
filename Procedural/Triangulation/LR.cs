namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Triangulation
{
    /// <summary>
    ///
    /// </summary>
    public enum Side
    {
#pragma warning disable 1591
        Left,
        Right
#pragma warning restore 1591
    }

    /// <summary>
    ///
    /// </summary>
    public class SideHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Side Other(Side value)
        {
            return value == Side.Left ? Side.Right : Side.Left;
        }
    }
}