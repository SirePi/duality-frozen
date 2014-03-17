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
    public sealed class ChangeWidgetStatus : ActiveAnimation<Widget>
    {
        Widget.WidgetStatus _target;

        internal ChangeWidgetStatus(GameObject inGameObject, Widget.WidgetStatus inStatus)
        { }

        internal override void Animate(float inSecondsPast, GameObject inGameObject)
        {
            Widget w = GetComponent(inGameObject);

            if (_timeToComplete <= 0)
            {
                w.Status = _target;
                IsComplete = true;
            }
            else
            {
                _timePast += inSecondsPast;

                if (_timePast >= _timeToComplete)
                {
                    w.Status = _target;
                    IsComplete = true;
                }
            }
        }

        protected override float GetAnimationLength()
        {
            return 1;
        }
    }
}


