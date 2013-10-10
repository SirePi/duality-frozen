// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Linq;
using Duality;
using Duality.ColorFormat;
using Duality.Resources;
using Duality.VertexFormat;
using FrozenCore.Data;
using OpenTK;

namespace FrozenCore.FX
{
    [Serializable]
    public class ParticleEmitter : Component, IDisposable, ICmpUpdatable, ICmpRenderer, ICmpInitializable
    {
        [NonSerialized]
        protected ColorRange _colorRange;

        [NonSerialized]
        protected float _emitterDirection;

        [NonSerialized]
        protected float _emitterRotationSpeed;

        [NonSerialized]
        protected FloatRange _initialDirectionRange;

        [NonSerialized]
        protected FloatRange _initialRotationRange;

        [NonSerialized]
        protected FloatRange _initialScaleRange;

        [NonSerialized]
        protected FloatRange _movementSpeedRange;

        [NonSerialized]
        protected IntegerRange _particlesNumberRange;

        [NonSerialized]
        protected FloatRange _rotationSpeedRange;

        [NonSerialized]
        protected FloatRange _scaleSpeedRange;

        [NonSerialized]
        protected FloatRange _timeToLiveRange;

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

        public float BoundRadius { get { return 0; } }

        public ColorRgba ColorEnd { get; set; }
        public ColorRgba ColorStart { get; set; }
        public bool DrawParticlesOffScreen { get; set; }
        public float EmitterDirection { get; set; }
        public float EmitterRotationSpeed { get; set; }
        public FXArea FXArea { get; set; }
        public Vector2 InitialDirection { get; set; }
        public Vector2 InitialRotation { get; set; }
        public Vector2 InitialScale { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public ContentRef<Material> Material { get; set; }
        public Vector2 MovementSpeed { get; set; }
        public int NewParticlesPerSecond { get; set; }
        public Vector2 ParticlesAmount { get; set; }
        public Vector2 RotationSpeed { get; set; }
        public Vector2 ScaleSpeed { get; set; }
        public Vector2 TimeToLive { get; set; }

        public VisibilityFlag VisibilityGroup { get; set; }

        public ParticleEmitter()
        {
            NewParticlesPerSecond = int.MaxValue;
            InitialScale = Vector2.One;
            ColorStart = Colors.White;
            ColorEnd = Colors.Transparent;

            DrawParticlesOffScreen = true;
            IsVisible = true;
            IsEnabled = true;

            VisibilityGroup = VisibilityFlag.Group0;
        }

        public void Burst()
        {
            _sendBurst = true;
        }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            _inEditor = (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Editor && DualityApp.ExecContext == DualityApp.ExecutionContext.Editor);

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
                Canvas c = new Canvas(device);
                if (FXArea != null)
                {
                    FXArea.DrawInEditor(c);
                }
            }
            else
            {
                if (_particlesAlive > 0)
                {
                    Vector3 posTemp = FXArea.GameObj.Transform.Pos;
                    float scaleTemp = 1f;
                    device.PreprocessCoords(ref posTemp, ref scaleTemp);

                    int index = 0;

                    foreach (Particle p in _particles)
                    {
                        if (p.IsAlive)
                        {
                            bool toDraw = DrawParticlesOffScreen || device.IsCoordInView(p.Position);

                            if (toDraw)
                            {
                                p.UpdateVertices(device, scaleTemp);
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
                        int particlesLimit = (int)Math.Ceiling(NewParticlesPerSecond * secondsPast);
                        /*
                        if (_particlesAlive < _particlesNumberRange.Min)
                        {
                            particlesLimit = _particlesNumberRange.Min - _particlesAlive;
                        }
                        */
                        int createdParticles = 0;
                        while (_particlesAlive < _particlesNumberRange.Max && createdParticles < particlesLimit)
                        {
                            AwakeParticle();

                            _particlesAlive++;
                            createdParticles++;
                        }
                    }
                }

                if (_sendBurst)
                {
                    _sendBurst = false;

                    foreach (Particle p in _particles)
                    {
                        InitializeParticle(p);
                    }
                }
            }
        }

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
            float movementSpeed = _movementSpeedRange.GetRandom(FrozenCore.FastRandom);
            float rotationSpeed = _rotationSpeedRange.GetRandom(FrozenCore.FastRandom);
            float scaleSpeed = _scaleSpeedRange.GetRandom(FrozenCore.FastRandom);

            float direction = _initialDirectionRange.GetRandom(FrozenCore.FastRandom);
            float rotation = _initialRotationRange.GetRandom(FrozenCore.FastRandom);
            float scale = _initialScaleRange.GetRandom(FrozenCore.FastRandom);

            float ttl = _timeToLiveRange.GetRandom(FrozenCore.FastRandom);

            Vector3 origin = FXArea.GetPoint(FrozenCore.FastRandom);

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