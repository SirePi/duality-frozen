// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality.Drawing;
using OpenTK;

namespace SnowyPeak.Duality.Plugin.Frozen.Core
{
    /// <summary>
    /// Represents a Point in Scene, World and UV coordinates
    /// </summary>
    public struct MultiSpacePoint
    {
        /// <summary>
        ///
        /// </summary>
        public Vector3 SceneCoords;

        /// <summary>
        ///
        /// </summary>
        public Vector2 UVCoords;

        /// <summary>
        ///
        /// </summary>
        public Vector3 WorldCoords;

        /// <summary>
        /// 
        /// </summary>
        public ColorRgba Tint;
    }
}