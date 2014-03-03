// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality.Drawing;
using OpenTK;

namespace FrozenCore.Data
{
    /// <summary>
    /// Implementation of Range for ColorRgba values
    /// </summary>
    public class ColorRange : Range<ColorRgba>
    {
        private Vector4 _colorDelta;
        private Vector4 _max;
        private Vector4 _min;

        public ColorRange(ColorRgba inMin, ColorRgba inMax)
            : base(inMin, inMax)
        {
            _min = new Vector4(inMin.R, inMin.G, inMin.B, inMin.A);
            _max = new Vector4(inMax.R, inMax.G, inMax.B, inMax.A);

            _colorDelta = _max - _min;
        }

        public override ColorRgba GetRandom(Random inRandom)
        {
            Vector4 color = _min + GetDeltaValue((float)inRandom.NextDouble());
            return new ColorRgba((byte)color.X, (byte)color.Y, (byte)color.Z, (byte)color.W);
        }

        protected override ColorRgba _Lerp(float inValue)
        {
            Vector4 color = _min + GetDeltaValue(inValue);
            return new ColorRgba((byte)color.X, (byte)color.Y, (byte)color.Z, (byte)color.W);
        }

        private Vector4 GetDeltaValue(float inValue)
        {
            return _colorDelta * inValue;
        }
    }
}