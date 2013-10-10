﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System.Collections.Generic;
using System.Linq;
using Duality;
using Duality.ColorFormat;
using Duality.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FrozenCore.FX
{
    /// <summary>
    /// Based on http://gamedev.tutsplus.com/tutorials/implementation/how-to-generate-shockingly-good-2d-lightning-effects/
    /// </summary>
    internal class LightningBolt
    {
        internal class BoltData
        {
            internal BatchInfo BatchInfo { get; set; }
            internal Vector3 End { get; set; }
            internal bool IsReady { get; set; }
            internal Vector3 Start { get; set; }
        }

        private Vector3 _absoluteEnd;
        private Vector3 _absoluteStart;
        private float _lifeTime;
        private System.Drawing.Pen _pen;
        private float _sway2;
        private float _timeToLive;
        private List<Vector2> _verticesPositions;
        public Dictionary<IDrawDevice, BoltData> BatchInfos { get; private set; }

        public ColorRgba CurrentColor { get; private set; }
        public float CurrentThickness { get; private set; }
        public bool IsAlive { get; private set; }

        internal LightningBolt()
        {
            _verticesPositions = new List<Vector2>();
            _pen = new System.Drawing.Pen(System.Drawing.Color.White);
            BatchInfos = new Dictionary<IDrawDevice, BoltData>();
        }

        internal void PrepareTextureForDrawDevice(IDrawDevice device)
        {
            System.Drawing.PointF[] points = new System.Drawing.PointF[_verticesPositions.Count];

            /**
             * Drawing texture based on displacement for each camera
             **/
            Vector3 start = _absoluteStart * device.GetScaleAtZ(_absoluteStart.Z);
            Vector3 end = _absoluteEnd * device.GetScaleAtZ(_absoluteEnd.Z);

            Vector2 tangent = (end - start).Xy;
            float length = tangent.Length;

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new System.Drawing.PointF(_verticesPositions[i].X * length, _verticesPositions[i].Y);
            }

            System.Drawing.Bitmap pixelData = new System.Drawing.Bitmap((int)MathF.Ceiling(length), (int)MathF.Ceiling(_sway2));

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(pixelData))
            {
                g.Clear(System.Drawing.Color.Transparent);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                g.DrawLines(_pen, points);
            }

            if (!BatchInfos.ContainsKey(device))
            {
                BatchInfo bi = new BatchInfo(
                    DrawTechnique.Add,
                    Colors.White,
                    new ContentRef<Texture>(
                        new Texture(new ContentRef<Pixmap>(new Pixmap()))
                        {
                            WrapX = TextureWrapMode.ClampToEdge,
                            WrapY = TextureWrapMode.ClampToEdge,
                            TexSizeMode = Texture.SizeMode.Stretch
                        }));

                BatchInfos.Add(device, new BoltData() { BatchInfo = bi });
            }

            BoltData bd = BatchInfos[device];

            Texture tx = bd.BatchInfo.MainTexture.Res;
            tx.BasePixmap.Res.MainLayer.FromBitmap(pixelData);
            tx.ReloadData();

            bd.Start = start;
            bd.End = end;
            bd.IsReady = true;
        }

        internal void SetData(float inSway, float inJaggedness, Vector3 inStart, Vector3 inEnd, ColorRgba inColor, float inThickness, float inTimeToLive)
        {
            _absoluteStart = inStart;
            _absoluteEnd = inEnd;

            _lifeTime = _timeToLive = inTimeToLive;
            _pen.Width = inThickness;

            float sway = inSway + inThickness;
            _sway2 = sway * 2;
            Vector2 tangent = (inEnd - inStart).Xy;
            float length = tangent.Length;

            /**
             * Preparing vertices
             **/
            _verticesPositions.Clear();
            _verticesPositions.Add(new Vector2(0, sway));
            _verticesPositions.Add(new Vector2(1, sway));

            for (int i = 0; i < length / 4; i++)
            {
                _verticesPositions.Add(new Vector2((float)(FrozenCore.FastRandom.NextDouble() * 0.9 + 0.05), sway));
            }
            _verticesPositions = _verticesPositions.OrderBy(v => v.X).ToList();

            /**
             * Calculating displacements
             **/
            float prevDisplacement = 0;
            for (int i = 1; i < _verticesPositions.Count - 1; i++)
            {
                Vector2 pos = _verticesPositions[i];

                float scale = (length * inJaggedness) * (_verticesPositions[i].X - _verticesPositions[i - 1].X);

                float displacement = ((float)FrozenCore.FastRandom.NextDouble() * _sway2) - sway;
                displacement -= (displacement - prevDisplacement) * (1 - scale);

                pos.Y += displacement;
                _verticesPositions[i] = pos;

                prevDisplacement = displacement;
            }

            IsAlive = true;
            CurrentThickness = inThickness;
            CurrentColor = inColor;
        }

        internal void Update(float inElapsedTimeInSeconds)
        {
            _timeToLive -= inElapsedTimeInSeconds;
            IsAlive = _timeToLive > 0;

            if (IsAlive)
            {
                float decay = (_lifeTime - _timeToLive) / _lifeTime;
                CurrentColor = CurrentColor.WithAlpha(1 - MathF.Pow(decay, .25f));
            }
            else
            {
                foreach (BoltData bd in BatchInfos.Values)
                {
                    bd.IsReady = false;
                }
            }
        }
    }
}