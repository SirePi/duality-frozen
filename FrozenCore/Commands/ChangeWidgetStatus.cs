using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality.Drawing;
using Duality;
using Duality.Components.Renderers;
using FrozenCore.Data;
using FrozenCore.Widgets;

namespace FrozenCore.Commands
{
    public sealed class ChangeWidgetStatus : Command<Widget>
    {
        Widget.WidgetStatus _target;

        internal ChangeWidgetStatus(Widget.WidgetStatus inStatus)
        {
            _target = inStatus;
        }

        public override void Execute(float inSecondsPast, GameObject inGameObject)
        {
            Widget w = GetComponent(inGameObject);

            w.Status = _target;
            IsComplete = true;
        }
    }
}


