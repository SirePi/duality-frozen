// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.FX.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.FX.Components
{
    /// <summary>
    /// Implementation of a circular FXArea
    /// </summary>
    [EditorHintImage(ResNames.ImageFXCircle)]
    [EditorHintCategory(ResNames.CategoryFX)]
    public class FXCircle : FXArea
    {
        /// <summary>
        /// [GET / SET] The radius of the Area
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        protected override void _DrawInEditor(Canvas inCanvas)
        {
            inCanvas.DrawCircle(this.GameObj.Transform.Pos.X, this.GameObj.Transform.Pos.Y, this.GameObj.Transform.Pos.Z, Radius);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override Vector3 _GetPoint()
        {
            Vector3 result = Vector3.Zero;

            do
            {
                result.X = MathF.Rnd.NextFloat(-1, 1);
                result.Y = MathF.Rnd.NextFloat(-1, 1);
                result.Z = 0;
            } while (result.LengthSquared > 1);

            return result * Radius;
        }
    }
}