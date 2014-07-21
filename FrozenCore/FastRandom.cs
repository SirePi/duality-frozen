// This code is provided under the MIT license. Originally by Roman Starkov.

using System;
using System.Collections.Generic;
using System.Linq;

namespace FrozenCore
{
    /// <summary>
    /// A XorShift-based fast random generator
    /// <see cref="http://roman.st/Article/Faster-Marsaglia-Xorshift-pseudo-random-generator-in-unsafe-C"/>
    /// </summary>
    public sealed class FastRandom : Xorshift
    {
        public override int FillBufferMultipleRequired { get { return 4; } }

        protected override void FillBuffer(byte[] buf, int offset, int offsetEnd)
        {
            while (offset < offsetEnd)
            {
                uint t = _x ^ (_x << 11);
                _x = _y; _y = _z; _z = _w;
                _w = _w ^ (_w >> 19) ^ (t ^ (t >> 8));
                buf[offset++] = (byte)(_w & 0xFF);
                buf[offset++] = (byte)((_w >> 8) & 0xFF);
                buf[offset++] = (byte)((_w >> 16) & 0xFF);
                buf[offset++] = (byte)((_w >> 24) & 0xFF);
            }
        }
    }

    public abstract class Xorshift : Random
    {
        protected uint _w = 88675123;
        protected uint _x = 123456789;
        protected uint _y = 362436069;
        protected uint _z = 521288629;
        private Queue<byte> _bytes = new Queue<byte>();
        public abstract int FillBufferMultipleRequired { get; }

        public override void NextBytes(byte[] buffer)
        {
            int offset = 0;
            while (_bytes.Any() && offset < buffer.Length)
                buffer[offset++] = _bytes.Dequeue();

            int length = ((buffer.Length - offset) / FillBufferMultipleRequired) * FillBufferMultipleRequired;
            if (length > 0)
                FillBuffer(buffer, offset, offset + length);

            offset += length;
            while (offset < buffer.Length)
            {
                if (_bytes.Count == 0)
                {
                    uint t = _x ^ (_x << 11);
                    _x = _y; _y = _z; _z = _w;
                    _w = _w ^ (_w >> 19) ^ (t ^ (t >> 8));
                    _bytes.Enqueue((byte)(_w & 0xFF));
                    _bytes.Enqueue((byte)((_w >> 8) & 0xFF));
                    _bytes.Enqueue((byte)((_w >> 16) & 0xFF));
                    _bytes.Enqueue((byte)((_w >> 24) & 0xFF));
                }
                buffer[offset++] = _bytes.Dequeue();
            }
        }

        protected abstract void FillBuffer(byte[] buf, int offset, int offsetEnd);
    }
}