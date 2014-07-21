// This code is provided under the MIT license. Originally by Alessandro Pilati.

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
    /// <summary>
    /// Command used to alter a Widget's Status.
    /// </summary>
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


