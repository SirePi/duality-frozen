// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using SnowyPeak.Duality.Plugin.Frozen.Core.Components;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Commands
{
    /// <summary>
    /// A utility Command that waits for a signal coming from another GameObject. Used for command synchronization
    /// via Signal
    /// </summary>
    public sealed class WaitFor : Command<Commander>
    {
        private Commander _targetAnimator;
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

        /// <summary>
        /// Advances the command's execution
        /// </summary>
        /// <param name="inSecondsPast"></param>
        /// <param name="inGameObject"></param>
        public override void Execute(float inSecondsPast, GameObject inGameObject)
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