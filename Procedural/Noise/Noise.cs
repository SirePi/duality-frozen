// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Duality;
using Duality.Drawing;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;

namespace SnowyPeak.Duality.Plugin.Frozen.Procedural.Noise
{
    /// <summary>
    ///
    /// </summary>
    public abstract class Noise
    {
        /// <summary>
        /// 
        /// </summary>
        protected float _maxValue;

        /// <summary>
        ///
        /// </summary>
        protected float _minValue;

        /// <summary>
        ///
        /// </summary>
        protected FastRandom _rnd;

        private int _seed;

        /// <summary>
        ///
        /// </summary>
        protected Noise()
        {
            _rnd = new FastRandom();
        }

        /// <summary>
        ///
        /// </summary>
        [Flags]
        protected enum Wrap
        {
#pragma warning disable 1591
            None = 0x00,
            Left = 0x01,
            Right = 0x02,
            Top = 0x04,
            Bottom = 0x08
#pragma warning restore 1591
        }

        /// <summary>
        ///
        /// </summary>
        public float[][] NoiseMap { get; private set; }
        /// <summary>
        ///
        /// </summary>
        public int Seed
        {
            get { return _seed; }
            set
            {
                _seed = value;
                _rnd.Reseed(_seed);
                OnSeedChange();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inWidth"></param>
        /// <param name="inHeight"></param>
        public void Generate(int inWidth, int inHeight)
        {
            PrepareMap(inWidth, inHeight);
            _Generate(inWidth, inHeight);

            UpdateMinMax();
        }

        /// <summary>
        ///
        /// </summary>
        public void Normalize()
        {
            Normalize(0, 1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inNormalizedMin"></param>
        /// <param name="inNormalizedMax"></param>
        public void Normalize(float inNormalizedMin, float inNormalizedMax)
        {
            float delta = _maxValue - _minValue;
            float deltaNorm = inNormalizedMax - inNormalizedMin;

            for (int x = 0; x < NoiseMap.Length; x++)
            {
                for (int y = 0; y < NoiseMap[x].Length; y++)
                {
                    NoiseMap[x][y] = delta == 0 ? 0 : (NoiseMap[x][y] - _minValue) / delta; // becomes 0 - 1
                    NoiseMap[x][y] = (NoiseMap[x][y] * deltaNorm) + inNormalizedMin; // becomes normMin - normMax
                }
            }

            _minValue = inNormalizedMin;
            _maxValue = inNormalizedMax;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inWidth"></param>
        /// <param name="inHeight"></param>
        /// <param name="inColors"></param>
        /// <returns></returns>
        public Bitmap ToBitmap(int inWidth, int inHeight, ColorRange inColors)
        {
            Generate(inWidth, inHeight);

            Bitmap bmp = new Bitmap(inWidth, inHeight, PixelFormat.Format32bppArgb);

            BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            int BytesPerPixel = Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
            byte[] pixels = new byte[bitmapData.Stride * bmp.Height];
            IntPtr firstPixel = bitmapData.Scan0;

            Marshal.Copy(firstPixel, pixels, 0, pixels.Length);

            for (int y = 0; y < bitmapData.Height; y++)
            {
                int CurrentLine = y * bitmapData.Stride;
                for (int x = 0; x < bitmapData.Width; x++)
                {
                    float value = NoiseMap[x][y];
                    ColorRgba color = inColors.Lerp(value);

                    int kx = (x * BytesPerPixel);
                    pixels[CurrentLine + kx] = color.B;
                    pixels[CurrentLine + kx + 1] = color.G;
                    pixels[CurrentLine + kx + 2] = color.R;
                    pixels[CurrentLine + kx + 3] = color.A;
                }
            }

            // Copy modified bytes back
            Marshal.Copy(pixels, 0, firstPixel, pixels.Length);
            bmp.UnlockBits(bitmapData);

            return bmp;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inWidth"></param>
        /// <param name="inHeight"></param>
        protected abstract void _Generate(int inWidth, int inHeight);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnSeedChange()
        { }

        private void PrepareMap(int inWidth, int inHeight)
        {
            NoiseMap = new float[inWidth][];

            for (int x = 0; x < inWidth; x++)
            {
                NoiseMap[x] = new float[inHeight];
            }
        }

        private void UpdateMinMax()
        {
            _minValue = float.PositiveInfinity;
            _maxValue = float.NegativeInfinity;
            for (int x = 0; x < NoiseMap.Length; x++)
            {
                for (int y = 0; y < NoiseMap[x].Length; y++)
                {
                    _maxValue = MathF.Max(_maxValue, NoiseMap[x][y]);
                    _minValue = MathF.Min(_minValue, NoiseMap[x][y]);
                }
            }
        }
    }
}