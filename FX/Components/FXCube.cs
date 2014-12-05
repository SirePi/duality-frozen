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
    /// Implementation of a "cubic" FXArea
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageFXCube)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryFX)]
    public class FXCube : FXArea
    {
        /// <summary>
        /// [GET / SET] the size of the Area
        /// </summary>
        public Vector3 Size { get; set; }
        /// <summary>
        ///
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public override Data.FloatRange ZRange
        {
            get
            {
                float halfSize = Size.Z / 2;
                return new Data.FloatRange(-halfSize, halfSize);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        protected override void _DrawInEditor(Canvas inCanvas)
        {
            inCanvas.State.TransformHandle = Size.Xy / 2;
            inCanvas.State.TransformAngle = this.GameObj.Transform.Angle;
            inCanvas.DrawRect(this.GameObj.Transform.Pos.X, this.GameObj.Transform.Pos.Y, this.GameObj.Transform.Pos.Z - (Size.Z / 2), Size.X, Size.Y);
            inCanvas.State.ColorTint = new ColorRgba(inCanvas.State.ColorTint) { A = (byte)(inCanvas.State.ColorTint.A / 1.5) };
            inCanvas.DrawRect(this.GameObj.Transform.Pos.X, this.GameObj.Transform.Pos.Y, this.GameObj.Transform.Pos.Z, Size.X, Size.Y);
            inCanvas.State.ColorTint = new ColorRgba(inCanvas.State.ColorTint) { A = (byte)(inCanvas.State.ColorTint.A / 1.5) };
            inCanvas.DrawRect(this.GameObj.Transform.Pos.X, this.GameObj.Transform.Pos.Y, this.GameObj.Transform.Pos.Z + (Size.Z / 2), Size.X, Size.Y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inRandom"></param>
        /// <returns></returns>
        protected override Vector3 _GetPoint(Random inRandom)
        {
            Vector3 result = Vector3.Zero;

            // Generating random point
            result.X = inRandom.NextFloat(-1, 1);
            result.Y = inRandom.NextFloat(-1, 1);
            result.Z = inRandom.NextFloat(-1, 1);

            // Rescaling
            result = result * Size / 2;

            // normalizing
            float radius = MathF.Max(Size.X, Size.Y) / 2;
            float dx = result.X / radius;
            float dy = result.Y / radius;

            // rotating
            float sin = MathF.Sin(this.GameObj.Transform.Angle);
            float cos = MathF.Cos(this.GameObj.Transform.Angle);

            result.X = dx * cos - dy * sin;
            result.Y = dx * sin + dy * cos;

            return result;
        }
    }
}