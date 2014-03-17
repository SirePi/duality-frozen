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
    public sealed class ColorizeSkinnedWidget : ActiveAnimation<SkinnedWidget>
    {
        private ColorRange _range;

        internal ColorizeSkinnedWidget(GameObject inGameObject, ColorRgba inTargetColor)
        {
            SkinnedWidget sw = GetComponent(inGameObject);
            _range = new ColorRange(sw.Tint, inTargetColor);
        }

        internal override void Animate(float inSecondsPast, GameObject inGameObject)
        {
            SkinnedWidget sw = GetComponent(inGameObject);

            if (_timeToComplete <= 0)
            {
                Colorize(sw, _range.Max);
                IsComplete = true;
            }
            else
            {
                _timePast += inSecondsPast;

                if (_timePast >= _timeToComplete)
                {
                    Colorize(sw, _range.Max);
                    IsComplete = true;
                }
                else
                {
                    Colorize(sw, _range.Lerp(_timePast / _timeToComplete));
                }
            }
        }

        protected override float GetAnimationLength()
        {
            return (_range.Max.ToVector4() - _range.Min.ToVector4()).Length;
        }

        private void Colorize(SkinnedWidget inWidget, ColorRgba inColor)
        {
            inWidget.Tint = inColor;

            if (inWidget is SkinnedButton)
            {
                (inWidget as SkinnedButton).TextColor = inColor;
            }
        }
    }
}


