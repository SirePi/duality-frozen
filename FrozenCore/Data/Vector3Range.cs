// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality.Drawing;
using OpenTK;

namespace FrozenCore.Data
{
    /// <summary>
    /// Implementation of Range for Vector3 values
    /// </summary>
    public class Vector3Range : Range<Vector3>
    {
        private Vector3 _delta;

        public Vector3Range(Vector3 inMin, Vector3 inMax)
            : base(inMin, inMax)
        {
            _delta = Max - Min;
        }

        public override Vector3 GetRandom(Random inRandom)
        {
            return Min + ((float)inRandom.NextDouble() * _delta);
        }

        protected override Vector3 _Lerp(float inValue)
        {
            return Min + (_delta * inValue);
        }
    }
}