// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality.Drawing;
using Duality.Editor;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.FX.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.FX.Components
{
    /// <summary>
    /// Implementation of a puntiform FXArea
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageFXPoint)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryFX)]
    public class FXPoint : FXArea
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        protected override void _DrawInEditor(Canvas inCanvas)
        {
            // nothing else to draw
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inRandom"></param>
        /// <returns></returns>
        protected override Vector3 _GetPoint(Random inRandom)
        {
            return Vector3.Zero;
        }
    }
}