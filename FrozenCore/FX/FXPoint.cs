// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality.Drawing;
using OpenTK;

namespace FrozenCore.FX
{
    /// <summary>
    /// Implementation of a puntiform FXArea
    /// </summary>
    [Serializable]
    public class FXPoint : FXArea
    {
        protected override void _DrawInEditor(Canvas inCanvas, Vector3 inPosition)
        {
            // nothing else to draw
        }

        protected override Vector3 _GetPoint(Random inRandom)
        {
            return Vector3.Zero;
        }
    }
}