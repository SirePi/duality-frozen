using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Duality.Components;
using Duality;
using FrozenCore.Data;

namespace FrozenCore.Animations
{
    public sealed class Move : ActiveAnimation<Transform>
    {
        private Vector3Range _range;

        internal Move(GameObject inGameObject, Vector3 inTargetPosition)
        {
            Transform t = GetComponent(inGameObject);
            _range = new Vector3Range(t.Pos, inTargetPosition);
        }

        public override void Animate(float inSecondsPast, GameObject inGameObject)
        {
            Transform t = GetComponent(inGameObject);

            if (_timeToComplete <= 0)
            {
                t.Pos = _range.Max;
                IsComplete = true;
            }
            else
            {
                _timePast += inSecondsPast;

                if (_timePast >= _timeToComplete)
                {
                    t.Pos = _range.Max;
                    IsComplete = true;
                }
                else
                {
                    t.Pos = _range.Lerp(_timePast / _timeToComplete);
                }
            }
        }

        protected override float GetAnimationLength()
        {
            return (_range.Max - _range.Min).Length;
        }
    }
}
