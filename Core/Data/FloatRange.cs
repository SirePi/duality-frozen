// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Data
{
    /// <summary>
    /// Implementation of Range for float values
    /// </summary>
    public class FloatRange : Range<float>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly FloatRange DIRECTION_2PI = new FloatRange(0, (float)MathF.TwoPi);

        /// <summary>
        ///
        /// </summary>
        public static readonly FloatRange ONE = new FloatRange(1, 1);

        /// <summary>
        ///
        /// </summary>
        public static readonly FloatRange ZERO = new FloatRange(0, 0);

        /// <summary>
        ///
        /// </summary>
        public static readonly FloatRange ZERO_TO_ONE = new FloatRange(0, 1);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inMin"></param>
        /// <param name="inMax"></param>
        public FloatRange(float inMin, float inMax)
            : base(inMin, inMax)
        {
            Delta = Max - Min;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inRandom"></param>
        /// <returns></returns>
        public override float GetRandom()
        {
            return (float)(Min + (MathF.Rnd.NextDouble() * Delta));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        protected override float _Lerp(float inValue)
        {
            return (float)(Min + (Delta * inValue));
        }
    }
}