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
    /// TimedCommand used to alter a GameObject's scale
    /// </summary>
    public sealed class Scale : TimedCommand<Transform>
    {
        private bool _isRelative;
        private FloatRange _range;
        private float _target;

        internal Scale(float inTargetScale, bool inIsRelative)
        {
            _target = inTargetScale;
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
                t.Scale = _range.Max;
                IsComplete = true;
            }
            else
            {
                t.Scale = _range.Lerp(_timePast / _timeToComplete);
            }
        }

        public override void Initialize(GameObject inGameObject)
        {
            Transform t = GetComponent(inGameObject);
            _range = new FloatRange(t.Scale, _isRelative ? t.Scale * _target : _target);
        }

        /// <summary>
        /// Returns the length, in abstract, of the command before it reaches its completion. Used for FixedSpeed commands
        /// </summary>
        /// <returns></returns>
        protected override float GetCommandLength()
        {
            return (_range.Max - _range.Min);
        }
    }
}