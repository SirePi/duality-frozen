// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using FrozenCore.Data;

namespace FrozenCore.Commands
{
    /// <summary>
    /// A Command used to stop the execution of the following Command until enough time has elapsed.
    /// </summary>
    public sealed class Wait : Command
    {
        private float _timeToWait;
        private float _elapsedTime;

        internal Wait(float inTimeToWait)
        {
            _timeToWait = inTimeToWait;
        }

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
