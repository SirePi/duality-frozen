// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using Duality;
using Duality.ColorFormat;
using Duality.VertexFormat;
using OpenTK;

namespace FrozenCore.FX
{
    [Serializable]
    public class Lightning : Component, ICmpUpdatable, ICmpRenderer, ICmpInitializable
    {
        [NonSerialized]
        private List<LightningBolt> _bolts;

        [NonSerialized]
        private bool _inEditor;

        [NonSerialized]
        private float _jaggedness;

        [NonSerialized]
        private float _timeSinceLastBolt;

        public float BoltLifeTime { get; set; }
        public ColorRgba Color { get; set; }
        public float EmitEvery { get; set; }
        public FXArea FXSource { get; set; }
        public FXArea FXTarget { get; set; }
        float ICmpRenderer.BoundRadius
        {
            get { return 0; }
        }
        public float Sway { get; set; }
        public float Thickness { get; set; }
        public VisibilityFlag VisibilityGroup { get; set; }

        public Lightning()
        {
            Sway = 100;
            Thickness = 2f;
            BoltLifeTime = 1;
            EmitEvery = 1;

            VisibilityGroup = VisibilityFlag.Group0;
            Color = Colors.White;
        }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            _inEditor = FrozenUtilities.IsDualityEditor;

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

                    FXSource.DrawInEditor(c);
                    FXTarget.DrawInEditor(c);

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
                    p3 += src + new Vector3(normal * .02f, p3.Z); ;
                    p4 += src + new Vector3(normal * -.04f, p4.Z); ;

                    c.CurrentState.ColorTint = Colors.Pink;
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
                foreach (LightningBolt bolt in _bolts)
                {
                    if (bolt.IsAlive)
                    {
                        if (!bolt.BatchInfos.ContainsKey(device) || !bolt.BatchInfos[device].IsReady)
                        {
                            bolt.PrepareTextureForDrawDevice(device);
                        }

                        LightningBolt.BoltData bd = bolt.BatchInfos[device];

                        float scaleAtStart = device.GetScaleAtZ(FXSource.GameObj.Transform.Pos.Z);
                        float scaleAtEnd = device.GetScaleAtZ(FXTarget.GameObj.Transform.Pos.Z);

                        Vector2 axis = (bd.End - bd.Start).Xy;
                        Vector2 normal = axis.PerpendicularLeft.Normalized;

                        VertexC1P3T2[] v = new VertexC1P3T2[4];

                        v[0] = new VertexC1P3T2();
                        v[0].Pos = bd.Start - new Vector3(normal * Sway * scaleAtStart, 0);
                        v[0].TexCoord = new Vector2(0, 0);
                        v[0].Color = bolt.CurrentColor;

                        v[1] = new VertexC1P3T2();
                        v[1].Pos = bd.Start + new Vector3(normal * Sway * scaleAtStart, 0);
                        v[1].TexCoord = new Vector2(0, 1);
                        v[1].Color = bolt.CurrentColor;

                        v[2] = new VertexC1P3T2();
                        v[2].Pos = bd.End + new Vector3(normal * Sway * scaleAtEnd, 0);
                        v[2].TexCoord = new Vector2(1, 1);
                        v[2].Color = bolt.CurrentColor;

                        v[3] = new VertexC1P3T2();
                        v[3].Pos = bd.End - new Vector3(normal * Sway * scaleAtEnd, 0);
                        v[3].TexCoord = new Vector2(1, 0);
                        v[3].Color = bolt.CurrentColor;

                        device.AddVertices(bd.BatchInfo, VertexMode.Quads, v);
                    }
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
            Vector3 source = FXSource.GetPoint(FrozenCore.FastRandom);
            Vector3 target = FXTarget.GetPoint(FrozenCore.FastRandom);

            inBolt.SetData(Sway, _jaggedness, source, target, Color, Thickness, BoltLifeTime);
        }
    }
}