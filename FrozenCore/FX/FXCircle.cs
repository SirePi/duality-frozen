// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using OpenTK;

namespace FrozenCore.FX
{
    [Serializable]
    public class FXCircle : FXArea
    {
        public float Radius { get; set; }

        protected override void _DrawInEditor(Canvas inCanvas, Vector3 inPosition)
        {
            inCanvas.DrawCircle(inPosition.X, inPosition.Y, inPosition.Z, Radius);
        }

        protected override Vector3 _GetPoint(FastRandom inRandom)
        {
            float angle = (float)inRandom.NextDouble() * MathF.TwoPi;
            float distance = (float)inRandom.NextDouble() * Radius;

            return new Vector3(
                (MathF.Cos(angle) * distance),
                (MathF.Sin(angle) * distance),
                0);
        }
    }
}