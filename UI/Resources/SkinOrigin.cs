// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using OpenTK;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public struct SkinOrigin
    {
        /// <summary>
        /// The TopLeft coordinates of the Active state of the Skin
        /// </summary>
        public Vector2 Active { get; set; }
        /// <summary>
        /// The TopLeft coordinates of the Disabled state of the Skin
        /// </summary>
        public Vector2 Disabled { get; set; }
        /// <summary>
        /// The TopLeft coordinates of the Hover state of the Skin
        /// </summary>
        public Vector2 Hover { get; set; }
        /// <summary>
        /// The TopLeft coordinates of the Normal state of the Skin
        /// </summary>
        public Vector2 Normal { get; set; }
    }
}