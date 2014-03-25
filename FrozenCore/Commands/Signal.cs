using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using FrozenCore.Data;

namespace FrozenCore.Commands
{
    public sealed class Signal : Command
    {
        public string Value { get; private set; }

        internal Signal(string inSignal)
        {
            Value = inSignal;
        }

        public override void Execute(float inSecondsPast, GameObject inGameObject)
        {
            IsComplete = true;
        }
    }
}
