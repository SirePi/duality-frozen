// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;

namespace FrozenCore.Data
{
    /// <summary>
    /// Abstract class used to interpolate and obtain random values between a maximum and a minimum defined.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Range<T>
    {
        protected T _delta;
        public T Max { get; protected set; }
        public T Min { get; protected set; }

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

        public abstract T GetRandom(Random inRandom);

        /// <summary>
        /// Get the value corresponding to the required interpolation. Parameter must be between 0 and 1
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public T Lerp(float inValue)
        {
            System.Diagnostics.Debug.Assert(inValue >= 0);
            System.Diagnostics.Debug.Assert(inValue <= 1);

            return _Lerp(inValue);
        }

        public override string ToString()
        {
            return String.Format("{0} -> {1}", Min, Max);
        }

        protected abstract T _Lerp(float inValue);
    }
}