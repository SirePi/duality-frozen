using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality.Drawing;
using Duality;
using Duality.Components.Renderers;
using FrozenCore.Data;

namespace FrozenCore.Animations
{
    public sealed class ColorizeSprite : ActiveAnimation<SpriteRenderer>
    {
        private ColorRange _range;

        internal ColorizeSprite(GameObject inGameObject, ColorRgba inTargetColor)
        {
            SpriteRenderer sr = GetComponent(inGameObject);
            _range = new ColorRange(sr.ColorTint, inTargetColor);
        }

        public override void Animate(float inSecondsPast, GameObject inGameObject)
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

        protected override float GetAnimationLength()
        {
            return (_range.Max.ToVector4() - _range.Min.ToVector4()).Length;
        }
    }
}


