// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
namespace FrozenCore.Data
{
    /// <summary>
    /// Implementation of Range for int values
    /// </summary>
    public class IntegerRange : Range<int>
    {
        public static readonly IntegerRange ONE = new IntegerRange(1, 1);
        public static readonly IntegerRange ZERO = new IntegerRange(0, 0);
        public static readonly IntegerRange ZERO_TO_ONE = new IntegerRange(0, 1);

        public IntegerRange(int inMin, int inMax)
            : base(inMin, inMax)
        {
            _delta = Max - Min;
        }

        public override int GetRandom(Random inRandom)
        {
            return (int)(Min + (inRandom.NextDouble() * _delta));
        }

        protected override int _Lerp(float inValue)
        {
            return (int)(Min + (_delta * inValue));
        }
    }
}