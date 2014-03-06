using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace FrozenCore.Resources.DefaultScripts
{
    public class Quit : Script
    {
        public override void Execute(Duality.GameObject inSource, object inParameter)
        {
            DualityApp.Terminate();
        }
    }
}
