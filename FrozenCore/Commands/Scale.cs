// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Duality.Components;
using Duality;
using FrozenCore.Data;

namespace FrozenCore.Commands
{
    public sealed class Scale : TimedCommand<Transform>
    {
        private FloatRange _range;

        internal Scale(GameObject inGameObject, float inTargetScale, bool inIsRelative)
        {
            Transform t = GetComponent(inGameObject);

            _range = new FloatRange(t.Scale, inIsRelative ? t.Scale * inTargetScale : inTargetScale);
        }

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
                t.Scale =_range.Lerp(_timePast / _timeToComplete);
            }
        }

        protected override float GetCommandLength()
        {
            return (_range.Max - _range.Min);
        }
    }
}
