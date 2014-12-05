// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using SnowyPeak.Duality.Plugin.Frozen.Core.Commands;
using SnowyPeak.Duality.Plugin.Frozen.UI.Widgets;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Commands
{
    /// <summary>
    /// Command used to alter a Widget's Status.
    /// </summary>
    public sealed class ChangeWidgetStatus : Command<Widget>
    {
        private Widget.WidgetStatus _target;

        internal ChangeWidgetStatus(Widget.WidgetStatus inStatus)
        {
            _target = inStatus;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        /// <param name="inGameObject"></param>
        public override void Execute(float inSecondsPast, GameObject inGameObject)
        {
            Widget w = GetComponent(inGameObject);

            w.Status = _target;
            IsComplete = true;
        }
    }
}