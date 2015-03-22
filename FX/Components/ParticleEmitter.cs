// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.Core.Data;
using SnowyPeak.Duality.Plugin.Frozen.FX.Properties;
using System;
using System.Linq;

namespace SnowyPeak.Duality.Plugin.Frozen.FX.Components
{
    /// <summary>
    /// A Particle Emitter
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageParticleEmitter)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryFX)]
    public class ParticleEmitter : Component, IDisposable, ICmpUpdatable, ICmpRenderer, ICmpInitializable
    {
        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected ColorRange _colorRange;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected float _emitterDirection;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected float _emitterRotationSpeed;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected FloatRange _initialDirectionRange;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected FloatRange _initialRotationRange;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected FloatRange _initialScaleRange;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected FloatRange _movementSpeedRange;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected IntegerRange _particlesNumberRange;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected FloatRange _rotationSpeedRange;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected FloatRange _scaleSpeedRange;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected FloatRange _timeToLiveRange;

        [NonSerialized]
        private static readonly Vector2 DEFAULT_PARTICLES = new Vector2(50, 100);

        [NonSerialized]
        private static readonly int DEFAULT_PARTICLES_PER_SECOND = 10;

        [NonSerialized]
        private static readonly Vector2 DEFAULT_TTL = new Vector2(1, 2);

        [NonSerialized]
        private bool _inEditor;

        [NonSerialized]
        private ParticleMaterial _particleMaterial;

        [NonSerialized]
        private Particle[] _particles;

        [NonSerialized]
        private int _particlesAlive;

        [NonSerialized]
        private VertexC1P3T2[] _particleVertices;

        [NonSerialized]
        private bool _sendBurst;

        [NonSerialized]
        private float _timeSinceLastParticle;

        /// <summary>
        ///
        /// </summary>
        public ParticleEmitter()
        {
            NewParticlesPerSecond = int.MaxValue;
            InitialScale = Vector2.One;
            ColorStart = Colors.White;
            ColorEnd = Colors.Transparent;

            DrawParticlesOffScreen = true;
            IsVisible = true;
            IsEnabled = true;

            NewParticlesPerSecond = DEFAULT_PARTICLES_PER_SECOND;
            InitialScale = Vector2.One;
            ParticlesAmount = DEFAULT_PARTICLES;
            TimeToLive = DEFAULT_TTL;

            VisibilityGroup = VisibilityFlag.Group0;
        }

        /// <summary>
        ///
        /// </summary>
        public float BoundRadius { get { return 0; } }

        /// <summary>
        /// [GET / SET] the Color tint of the particles at the end of their lifetime
        /// </summary>
        public ColorRgba ColorEnd { get; set; }

        /// <summary>
        /// [GET / SET] the Color tint of the particles at the beginning of their lifetime
        /// </summary>
        public ColorRgba ColorStart { get; set; }

        /// <summary>
        /// [GET / SET] if off-screen particles should be drawn; Can improve performance to keep it off, but could
        /// also cause particles to disappear prematurely if too big.
        /// Only the particle's Center Coordinate is compared to the screen viewport.
        /// </summary>
        public bool DrawParticlesOffScreen { get; set; }

        /// <summary>
        /// [GET / SET] the starting relative direction of the emitter
        /// </summary>
        public float EmitterDirection { get; set; }

        /// <summary>
        /// [GET / SET] the rotation speed (in degrees/second) of the emitter
        /// </summary>
        public float EmitterRotationSpeed { get; set; }

        /// <summary>
        /// [GET / SET] the FXArea generating particles
        /// </summary>
        public FXArea FXArea { get; set; }

        /// <summary>
        /// [GET / SET] the initial direction range of the particles
        /// </summary>
        public Vector2 InitialDirection { get; set; }

        /// <summary>
        /// [GET / SET] the initial rotation range of the particles
        /// </summary>
        public Vector2 InitialRotation { get; set; }

        /// <summary>
        /// [GET / SET] the initial scale range of the particles
        /// </summary>
        public Vector2 InitialScale { get; set; }

        /// <summary>
        /// [GET / SET] if new particles are generated
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// [GET / SET] if particles are drawn
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// [GET / SET] the material used for the particles
        /// </summary>
        public ContentRef<Material> Material { get; set; }

        /// <summary>
        /// [GET / SET] the movement speed range of the particles
        /// </summary>
        public Vector2 MovementSpeed { get; set; }

        /// <summary>
        /// [GET / SET] how many particles can be generated at most each second
        /// </summary>
        public int NewParticlesPerSecond { get; set; }

        /// <summary>
        /// [GET / SET] the minimum / maximum of particles that should be alive at a given moment
        /// </summary>
        public Vector2 ParticlesAmount { get; set; }

        /// <summary>
        /// [GET / SET] the rotation speed range of the particles
        /// </summary>
        public Vector2 RotationSpeed { get; set; }

        /// <summary>
        /// [GET / SET] the scale-change speed range of the particles
        /// </summary>
        public Vector2 ScaleSpeed { get; set; }

        /// <summary>
        /// [GET / SET] the time to live range of the particles
        /// </summary>
        public Vector2 TimeToLive { get; set; }

        /// <summary>
        /// [GET / SET] the VisibilityGroup
        /// </summary>
        public VisibilityFlag VisibilityGroup { get; set; }

        /// <summary>
        /// [GET] if there are particles still alive
        /// </summary>
        public bool HasAliveParticles { get { return _particlesAlive > 0; } }

        /// <summary>
        /// Sends a Burst of particles (emit all particles up to the maximum number in the next frame)
        /// </summary>
        public void Burst()
        {
            _sendBurst = true;
        }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            _inEditor = Utilities.IsDualityEditor;

            if (context == InitContext.Activate && Material != null)
            {
                _particleMaterial = new ParticleMaterial(Material);

                _colorRange = new ColorRange(ColorStart, ColorEnd);
                _initialDirectionRange = new FloatRange(MathF.DegToRad(InitialDirection.X), MathF.DegToRad(InitialDirection.Y));
                _initialRotationRange = new FloatRange(MathF.DegToRad(InitialRotation.X), MathF.DegToRad(InitialRotation.Y));
                _initialScaleRange = new FloatRange(InitialScale.X, InitialScale.Y);
                _movementSpeedRange = new FloatRange(MovementSpeed.X, MovementSpeed.Y);
                _particlesNumberRange = new IntegerRange((int)ParticlesAmount.X, (int)ParticlesAmount.Y);
                _rotationSpeedRange = new FloatRange(MathF.DegToRad(RotationSpeed.X), MathF.DegToRad(RotationSpeed.Y));
                _scaleSpeedRange = new FloatRange(ScaleSpeed.X, ScaleSpeed.Y);
                _timeToLiveRange = new FloatRange(TimeToLive.X, TimeToLive.Y);

                _emitterDirection = EmitterDirection * MathF.Pi / 180;
                _emitterRotationSpeed = EmitterRotationSpeed * MathF.Pi / 180;

                _particleVertices = new VertexC1P3T2[_particlesNumberRange.Max * 4];

                _particles = new Particle[_particlesNumberRange.Max];
                for (int i = 0; i < _particles.Length; i++)
                {
                    _particles[i] = new Particle();
                }

                _timeSinceLastParticle = 0;
            }
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        {
            if (context == ShutdownContext.Deactivate)
            {
                KillAll();
                _particles = null;
            }
        }

        void ICmpRenderer.Draw(IDrawDevice device)
        {
            if (_inEditor)
            {
                if (FXArea != null)
                {
                    Canvas c = new Canvas(device);
                    FXArea.DrawInEditor(c, Colors.Cyan);
                }
            }
            else
            {
                if (_particlesAlive > 0)
                {
                    Vector3 minPos = FXArea.GameObj.Transform.Pos;
                    minPos.Z += FXArea.ZRange.Min;
                    float minScale = 1;

                    Vector3 maxPos = FXArea.GameObj.Transform.Pos;
                    maxPos.Z += FXArea.ZRange.Max;
                    float maxScale = 1;

                    device.PreprocessCoords(ref minPos, ref minScale);
                    device.PreprocessCoords(ref maxPos, ref maxScale);

                    Vector3Range posRange = new Vector3Range(minPos, maxPos);
                    FloatRange scaleRange = new FloatRange(minScale, maxScale);

                    Vector3 pos = minPos;
                    float scale = minScale;

                    int index = 0;
                    foreach (Particle p in _particles)
                    {
                        if (p.IsAlive)
                        {
                            bool toDraw = DrawParticlesOffScreen || device.IsCoordInView(p.Position);

                            if (FXArea.ZRange.Delta != 0)
                            {
                                float zScale = 1 - ((FXArea.ZRange.Max - p.Position.Z) / FXArea.ZRange.Delta);
                                pos = posRange.Lerp(zScale);
                                scale = scaleRange.Lerp(zScale);
                            }

                            if (toDraw)
                            {
                                p.UpdateVertices(device, pos, scale);
                                Array.Copy(p.Vertices, 0, _particleVertices, index * 4, 4);
                            }
                            index++;
                        }
                    }

                    device.AddVertices(_particleMaterial.Material, VertexMode.Quads, _particleVertices, index * 4);
                }
            }
        }

        bool ICmpRenderer.IsVisible(IDrawDevice device)
        {
            bool result = IsVisible;

            if (result)
            {
                // Differing ScreenOverlay flag? Don't render!
                if ((device.VisibilityMask & VisibilityFlag.ScreenOverlay) != (VisibilityGroup & VisibilityFlag.ScreenOverlay))
                {
                    result = false;
                }
                // No match in any VisibilityGroup? Don't render!
                if ((VisibilityGroup & device.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None)
                {
                    result = false;
                }
            }
            return result;
        }

        void ICmpUpdatable.OnUpdate()
        {
            if (!_inEditor && FXArea != null)
            {
                float secondsPast = Time.LastDelta / 1000f;

                _emitterDirection = (_emitterDirection + (_emitterRotationSpeed * secondsPast)) % MathF.TwoPi;

                _particlesAlive = 0;
                foreach (Particle p in _particles)
                {
                    if (p.IsAlive)
                    {
                        p.Update(secondsPast);
                        _particlesAlive++;
                    }
                }

                if (IsEnabled)
                {
                    if (_particlesAlive < _particlesNumberRange.Max)
                    {
                        _timeSinceLastParticle += secondsPast;
                        int particlesLimit = (int)Math.Floor(NewParticlesPerSecond * _timeSinceLastParticle);

                        if (_particlesAlive < _particlesNumberRange.Min)
                        {
                            particlesLimit = _particlesNumberRange.Min - _particlesAlive;
                        }

                        int createdParticles = 0;
                        while (_particlesAlive < _particlesNumberRange.Max && createdParticles < particlesLimit)
                        {
                            AwakeParticle();

                            _particlesAlive++;
                            createdParticles++;
                        }

                        if (createdParticles > 0)
                        {
                            _timeSinceLastParticle = 0;
                        }
                    }
                }

                if (_sendBurst)
                {
                    _sendBurst = false;

                    foreach (Particle p in _particles.Where(p => !p.IsAlive))
                    {
                        InitializeParticle(p);
                    }
                }
            }
        }

        /// <summary>
        /// Kills all particles currently active.
        /// </summary>
        public void KillAll()
        {
            if (_particles != null)
            {
                foreach (Particle p in _particles)
                {
                    p.Kill();
                }
            }
        }

        internal void AwakeParticle()
        {
            Particle particle = _particles.FirstOrDefault(p => !p.IsAlive);
            if (particle != null)
            {
                InitializeParticle(particle);
            }
        }

        internal virtual void InitializeParticle(Particle inParticle)
        {
            float movementSpeed = _movementSpeedRange.GetRandom(FastRandom.Instance);
            float rotationSpeed = _rotationSpeedRange.GetRandom(FastRandom.Instance);
            float scaleSpeed = _scaleSpeedRange.GetRandom(FastRandom.Instance);

            float direction = FXArea.GameObj.Transform.Angle + _initialDirectionRange.GetRandom(FastRandom.Instance);
            float rotation = FXArea.GameObj.Transform.Angle + _initialRotationRange.GetRandom(FastRandom.Instance);
            float scale = _initialScaleRange.GetRandom(FastRandom.Instance);

            float ttl = _timeToLiveRange.GetRandom(FastRandom.Instance);

            Vector3 origin = FXArea.GetPoint(FastRandom.Instance);

            inParticle.SetData(_particleMaterial,
                origin,
                movementSpeed,
                rotationSpeed,
                scaleSpeed,
                rotation,
                _emitterDirection + direction,
                scale,
                ttl,
                _colorRange);
        }
    }
}