// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using OpenTK;

namespace FrozenCore.FX
{
    [Serializable]
    public class FXPoint : FXArea
    {
        public FXPoint()
        {
        }

        protected override void _DrawInEditor(Duality.Canvas inCanvas, Vector3 inPosition)
        {
            // nothing else to draw
        }

        protected override Vector3 _GetPoint(FastRandom inRandom)
        {
            return Vector3.Zero;
        }
    }
}