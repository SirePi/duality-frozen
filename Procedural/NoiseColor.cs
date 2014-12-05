using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using SnowyPeak.Duality.Plugin.Frozen.Core;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural
{
    /// <summary>
    ///
    /// </summary>
    public abstract class NoiseColor
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public abstract float GetValue(int x, int y);
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class WhiteNoise : NoiseColor
    {
        //http://www.dspguru.com/dsp/howtos/how-to-generate-white-gaussian-noise
        private static WhiteNoise __instance;

        private int _n;
        private float _varianceCoefficient;

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSamples"></param>
        public WhiteNoise(int inSamples)
        {
            _n = inSamples;
            _varianceCoefficient = MathF.Sqrt(12 / _n);
        }

        /// <summary>
        ///
        /// </summary>
        public static WhiteNoise Instance
        {
            get
            {
                if (__instance == null)
                    __instance = new WhiteNoise(20);

                return __instance;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override float GetValue(int x, int y)
        {
            float value = 0;

            for (int i = 0; i < _n; i++)
                value += FastRandom.Instance.NextFloat();

            value -= (_n / 2);              // avg set to 0
            value *= _varianceCoefficient;  // variance set to 1

            return value;
        }
    }
}