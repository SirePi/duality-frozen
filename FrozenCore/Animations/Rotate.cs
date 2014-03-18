using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using FrozenCore.Data;

namespace FrozenCore.Animations
{
    public sealed class Rotate : ActiveAnimation<Transform>
    {
        private FloatRange _range;

        internal Rotate(GameObject inGameObject, float inTargetAngle)
        {
            Transform t = GetComponent(inGameObject);
            _range = new FloatRange(t.Angle, inTargetAngle);
        }

        public override void Animate(float inSecondsPast, GameObject inGameObject)
        {
            Transform t = GetComponent(inGameObject);

            if (_timeToComplete <= 0)
            {
                t.Angle = _range.Max;
                IsComplete = true;
            }
            else
            {
                _timePast += inSecondsPast;

                if (_timePast >= _timeToComplete)
                {
                    t.Angle = _range.Max;
                    IsComplete = true;
                }
                else
                {
                    t.Angle = _range.Lerp(_timePast / _timeToComplete);
                }
            }
        }

        protected override float GetAnimationLength()
        {
            return (_range.Max - _range.Min);
        }
    }
}
