// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Commands
{
    /// <summary>
    /// TimedCommand used to Move a GameObject in 3D
    /// </summary>
    public sealed class Move : TimedCommand<Transform>
    {
        private bool _isRelative;
        private Vector3Range _range;

        internal Move(GameObject inGameObject, Vector3 inTargetPosition, bool inIsRelative)
        {
            Transform t = GetComponent(inGameObject);
            _range = new Vector3Range(inIsRelative ? t.RelativePos : t.Pos, inTargetPosition);
            _isRelative = inIsRelative;
        }

        /// <summary>
        /// Advances the command's execution
        /// </summary>
        /// <param name="inSecondsPast"></param>
        /// <param name="inGameObject"></param>
        public override void Execute(float inSecondsPast, GameObject inGameObject)
        {
            Transform t = GetComponent(inGameObject);
            _timePast += inSecondsPast;

            if (_timeToComplete <= 0 || _timePast >= _timeToComplete)
            {
                SetPosition(t, _range.Max);
                IsComplete = true;
            }
            else
            {
                SetPosition(t, _range.Lerp(_timePast / _timeToComplete));
            }
        }

        /// <summary>
        /// Returns the length, in abstract, of the command before it reaches its completion. Used for FixedSpeed commands
        /// </summary>
        /// <returns></returns>
        protected override float GetCommandLength()
        {
            return (_range.Max - _range.Min).Length;
        }

        private void SetPosition(Transform inTransform, Vector3 inPosition)
        {
            if (_isRelative)
            {
                inTransform.MoveTo(inPosition);
            }
            else
            {
                inTransform.MoveToAbs(inPosition);
            }
        }
    }
}