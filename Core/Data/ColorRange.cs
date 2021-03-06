﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Drawing;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Data
{
    /// <summary>
    /// Implementation of Range for ColorRgba values
    /// </summary>
    public class ColorRange : Range<ColorRgba>
    {
        private Vector4 _colorDelta;
        private Vector4 _max;
        private Vector4 _min;

        /// <summary>
        ///
        /// </summary>
        /// <param name="inMin"></param>
        /// <param name="inMax"></param>
        public ColorRange(ColorRgba inMin, ColorRgba inMax)
            : base(inMin, inMax)
        {
            _min = new Vector4(inMin.R, inMin.G, inMin.B, inMin.A);
            _max = new Vector4(inMax.R, inMax.G, inMax.B, inMax.A);

            _colorDelta = _max - _min;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inRandom"></param>
        /// <returns></returns>
        public override ColorRgba GetRandom( )
        {
            Vector4 color = _min + GetDeltaValue((float)MathF.Rnd.NextFloat());
            return new ColorRgba((byte)color.X, (byte)color.Y, (byte)color.Z, (byte)color.W);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inValue"></param>
        /// <returns></returns>
        protected override ColorRgba _Lerp(float inValue)
        {
			inValue = MathF.Max(MathF.Min(1, inValue), 0);

            Vector4 color = _min + GetDeltaValue(inValue);
            return new ColorRgba((byte)color.X, (byte)color.Y, (byte)color.Z, (byte)color.W);
        }

        private Vector4 GetDeltaValue(float inValue)
        {
            return _colorDelta * inValue;
        }
    }
}