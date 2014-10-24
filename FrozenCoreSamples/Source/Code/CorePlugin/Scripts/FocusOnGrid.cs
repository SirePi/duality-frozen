using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrozenCore;
using FrozenCore.Widgets;
using Duality.Resources;
using Duality;

namespace FrozenCoreSamples.Scripts
{
    [Serializable]
    public class FocusOnGrid : Script
    {
        public override void Execute(Duality.GameObject inSource, object inParameter)
        {
            WidgetController controller = Scene.Current.FindComponent<WidgetController>();
            GameObject grid = Scene.Current.FindGameObject("SkinnedCommandGrid");

            if (grid != null && controller != null)
            {
                controller.FocusOn(grid.GetComponent<Widget>());
            }
        }
    }
}
