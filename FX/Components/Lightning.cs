// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using Duality;
using Duality.Drawing;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.FX.Properties;
using SnowyPeak.Duality.Plugin.Frozen.Core;

namespace SnowyPeak.Duality.Plugin.Frozen.FX.Components
{
    /// <summary>
    /// A Lightning effect emitter. Requires 2 FXAreas to work
    /// </summary>
    [EditorHintImage(ResNames.ImageLightning)]
    [EditorHintCategory(ResNames.CategoryFX)]
    public class Lightning : Component, ICmpUpdatable, ICmpRenderer, ICmpInitializable
    {
        [DontSerialize]
        private List<LightningBolt> _bolts;

        [DontSerialize]
        private bool _inEditor;

        [DontSerialize]
        private float _jaggedness;

        [DontSerialize]
        private float _timeSinceLastBolt;

        /// <summary>
        /// Constructor
        /// </summary>
        public Lightning()
        {
            Sway = 100;
            Thickness = 2f;
            BoltLifeTime = 1;
            EmitEvery = 1;
            DetailLevel = 5;

            VisibilityGroup = VisibilityFlag.Group0;
            Color = Colors.White;
        }

        /// <summary>
        /// [GET / SET] The lifetime, in seconds, of a single Bolt on screen
        /// </summary>
        public float BoltLifeTime { get; set; }
        /// <summary>
        /// [GET / SET] The color of the Bolts emitted by this instance
        /// </summary>
        public ColorRgba Color { get; set; }
        /// <summary>
        /// [GET / SET] The span, in seconds, between the emission of 2 Bolts
        /// </summary>
        public float EmitEvery { get; set; }
        /// <summary>
        /// [GET / SET] The Source Area of the Bolts
        /// </summary>
        public FXArea FXSource { get; set; }
        /// <summary>
        /// [GET / SET] The Target Area of the Bolts
        /// </summary>
        public FXArea FXTarget { get; set; }
        /// <summary>
        /// [GET / SET] The DetailLevel of the lightning bolts. Lower values means more points in the lightning bolts.
        /// </summary>
        public int DetailLevel { get; set; }

        float ICmpRenderer.BoundRadius
        {
            get { return 0; }
        }
        /// <summary>
        /// [GET / SET] The maximum amount of Sway (distance from the line connecting the starting and ending point)
        /// each Bolt can move from the median line
        /// </summary>
        public float Sway { get; set; }
        /// <summary>
        /// [GET / SET] Thickness, in pixels, at focus distance of the Camera, of the Bolt
        /// </summary>
        public float Thickness { get; set; }
        /// <summary>
        /// [GET / SET] The VisibilityGroup of the Component
        /// </summary>
        public VisibilityFlag VisibilityGroup { get; set; }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            _inEditor = Utilities.IsDualityEditor;

            if (context == InitContext.Activate)
            {
                _bolts = new List<LightningBolt>();
                _jaggedness = 1 / Sway;
            }
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        {
            if (context == ShutdownContext.Deactivate)
            {
                _bolts.Clear();
            }
        }

        void ICmpRenderer.Draw(IDrawDevice device)
        {
            if (_inEditor)
            {
                if (FXSource != null && FXTarget != null)
                {
                    Canvas c = new Canvas(device);

                    FXSource.DrawInEditor(c, Colors.MediumPurple);
                    FXTarget.DrawInEditor(c, Colors.MediumPurple);

                    Vector3 src = FXSource.GameObj.Transform.Pos;
                    Vector3 tar = FXTarget.GameObj.Transform.Pos;

                    c.PushState();

                    Vector3 line = tar - src;
                    Vector2 normal = line.Xy.PerpendicularLeft;

                    Vector3 p1 = line * 0.33f;
                    Vector3 p2 = line * 0.33f;
                    Vector3 p3 = line * 0.67f;
                    Vector3 p4 = line * 0.67f;

                    p1 += src + new Vector3(normal * .04f, p1.Z);
                    p2 += src + new Vector3(normal * -.02f, p2.Z);
                    p3 += src + new Vector3(normal * .02f, p3.Z);
                    p4 += src + new Vector3(normal * -.04f, p4.Z);

                    c.State.ColorTint = Colors.Pink;
                    c.DrawLine(src.X, src.Y, src.Z, p1.X, p1.Y, p1.Z);
                    c.DrawLine(p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z);
                    c.DrawLine(p2.X, p2.Y, p2.Z, p3.X, p3.Y, p3.Z);
                    c.DrawLine(p3.X, p3.Y, p3.Z, p4.X, p4.Y, p4.Z);
                    c.DrawLine(p4.X, p4.Y, p4.Z, tar.X, tar.Y, tar.Z);

                    c.PopState();
                }
            }
            else
            {
                foreach (LightningBolt bolt in _bolts.Where(b => b.IsAlive))
                {
                    VertexC1P3T2[] v = new VertexC1P3T2[4];

                    if (!bolt.BatchInfos.ContainsKey(device) || !bolt.BatchInfos[device].IsReady)
                    {
                        bolt.PrepareTextureForDrawDevice(device);
                    }

                    LightningBolt.BoltData bd = bolt.BatchInfos[device];

                    Vector2 axis = (bd.End - bd.Start).Xy;
                    Vector2 normal = axis.PerpendicularLeft.Normalized;

                    Vector3 start = bd.Start;
                    Vector3 end = bd.End;
                    float scaleStartTemp = 1;
                    float scaleEndTemp = 1;

                    device.PreprocessCoords(ref start, ref scaleStartTemp);
                    device.PreprocessCoords(ref end, ref scaleEndTemp);

                    v[0] = new VertexC1P3T2();
                    v[0].Pos = start - new Vector3(normal * Sway * scaleStartTemp, 0);
                    v[0].TexCoord = Vector2.Zero;
                    v[0].Color = bolt.CurrentColor;

                    v[1] = new VertexC1P3T2();
                    v[1].Pos = start + new Vector3(normal * Sway * scaleStartTemp, 0);
                    v[1].TexCoord = Vector2.UnitY;
                    v[1].Color = bolt.CurrentColor;

                    v[2] = new VertexC1P3T2();
                    v[2].Pos = end + new Vector3(normal * Sway * scaleEndTemp, 0);
                    v[2].TexCoord = new Vector2(1, 1);
                    v[2].Color = bolt.CurrentColor;

                    v[3] = new VertexC1P3T2();
                    v[3].Pos = end - new Vector3(normal * Sway * scaleEndTemp, 0);
                    v[3].TexCoord = Vector2.UnitX;
                    v[3].Color = bolt.CurrentColor;

                    device.AddVertices(bd.BatchInfo, VertexMode.Quads, v);
                }
            }
        }

        bool ICmpRenderer.IsVisible(IDrawDevice device)
        {
            bool result = true;

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

            return result;
        }

        void ICmpUpdatable.OnUpdate()
        {
            if (DetailLevel == 0)
            {
                DetailLevel = 1;
            }

            if (!_inEditor && FXSource != null && FXTarget != null)
            {
                float secondsPast = Time.LastDelta / 1000f;
                _timeSinceLastBolt += secondsPast;

                foreach (LightningBolt bolt in _bolts)
                {
                    if (bolt.IsAlive)
                    {
                        bolt.Update(secondsPast);
                    }
                }

                if (_timeSinceLastBolt > EmitEvery)
                {
                    AwakeBolt();
                    _timeSinceLastBolt = 0;
                }
            }
        }

        internal void AwakeBolt()
        {
            LightningBolt bolt = _bolts.FirstOrDefault(b => !b.IsAlive);
            if (bolt == null)
            {
                bolt = new LightningBolt();
                _bolts.Add(bolt);
            }
            InitializeBolt(bolt);
        }

        internal virtual void InitializeBolt(LightningBolt inBolt)
        {
            Vector3 source = FXSource.GameObj.Transform.Pos + FXSource.GetPoint();
            Vector3 target = FXTarget.GameObj.Transform.Pos + FXTarget.GetPoint();

            inBolt.SetData(Sway, _jaggedness, source, target, Color, Thickness, DetailLevel, BoltLifeTime);
        }
    }
}