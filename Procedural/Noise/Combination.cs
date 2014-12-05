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
    public sealed class Add : Combination
    {
        private float? _value;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Add(Noise a, Noise b)
            : base(a, b)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Add(Noise a, float b)
            : base(a, null)
        {
            _value = b;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected override void _Combine(int x, int y)
        {
            if (!_value.HasValue)
                NoiseMap[x][y] = (_a != null ? _a.NoiseMap[x][y] : 0) + (_b != null ? _b.NoiseMap[x][y] : 0);
            else
                NoiseMap[x][y] = (_a != null ? _a.NoiseMap[x][y] : 0) + _value.Value;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class Combination : Noise
    {
        /// <summary>
        ///
        /// </summary>
        protected Noise _a;

        /// <summary>
        ///
        /// </summary>
        protected Noise _b;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        protected Combination(Noise a, Noise b)
        {
            _a = a;
            _b = b;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected abstract void _Combine(int x, int y);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inWidth"></param>
        /// <param name="inHeight"></param>
        protected override void _Generate(int inWidth, int inHeight)
        {
            if (_a != null)
                _a.Generate(inWidth, inHeight);
            if (_b != null)
                _b.Generate(inWidth, inHeight);

            _Prepare();
            for (int x = 0; x < NoiseMap.Length; x++)
            {
                for (int y = 0; y < NoiseMap[x].Length; y++)
                {
                    _Combine(x, y);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void _Prepare()
        { }

        /// <summary>
        ///
        /// </summary>
        protected override void OnSeedChange()
        {
            if (_a != null)
                _a.Seed = Seed;
            if (_b != null)
                _b.Seed = Seed;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class Max : Combination
    {
        private float? _max;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Max(Noise a, Noise b)
            : base(a, b)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Max(Noise a, float b)
            : base(a, null)
        {
            _max = b;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected override void _Combine(int x, int y)
        {
            if (!_max.HasValue)
                NoiseMap[x][y] = Math.Max((_a != null ? _a.NoiseMap[x][y] : 0), (_b != null ? _b.NoiseMap[x][y] : 0));
            else
                NoiseMap[x][y] = Math.Max((_a != null ? _a.NoiseMap[x][y] : 0), _max.Value);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class Min : Combination
    {
        private float? _min;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Min(Noise a, Noise b)
            : base(a, b)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Min(Noise a, float b)
            : base(a, null)
        {
            _min = b;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected override void _Combine(int x, int y)
        {
            if (!_min.HasValue)
                NoiseMap[x][y] = Math.Min((_a != null ? _a.NoiseMap[x][y] : 1), (_b != null ? _b.NoiseMap[x][y] : 1));
            else
                NoiseMap[x][y] = Math.Min((_a != null ? _a.NoiseMap[x][y] : 1), _min.Value);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class Multiply : Combination
    {
        private float? _value;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Multiply(Noise a, Noise b)
            : base(a, b)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Multiply(Noise a, float b)
            : base(a, null)
        {
            _value = b;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected override void _Combine(int x, int y)
        {
            if (!_value.HasValue)
                NoiseMap[x][y] = (_a != null ? _a.NoiseMap[x][y] : 0) * (_b != null ? _b.NoiseMap[x][y] : 0);
            else
                NoiseMap[x][y] = (_a != null ? _a.NoiseMap[x][y] : 0) * _value.Value;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class Normalize : Combination
    {
        private float _max;
        private float _min;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        public Normalize(Noise a)
            : this(a, 0, 1)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Normalize(Noise a, float min, float max)
            : base(a, null)
        {
            _min = min;
            _max = max;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected override void _Combine(int x, int y)
        {
            NoiseMap[x][y] = (_a != null ? _a.NoiseMap[x][y] : 0);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void _Prepare()
        {
            if (_a != null)
            {
                _a.Normalize(_min, _max);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class Not : Combination
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        public Not(Noise a)
            : base(a, null)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected override void _Combine(int x, int y)
        {
            NoiseMap[x][y] = 1 - (_a != null ? _a.NoiseMap[x][y] : 0);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class Subtract : Combination
    {
        private float? _value;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Subtract(Noise a, Noise b)
            : base(a, b)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Subtract(Noise a, float b)
            : base(a, null)
        {
            _value = b;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected override void _Combine(int x, int y)
        {
            if (!_value.HasValue)
                NoiseMap[x][y] = (_a != null ? _a.NoiseMap[x][y] : 0) - (_b != null ? _b.NoiseMap[x][y] : 0);
            else
                NoiseMap[x][y] = (_a != null ? _a.NoiseMap[x][y] : 0) - _value.Value;
        }
    }
}