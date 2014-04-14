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
    public sealed class Move : TimedCommand<Transform>
    {
        private Vector3Range _range;
        private bool _isRelative;

        internal Move(GameObject inGameObject, Vector3 inTargetPosition, bool inIsRelative)
        {
            Transform t = GetComponent(inGameObject);
            _range = new Vector3Range(inIsRelative ? t.RelativePos : t.Pos, inTargetPosition);
            _isRelative = inIsRelative;
        }

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

        private void SetPosition(Transform inTransform, Vector3 inPosition)
        {
            if (_isRelative)
            {
                inTransform.RelativePos = inPosition;
            }
            else
            {
                inTransform.Pos = inPosition;
            }
        }

        protected override float GetCommandLength()
        {
            return (_range.Max - _range.Min).Length;
        }
    }
}
