using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality.Drawing;
using Duality;
using Duality.Components.Renderers;
using FrozenCore.Data;
using FrozenCore.Widgets;

namespace FrozenCore.Animations
{
    public sealed class ChangeWidgetStatus : Animation<Widget>
    {
        Widget.WidgetStatus _target;

        internal ChangeWidgetStatus(Widget.WidgetStatus inStatus)
        {
            _target = inStatus;
        }

        public override void Animate(float inSecondsPast, GameObject inGameObject)
        {
            Widget w = GetComponent(inGameObject);

            w.Status = _target;
            IsComplete = true;
        }
    }
}


