using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrozenCore.Widgets;
using Duality.Resources;

namespace FrozenCore.Resources.DefaultScripts
{
    [Serializable]
    public class Quit : Script
    {
        public override void Execute(Duality.GameObject inSource, object inParameter)
        {
            Duality.DualityApp.Terminate();
        }
    }
}
