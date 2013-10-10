// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using OpenTK;

namespace FrozenCore.FX
{
    /// <summary>
    /// Component used by
    /// </summary>
    [Serializable]
    [RequiredComponent(typeof(Transform))]
    public abstract class FXArea : Component
    {
        public void DrawInEditor(Canvas inCanvas)
        {
            inCanvas.PushState();

            inCanvas.CurrentState.ColorTint = Colors.Aqua;
            inCanvas.DrawCross(GameObj.Transform.Pos.X, GameObj.Transform.Pos.Y, GameObj.Transform.Pos.Z, 10);
            _DrawInEditor(inCanvas, GameObj.Transform.Pos);

            inCanvas.PopState();
        }

        public Vector3 GetPoint(FastRandom inRandom)
        {
            return GameObj.Transform.Pos + _GetPoint(inRandom);
        }

        protected abstract void _DrawInEditor(Canvas inCanvas, Vector3 inPosition);

        protected abstract Vector3 _GetPoint(FastRandom inRandom);
    }
}