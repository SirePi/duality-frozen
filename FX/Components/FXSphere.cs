// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.FX.Properties;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;

namespace SnowyPeak.Duality.Plugin.Frozen.FX.Components
{
    /// <summary>
    /// Implementation of a spheric FXArea
    /// </summary>
    [EditorHintImage(ResNames.ImageFXSphere)]
    [EditorHintCategory(ResNames.CategoryFX)]
    public class FXSphere : FXArea
    {
        /// <summary>
        /// [GET / SET] The radius of the Area
        /// </summary>
        public float Radius { get; set; }
        /// <summary>
        ///
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public override FloatRange ZRange
        {
            get { return new FloatRange(-Radius, Radius); }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        protected override void _DrawInEditor(Canvas inCanvas)
        {
            inCanvas.DrawCircle(this.GameObj.Transform.Pos.X, this.GameObj.Transform.Pos.Y, this.GameObj.Transform.Pos.Z, Radius);
            Core.Utilities.DrawCross(inCanvas, this.GameObj.Transform.Pos, Radius);
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
                result.Z = MathF.Rnd.NextFloat(-1, 1);
            } while (result.LengthSquared > 1);

            return result * Radius;
        }
    }
}