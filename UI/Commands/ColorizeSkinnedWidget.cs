// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Drawing;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.Core.Commands;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;
using SnowyPeak.Duality.Plugin.Frozen.UI.Widgets;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Commands
{
    /// <summary>
    /// TimedCommand used to alter over time the colorization of a Widget
    /// </summary>
    public sealed class ColorizeSkinnedWidget : TimedCommand<SkinnedWidget>
    {
        private ColorRange _range;

        internal ColorizeSkinnedWidget(GameObject inGameObject, ColorRgba inTargetColor)
        {
            SkinnedWidget sw = GetComponent(inGameObject);
            _range = new ColorRange(sw.Tint, inTargetColor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        /// <param name="inGameObject"></param>
        public override void Execute(float inSecondsPast, GameObject inGameObject)
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

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override float GetCommandLength()
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