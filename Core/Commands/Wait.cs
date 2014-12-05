// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Commands
{
    /// <summary>
    /// A Command used to stop the execution of the following Command until enough time has elapsed.
    /// </summary>
    public sealed class Wait : Command
    {
        private float _elapsedTime;
        private float _timeToWait;

        internal Wait(float inTimeToWait)
        {
            _timeToWait = inTimeToWait;
        }

        /// <summary>
        /// Advances the command's execution
        /// </summary>
        /// <param name="inSecondsPast"></param>
        /// <param name="inGameObject"></param>
        public override void Execute(float inSecondsPast, GameObject inGameObject)
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