using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using FrozenCore.Data;

namespace FrozenCore.Animations
{
    public sealed class Signal : Animation
    {
        public string Value { get; private set; }

        internal Signal(string inSignal)
        {
            Value = inSignal;
        }

        public override void Animate(float inSecondsPast, GameObject inGameObject)
        {
            IsComplete = true;
        }
    }
}
