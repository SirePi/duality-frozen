//
// System.Random.cs
//
// Authors:
//   Bob Smith (bob@thestuff.net)
//   Ben Maurer (bmaurer@users.sourceforge.net)
//   Sebastien Pouliot  <sebastien@xamarin.com>
//   Alessandro Pilati (github.com/SirePi)
//
// (C) 2001 Bob Smith.  http://www.thestuff.net
// (C) 2003 Ben Maurer
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
// Copyright 2013 Xamarin Inc. (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.Core
{
    /// <summary>
    /// Mono's Random implementation. Actually faster than System.Random
    /// </summary>
    public class FastRandom : Random
    {
        private static FastRandom __instance;
        private uint _c;
        private uint _x;
        private uint _y;
        private uint _z;

        /// <summary>
        /// Constructor
        /// </summary>
        public FastRandom()
            : this(Environment.TickCount)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Seed"></param>
        public FastRandom(int Seed)
        {
            Reseed(Seed);
        }

        /// <summary>
        ///
        /// </summary>
        public static FastRandom Instance
        {
            get
            {
                if (__instance == null)
                {
                    __instance = new FastRandom();
                }

                return __instance;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public override int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("Maximum value is less than minimal value.");

            // special case: a difference of one (or less) will always return the minimum
            // e.g. -1,-1 or -1,0 will always return -1
            uint diff = (uint)(maxValue - minValue);
            if (diff <= 1)
                return minValue;

            return minValue + ((int)(JKiss() % diff));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public override int Next(int maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("Maximum value is less than minimal value.");

            return maxValue > 0 ? (int)(JKiss() % maxValue) : 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int Next()
        {
            // returns a non-negative, [0 - Int32.MacValue], random number
            // but we want to avoid calls to Math.Abs (call cost and branching cost it requires)
            // and the fact it would throw for Int32.MinValue (so roughly 1 time out of 2^32)
            int random = (int)JKiss();
            while (random == Int32.MinValue)
                random = (int)JKiss();
            int mask = random >> 31;
            random ^= mask;
            return random + (mask & 1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffer"></param>
        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            // each random `int` can fill 4 bytes
            int p = 0;
            uint random;
            for (int i = 0; i < (buffer.Length >> 2); i++)
            {
                random = JKiss();
                buffer[p++] = (byte)(random >> 24);
                buffer[p++] = (byte)(random >> 16);
                buffer[p++] = (byte)(random >> 8);
                buffer[p++] = (byte)random;
            }
            if (p == buffer.Length)
                return;

            // complete the array
            random = JKiss();
            while (p < buffer.Length)
            {
                buffer[p++] = (byte)random;
                random >>= 8;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override double NextDouble()
        {
            // return a double value between [0,1]
            return Sample();
        }

        /// <summary>
        /// Sets the seed
        /// </summary>
        /// <param name="Seed"></param>
        public void Reseed(int Seed)
        {
            _x = (uint)Seed;
            _y = (uint)987654321;
            _z = (uint)43219876;
            _c = (uint)6543217;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override double Sample()
        {
            // a single 32 bits random value is not enough to create a random double value
            uint a = JKiss() >> 6;	// Upper 26 bits
            uint b = JKiss() >> 5;	// Upper 27 bits
            return (a * 134217728.0 + b) / 9007199254740992.0;
        }

        private uint JKiss()
        {
            _x = 314527869 * _x + 1234567;
            _y ^= _y << 5;
            _y ^= _y >> 7;
            _y ^= _y << 22;
            ulong t = ((ulong)4294584393 * _z + _c);
            _c = (uint)(t >> 32);
            _z = (uint)t;
            return (_x + _y + _z);
        }
    }
}