// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality.Drawing;
using Duality;
using Duality.Components.Renderers;
using FrozenCore.Data;

namespace FrozenCore.Commands
{
    /// <summary>
    /// TimedCommand used to alter over time the colorization of a SpriteRenderer
    /// </summary>
    public sealed class ColorizeSprite : TimedCommand<SpriteRenderer>
    {
        private ColorRange _range;

        internal ColorizeSprite(GameObject inGameObject, ColorRgba inTargetColor)
        {
            SpriteRenderer sr = GetComponent(inGameObject);
            _range = new ColorRange(sr.ColorTint, inTargetColor);
        }

        public override void Execute(float inSecondsPast, GameObject inGameObject)
        {
            SpriteRenderer sr = GetComponent(inGameObject);

            if (_timeToComplete <= 0)
            {
                sr.ColorTint = _range.Max;
                IsComplete = true;
            }
            else
            {
                _timePast += inSecondsPast;

                if (_timePast >= _timeToComplete)
                {
                    sr.ColorTint = _range.Max;
                    IsComplete = true;
                }
                else
                {
                    sr.ColorTint = _range.Lerp(_timePast / _timeToComplete);
                }
            }
        }

        protected override float GetCommandLength()
        {
            return (_range.Max.ToVector4() - _range.Min.ToVector4()).Length;
        }
    }
}


