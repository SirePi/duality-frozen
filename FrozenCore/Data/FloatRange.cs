// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using System;

namespace FrozenCore.Data
{
    /// <summary>
    /// Implementation of Range for float values
    /// </summary>
    public class FloatRange : Range<float>
    {
        public static readonly FloatRange DIRECTION_2PI = new FloatRange(0, (float)MathF.TwoPi);
        public static readonly FloatRange ONE = new FloatRange(1, 1);
        public static readonly FloatRange ZERO = new FloatRange(0, 0);
        public static readonly FloatRange ZERO_TO_ONE = new FloatRange(0, 1);

        public FloatRange(float inMin, float inMax)
            : base(inMin, inMax)
        {
            _delta = Max - Min;
        }

        public override float GetRandom(Random inRandom)
        {
            return (float)(Min + (inRandom.NextDouble() * _delta));
        }

        protected override float _Lerp(float inValue)
        {
            return (float)(Min + (_delta * inValue));
        }
    }
}