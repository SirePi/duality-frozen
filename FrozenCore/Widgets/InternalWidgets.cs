using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality.Resources;
using Duality;
using Duality.Components;

namespace FrozenCore.Widgets
{
    internal class CloseButton : Button
    {
        public CloseButton()
        {
            this.OnLeftClick = InternalScripts.GetScript<InternalScripts.CloseButtonLeftMouseDown>();
        }
    }

    internal class MinimizeButton: Button
    {
        public MinimizeButton()
        {
            this.OnLeftClick = InternalScripts.GetScript<InternalScripts.MinimizeButtonLeftMouseDown>();
        }
    }

    internal class MaximizeButton : Button
    {
        public MaximizeButton()
        {
            this.OnLeftClick = InternalScripts.GetScript<InternalScripts.MaximizeButtonLeftMouseDown>();
        }
    }

    internal class RestoreButton : Button
    {
        public RestoreButton()
        {
            this.OnLeftClick = InternalScripts.GetScript<InternalScripts.RestoreButtonLeftMouseDown>();
        }
    }

}
