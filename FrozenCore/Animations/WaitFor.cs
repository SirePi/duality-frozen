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
        private string _targetSignal;

        internal WaitFor(GameObject inGameObject, string inSignal)
        {
            try
            {
                _targetAnimator = GetComponent(inGameObject);
                _targetSignal = inSignal;
            }
            catch
            {
                // if I get an exception is because the target doesn't have an Animator component. 
                // That's ok, it just means I can't wait for it.
            }
        }

        internal override void Animate(float inSecondsPast, GameObject inGameObject)
        {
            if (_targetAnimator == null)
            {
                IsComplete = true;
            }
            else
            {
                if (_targetSignal != null)
                {
                    IsComplete = _targetAnimator.CurrentSignal == _targetSignal;
                }

                if (_targetAnimator.IsIdle)
                {
                    IsComplete = true;
                }
            }
        }
    }
}
