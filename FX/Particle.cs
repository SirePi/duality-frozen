// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;

namespace SnowyPeak.Duality.Plugin.Frozen.FX
{
    /// <summary>
    /// Class modeling a single Particle emitted by a Particle emitter
    /// </summary>
    internal class Particle
    {
        private ColorRange _colorRange;
        private ColorRgba _currentColor;
        private float _lifeTime;
        private ParticleMaterial _material;
        private float _movementSpeed;
        private float _rotation;
        private float _rotationSpeed;
        private float _scale;
        private float _scaleSpeed;
        private float _timeToLive;
		private Rect _frameRect;
		private Rect _uvRect;
		private Vector2[] _vertexFactors;

        internal Particle()
        {
			_vertexFactors = new Vector2[4];
            Vertices = new VertexC1P3T2[4];
        }

		internal float Direction { get; set; }
		internal Vector3 Position { get; set; }

        internal bool IsAlive { get; private set; }
        internal VertexC1P3T2[] Vertices { get; private set; }

        internal void Kill()
        {
            IsAlive = false;
        }

        internal void SetData(ParticleMaterial inMaterial, Vector3 inOrigin, float inMovementSpeed, float inRotationSpeed, float inScaleSpeed, float inInitialRotation, float inInitialDirection, float inInitialScale, float inTimeToLive, ColorRange inColorRange, float randomFactor)
        {
            _material = inMaterial;
			_frameRect = inMaterial.GetFrame();

			Vector2 topLeft = _frameRect.TopLeft / inMaterial.Rectangle.Size;
			Vector2 size = _frameRect.Size / inMaterial.Rectangle.Size;
			_uvRect = Rect.Align(Alignment.TopLeft, topLeft.X, topLeft.Y, size.X, size.Y);

            Position = inOrigin;
            _scale = inInitialScale;
			Direction = inInitialDirection - MathF.PiOver2;
            _rotation = inInitialRotation;

            _movementSpeed = inMovementSpeed;
            _rotationSpeed = inRotationSpeed;
            _scaleSpeed = inScaleSpeed;

            _lifeTime = _timeToLive = inTimeToLive;

            _colorRange = inColorRange;
            _currentColor = _colorRange.Min;

            IsAlive = true;

			_vertexFactors[0] = Vector2.One - MathF.Rnd.NextVector2(0, 0, randomFactor, randomFactor); //topLeft
			_vertexFactors[1] = Vector2.One - MathF.Rnd.NextVector2(0, 0, randomFactor, randomFactor); //bottomLeft
			_vertexFactors[2] = Vector2.One - MathF.Rnd.NextVector2(0, 0, randomFactor, randomFactor); //bottomRight
			_vertexFactors[3] = Vector2.One - MathF.Rnd.NextVector2(0, 0, randomFactor, randomFactor); //topRight
        }

        internal void Update(float inElapsedTimeInSeconds)
        {
            _timeToLive -= inElapsedTimeInSeconds;
            IsAlive = _timeToLive > 0;

            if (IsAlive)
            {
                float decay = (_lifeTime - _timeToLive) / _lifeTime;

                Vector3 newPosition = Position;
				newPosition.X += (float)((Math.Cos(Direction) * _movementSpeed * inElapsedTimeInSeconds));
				newPosition.Y += (float)((Math.Sin(Direction) * _movementSpeed * inElapsedTimeInSeconds));

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

			Rect rectTemp = _frameRect.WithOffset(-_frameRect.Center).Transformed(_scale, _scale);
			Vector2 edge1 = rectTemp.TopLeft * _vertexFactors[0];
			Vector2 edge2 = rectTemp.BottomLeft * _vertexFactors[1];
			Vector2 edge3 = rectTemp.BottomRight * _vertexFactors[2];
			Vector2 edge4 = rectTemp.TopRight * _vertexFactors[3];

            MathF.TransformDotVec(ref edge1, ref xDot, ref yDot);
            MathF.TransformDotVec(ref edge2, ref xDot, ref yDot);
            MathF.TransformDotVec(ref edge3, ref xDot, ref yDot);
            MathF.TransformDotVec(ref edge4, ref xDot, ref yDot);

            Vertices[0].Pos.X = preprocessedPosition.X + edge1.X;
            Vertices[0].Pos.Y = preprocessedPosition.Y + edge1.Y;
            Vertices[0].Pos.Z = preprocessedPosition.Z;
			Vertices[0].TexCoord.X = _uvRect.X;
			Vertices[0].TexCoord.Y = _uvRect.Y;
            Vertices[0].Color = _currentColor;

            Vertices[1].Pos.X = preprocessedPosition.X + edge2.X;
            Vertices[1].Pos.Y = preprocessedPosition.Y + edge2.Y;
            Vertices[1].Pos.Z = preprocessedPosition.Z;
            Vertices[1].TexCoord.X = _uvRect.X;
			Vertices[1].TexCoord.Y = _uvRect.Y + _uvRect.H;
            Vertices[1].Color = _currentColor;

            Vertices[2].Pos.X = preprocessedPosition.X + edge3.X;
            Vertices[2].Pos.Y = preprocessedPosition.Y + edge3.Y;
            Vertices[2].Pos.Z = preprocessedPosition.Z;
			Vertices[2].TexCoord.X = _uvRect.X + _uvRect.W;
			Vertices[2].TexCoord.Y = _uvRect.Y + _uvRect.H;
            Vertices[2].Color = _currentColor;

            Vertices[3].Pos.X = preprocessedPosition.X + edge4.X;
            Vertices[3].Pos.Y = preprocessedPosition.Y + edge4.Y;
            Vertices[3].Pos.Z = preprocessedPosition.Z;
			Vertices[3].TexCoord.X = _uvRect.X + _uvRect.W;
			Vertices[3].TexCoord.Y = _uvRect.Y;
            Vertices[3].Color = _currentColor;
        }
    }
}