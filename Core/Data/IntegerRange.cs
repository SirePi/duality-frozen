// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Data
{
    /// <summary>
    /// Implementation of Range for int values
    /// </summary>
    public class IntegerRange : Range<int>
    {
        /// <summary>
        ///
        /// </summary>
        public static readonly IntegerRange ONE = new IntegerRange(1, 1);

        /// <summary>
        ///
        /// </summary>
        public static readonly IntegerRange ZERO = new IntegerRange(0, 0);

        /// <summary>
        ///
        /// </summary>
        public static readonly IntegerRange ZERO_TO_ONE = new IntegerRange(0, 1);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inMin"></param>
        /// <param name="inMax"></param>
        public IntegerRange(int inMin, int inMax)
            : base(inMin, inMax)
        {
            Delta = Max - Min;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inRandom"></param>
        /// <returns></returns>
        public override int GetRandom(Random inRandom)
        {
            return (int)(Min + (inRandom.NextDouble() * Delta));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        protected override int _Lerp(float inValue)
        {
            return (int)(Min + (Delta * inValue));
        }
    }
}