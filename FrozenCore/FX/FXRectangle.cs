// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using OpenTK;

namespace FrozenCore.FX
{
    [Serializable]
    public class FXRectangle : FXArea
    {
        public Vector2 Size { get; set; }

        protected override void _DrawInEditor(Canvas inCanvas, Vector3 inPosition)
        {
            inCanvas.DrawRect(inPosition.X - Size.X / 2, inPosition.Y - Size.Y / 2, inPosition.Z, Size.X, Size.Y);
        }

        protected override Vector3 _GetPoint(Random inRandom)
        {
            float x = ((float)inRandom.NextDouble() * Size.X) - (Size.X / 2);
            float y = ((float)inRandom.NextDouble() * Size.Y) - (Size.Y / 2);

            return new Vector3(x, y, 0);
        }
    }
}