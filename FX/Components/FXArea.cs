// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;
using SnowyPeak.Duality.Plugin.Frozen.Core;


namespace SnowyPeak.Duality.Plugin.Frozen.FX.Components
{
    /// <summary>
    /// Abstract Component used by FX classes to determine the area where the effect can appear
    /// </summary>
    [RequiredComponent(typeof(Transform))]
    public abstract class FXArea : Component
    {
        /// <summary>
        ///
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public virtual FloatRange ZRange
        {
            get { return FloatRange.ZERO; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        /// <param name="inColor"></param>
        internal void DrawInEditor(Canvas inCanvas, ColorRgba inColor)
        {
            inCanvas.PushState();

            inCanvas.State.ColorTint = inColor;
            Utilities.DrawCross(inCanvas, GameObj.Transform.Pos, 10);
            _DrawInEditor(inCanvas);

            inCanvas.PopState();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        internal Vector3 GetPoint()
        {
            return _GetPoint();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        protected abstract void _DrawInEditor(Canvas inCanvas);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract Vector3 _GetPoint();
    }
}