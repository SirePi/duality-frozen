// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.FX.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.FX.Components
{
    /// <summary>
    /// Implementation of a Rectangular FXArea
    /// </summary>
    [EditorHintImage(ResNames.ImageFXRect)]
    [EditorHintCategory(ResNames.CategoryFX)]
    public class FXRectangle : FXArea
    {
        /// <summary>
        /// [GET / SET] the size of the Area
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        protected override void _DrawInEditor(Canvas inCanvas)
        {
            inCanvas.State.TransformHandle = Size / 2;
            inCanvas.State.TransformAngle = this.GameObj.Transform.Angle;
            inCanvas.DrawRect(this.GameObj.Transform.Pos.X, this.GameObj.Transform.Pos.Y, this.GameObj.Transform.Pos.Z, Size.X, Size.Y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override Vector3 _GetPoint()
        {
            Vector2 result = Vector2.Zero;

            // Generating random point
            result.X = MathF.Rnd.NextFloat(-1, 1);
            result.Y = MathF.Rnd.NextFloat(-1, 1);

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

            return new Vector3(result * radius, 0);
        }
    }
}