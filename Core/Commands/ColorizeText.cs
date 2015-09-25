// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components.Renderers;
using Duality.Drawing;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Commands
{
    /// <summary>
    /// TimedCommand used to alter over time the colorization of a SpriteRenderer
    /// </summary>
    public sealed class ColorizeText : TimedCommand<TextRenderer>
    {
        private ColorRange _range;
        private ColorRgba _target;

        internal ColorizeText(ColorRgba inTargetColor)
        {
            _target = inTargetColor;
        }

        /// <summary>
        /// Advances the command's execution
        /// </summary>
        /// <param name="inSecondsPast"></param>
        /// <param name="inGameObject"></param>
        public override void Execute(float inSecondsPast, GameObject inGameObject)
        {
            TextRenderer tr = GetComponent(inGameObject);

            if (_timeToComplete <= 0)
            {
                tr.ColorTint = _range.Max;
                IsComplete = true;
            }
            else
            {
                _timePast += inSecondsPast;

                if (_timePast >= _timeToComplete)
                {
                    tr.ColorTint = _range.Max;
                    IsComplete = true;
                }
                else
                {
                    tr.ColorTint = _range.Lerp(_timePast / _timeToComplete);
                }
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="inGameObject"></param>
        public override void Initialize(GameObject inGameObject)
        {
            TextRenderer tr = GetComponent(inGameObject);
            _range = new ColorRange(tr.ColorTint, _target);
        }

        /// <summary>
        /// Returns the length, in abstract, of the command before it reaches its completion. Used for FixedSpeed commands
        /// </summary>
        /// <returns></returns>
        protected override float GetCommandLength()
        {
            return (_range.Max.ToVector4() - _range.Min.ToVector4()).Length;
        }
    }
}