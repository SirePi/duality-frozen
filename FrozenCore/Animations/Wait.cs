using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using FrozenCore.Data;

namespace FrozenCore.Animations
{
    public sealed class Wait : Animation
    {
        private float _timeToWait;
        private float _elapsedTime;

        internal Wait(float inTimeToWait)
        {
            _timeToWait = inTimeToWait;
        }

        internal override void Animate(float inSecondsPast, GameObject inGameObject)
        {
            if (_timeToWait <= 0)
            {
                IsComplete = true;
            }
            else
            {
                _elapsedTime += inSecondsPast;

                if (_elapsedTime >= _timeToWait)
                {
                    IsComplete = true;
                }
            }
        }
    }
}
