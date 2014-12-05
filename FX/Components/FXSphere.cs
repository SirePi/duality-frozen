// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using Duality.Editor;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.FX.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.FX.Components
{
    /// <summary>
    /// Implementation of a spheric FXArea
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageFXSphere)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryFX)]
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
        public override Data.FloatRange ZRange
        {
            get { return new Data.FloatRange(-Radius, Radius); }
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
        /// <param name="inRandom"></param>
        /// <returns></returns>
        protected override Vector3 _GetPoint(Random inRandom)
        {
            Vector3 result = Vector3.Zero;

            do
            {
                result.X = inRandom.NextFloat(-1, 1);
                result.Y = inRandom.NextFloat(-1, 1);
                result.Z = inRandom.NextFloat(-1, 1);
            } while (result.LengthSquared > 1);

            return result * Radius;
        }
    }
}