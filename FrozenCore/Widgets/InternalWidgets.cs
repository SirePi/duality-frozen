using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality.Resources;
using Duality;
using Duality.Components;

namespace FrozenCore.Widgets
{
    internal class CloseButton : SkinnedButton
    {
        public CloseButton()
        {
            this.OnLeftClick = InternalScripts.GetScript<InternalScripts.CloseButtonLeftMouseDown>();
        }
    }

    internal class MinimizeButton: SkinnedButton
    {
        public MinimizeButton()
        {
            this.OnLeftClick = InternalScripts.GetScript<InternalScripts.MinimizeButtonLeftMouseDown>();
        }
    }

    internal class MaximizeButton : SkinnedButton
    {
        public MaximizeButton()
        {
            this.OnLeftClick = InternalScripts.GetScript<InternalScripts.MaximizeButtonLeftMouseDown>();
        }
    }

    internal class RestoreButton : SkinnedButton
    {
        public RestoreButton()
        {
            this.OnLeftClick = InternalScripts.GetScript<InternalScripts.RestoreButtonLeftMouseDown>();
        }
    }

}
