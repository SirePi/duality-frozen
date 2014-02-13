// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.ColorFormat;
using Duality.VertexFormat;
using FrozenCore.Data;
using OpenTK;

namespace FrozenCore.FX
{
    internal class Particle
    {
        private ColorRange _colorRange;
        private ColorRgba _currentColor;
        private float _direction;
        private float _lifeTime;
        private ParticleMaterial _material;
        private float _movementSpeed;
        private float _rotation;
        private float _rotationSpeed;
        private float _scale;
        private float _scaleSpeed;
        private float _timeToLive;

        public bool IsAlive { get; private set; }
        public Vector3 Position { get; private set; }
        public VertexC1P3T2[] Vertices { get; private set; }

        internal Particle()
        {
            Vertices = new VertexC1P3T2[4];
        }

        internal void Kill()
        {
            IsAlive = false;
        }

        internal void SetData(ParticleMaterial inMaterial, Vector3 inOrigin, float inMovementSpeed, float inRotationSpeed, float inScaleSpeed, float inInitialRotation, float inInitialDirection, float inInitialScale, float inTimeToLive, ColorRange inColorRange)
        {
            _material = inMaterial;

            Position = inOrigin;
            _scale = inInitialScale;
            _direction = inInitialDirection;
            _rotation = inInitialRotation;

            _movementSpeed = inMovementSpeed;
            _rotationSpeed = inRotationSpeed;
            _scaleSpeed = inScaleSpeed;

            _lifeTime = _timeToLive = inTimeToLive;

            _colorRange = inColorRange;
            _currentColor = _colorRange.Min;

            IsAlive = true;
        }

        internal void Update(float inElapsedTimeInSeconds)
        {
            _timeToLive -= inElapsedTimeInSeconds;
            IsAlive = _timeToLive > 0;

            if (IsAlive)
            {
                float decay = (_lifeTime - _timeToLive) / _lifeTime;

                Vector3 newPosition = Position;
                newPosition.X += (float)((Math.Cos(_direction) * _movementSpeed * inElapsedTimeInSeconds));
                newPosition.Y += (float)((Math.Sin(_direction) * _movementSpeed * inElapsedTimeInSeconds));

                Position = newPosition;
                _rotation += (_rotationSpeed * inElapsedTimeInSeconds);
                _scale += (_scaleSpeed * inElapsedTimeInSeconds);
                _currentColor = _colorRange.Lerp(decay);
            }
        }

        internal void UpdateVertices(IDrawDevice device, Vector3 inPreprocessedPosition, float inPreprocessedScale)
        {
            Vector3 preprocessedPosition = inPreprocessedPosition + (Position * inPreprocessedScale);

            Vector2 xDot, yDot;
            MathF.GetTransformDotVec(_rotation, inPreprocessedScale, out xDot, out yDot);

            Rect rectTemp = _material.Rectangle.Transform(_scale, _scale);
            Vector2 edge1 = rectTemp.TopLeft;
            Vector2 edge2 = rectTemp.BottomLeft;
            Vector2 edge3 = rectTemp.BottomRight;
            Vector2 edge4 = rectTemp.TopRight;

            MathF.TransformDotVec(ref edge1, ref xDot, ref yDot);
            MathF.TransformDotVec(ref edge2, ref xDot, ref yDot);
            MathF.TransformDotVec(ref edge3, ref xDot, ref yDot);
            MathF.TransformDotVec(ref edge4, ref xDot, ref yDot);

            Vertices[0].Pos.X = preprocessedPosition.X + edge1.X;
            Vertices[0].Pos.Y = preprocessedPosition.Y + edge1.Y;
            Vertices[0].Pos.Z = preprocessedPosition.Z;
            Vertices[0].TexCoord.X = 0;
            Vertices[0].TexCoord.Y = 0;
            Vertices[0].Color = _currentColor;

            Vertices[1].Pos.X = preprocessedPosition.X + edge2.X;
            Vertices[1].Pos.Y = preprocessedPosition.Y + edge2.Y;
            Vertices[1].Pos.Z = preprocessedPosition.Z;
            Vertices[1].TexCoord.X = 0;
            Vertices[1].TexCoord.Y = 1;
            Vertices[1].Color = _currentColor;

            Vertices[2].Pos.X = preprocessedPosition.X + edge3.X;
            Vertices[2].Pos.Y = preprocessedPosition.Y + edge3.Y;
            Vertices[2].Pos.Z = preprocessedPosition.Z;
            Vertices[2].TexCoord.X = 1;
            Vertices[2].TexCoord.Y = 1;
            Vertices[2].Color = _currentColor;

            Vertices[3].Pos.X = preprocessedPosition.X + edge4.X;
            Vertices[3].Pos.Y = preprocessedPosition.Y + edge4.Y;
            Vertices[3].Pos.Z = preprocessedPosition.Z;
            Vertices[3].TexCoord.X = 1;
            Vertices[3].TexCoord.Y = 0;
            Vertices[3].Color = _currentColor;
        }
    }
}