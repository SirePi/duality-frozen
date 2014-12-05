// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Noise
{
    /// <summary>
    ///
    /// </summary>
    public class ColoredNoise : Noise
    {
        private NoiseColor _color;

        /// <summary>
        ///
        /// </summary>
        /// <param name="inWidth"></param>
        /// <param name="inHeight"></param>
        /// <param name="inColor"></param>
        public void Generate(int inWidth, int inHeight, NoiseColor inColor)
        {
            _color = inColor;
            Generate(inWidth, inHeight);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inWidth"></param>
        /// <param name="inHeight"></param>
        protected override void _Generate(int inWidth, int inHeight)
        {
            if (_color == null)
            {
                _color = WhiteNoise.Instance;
            }

            for (int x = 0; x < inWidth; x++)
            {
                for (int y = 0; y < inHeight; y++)
                {
                    NoiseMap[x][y] += _color.GetValue(x, y);
                }
            }
        }
    }
}