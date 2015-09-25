// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality.Drawing;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.FX.Properties;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.FX.Components
{
    /// <summary>
    /// Implementation of a puntiform FXArea
    /// </summary>
    [EditorHintImage(ResNames.ImageFXPoint)]
    [EditorHintCategory(ResNames.CategoryFX)]
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
        /// <returns></returns>
        protected override Vector3 _GetPoint()
        {
            return Vector3.Zero;
        }
    }
}