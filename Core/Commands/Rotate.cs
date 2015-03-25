// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Commands
{
    /// <summary>
    /// TimedCommand used to rotate a GameObject around its Z axis.
    /// </summary>
    public sealed class Rotate : TimedCommand<Transform>
    {
        private bool _isRelative;
        private FloatRange _range;
        private float _target;

        internal Rotate(float inTargetAngle, bool inIsRelative)
        {
            _target = inTargetAngle;
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
                SetAngle(t, _range.Max);
                IsComplete = true;
            }
            else
            {
                SetAngle(t, _range.Lerp(_timePast / _timeToComplete));
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="inGameObject"></param>
        public override void Initialize(GameObject inGameObject)
        {
            Transform t = GetComponent(inGameObject);
            _range = new FloatRange(_isRelative ? t.RelativeAngle : t.Angle, _target);
        }

        /// <summary>
        /// Returns the length, in abstract, of the command before it reaches its completion. Used for FixedSpeed commands
        /// </summary>
        /// <returns></returns>
        protected override float GetCommandLength()
        {
            return (_range.Max - _range.Min);
        }

        private void SetAngle(Transform inTransform, float inAngle)
        {
            if (_isRelative)
            {
                inTransform.TurnTo(inAngle);
            }
            else
            {
                inTransform.TurnToAbs(inAngle);
            }
        }
    }
}