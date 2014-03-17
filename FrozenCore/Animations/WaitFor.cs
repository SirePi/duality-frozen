using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using FrozenCore.Data;
using FrozenCore.Components;

namespace FrozenCore.Animations
{
    public sealed class WaitFor : Animation<Animator>
    {
        private Animator _targetAnimator;
        private float _timeToWait;
        private float _elapsedTime;

        internal WaitFor(GameObject inTargetGameObject)
        {
            try
            {
                _targetAnimator = GetComponent(inTargetGameObject);
            }
            catch
            {
                // if I get an exception is because the target doesn't have an Animator component. 
                // That's ok, it just means I can't wait for it.
            }
            _timeToWait = float.PositiveInfinity;
        }

        internal override void Animate(float inSecondsPast, GameObject inGameObject)
        {
            if (_timeToWait <= 0 || _targetAnimator == null)
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
