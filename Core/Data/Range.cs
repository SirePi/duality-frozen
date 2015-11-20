// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Data
{
    /// <summary>
    /// Abstract class used to interpolate and obtain random values between a maximum and a minimum defined.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Range<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inMin"></param>
        /// <param name="inMax"></param>
        public Range(T inMin, T inMax)
        {
            Min = inMin;
            Max = inMax;
        }

        /// <summary>
        ///
        /// </summary>
        public T Delta { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public T Max { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        public T Min { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inRandom">The random instance to use</param>
        /// <returns></returns>
        public abstract T GetRandom();

        /// <summary>
        /// Get the value corresponding to the required interpolation. Parameter must be between 0 and 1
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public T Lerp(float inValue)
        {
			inValue = MathF.Max(MathF.Min(1, inValue), 0);
            
			//System.Diagnostics.Debug.Assert(inValue >= 0);
            //System.Diagnostics.Debug.Assert(inValue <= 1);

            return _Lerp(inValue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} -> {1}", Min, Max);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        protected abstract T _Lerp(float inValue);
    }
}