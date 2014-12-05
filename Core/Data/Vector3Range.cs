// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using OpenTK;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Data
{
    /// <summary>
    /// Implementation of Range for Vector3 values
    /// </summary>
    public class Vector3Range : Range<Vector3>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="inMin"></param>
        /// <param name="inMax"></param>
        public Vector3Range(Vector3 inMin, Vector3 inMax)
            : base(inMin, inMax)
        {
            Delta = Max - Min;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inRandom"></param>
        /// <returns></returns>
        public override Vector3 GetRandom(Random inRandom)
        {
            return Min + ((float)inRandom.NextDouble() * Delta);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        protected override Vector3 _Lerp(float inValue)
        {
            return Min + (Delta * inValue);
        }
    }
}