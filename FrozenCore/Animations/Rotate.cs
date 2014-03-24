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
        private bool _isRelative;

        internal Rotate(GameObject inGameObject, float inTargetAngle, bool inIsRelative)
        {
            Transform t = GetComponent(inGameObject);
            _range = new FloatRange(inIsRelative ? t.RelativeAngle : t.Angle, inTargetAngle);
            _isRelative = inIsRelative;
        }

        public override void Animate(float inSecondsPast, GameObject inGameObject)
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

        private void SetAngle(Transform inTransform, float inAngle)
        {
            if (_isRelative)
            {
                inTransform.RelativeAngle = inAngle;
            }
            else
            {
                inTransform.Angle = inAngle;
            }
        }

        protected override float GetAnimationLength()
        {
            return (_range.Max - _range.Min);
        }
    }
}
