using Duality;
using Duality.Components;
using Duality.Drawing;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowyPeak.Duality.Plugin.Frozen.FX.Components
{
    /// <summary>
    /// 
    /// </summary>
    [RequiredComponent(typeof(Transform))]
    public abstract class ParticleAlterator : Component, ICmpUpdatable, ICmpRenderer
    {
        /// <summary>
        /// [GET/SET] The Strength of the effect applied to the particles - Altering the Direction only requires higher values
        /// </summary>
        public float Strength { get; set; }

        /// <summary>
        /// [GET/SET] The maximum radius where particles are affected. Particles outside this radius are not affected.
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// [GET/SET] If the particles' Direction should be altered or just their position.
        /// </summary>
        public bool AlterParticleDirection { get; set; }

        /// <summary>
        /// [GET/SET] The Emitter which particles will be affected by this entity.
        /// </summary>
        public ParticleEmitter AffectedEmitter { get; set; }

        void ICmpUpdatable.OnUpdate()
        {
            float secondsPast = Time.MsPFMult * Time.TimeMult / 1000f;
            AffectedEmitter.AlterParticles(this, secondsPast);
        }

        internal abstract void AlterParticle(Particle inParticle, float inSecondsPast);

        /// <summary>
        /// Method used to draw the entity inside the editor
        /// </summary>
        /// <param name="inCanvas"></param>
        protected abstract void _DrawInEditor(Canvas inCanvas);

        float ICmpRenderer.BoundRadius
        {
            get { return Radius; }
        }

        void ICmpRenderer.Draw(global::Duality.Drawing.IDrawDevice device)
        {
            if (Utilities.IsDualityEditor)
            {
                Canvas c = new Canvas(device);

                c.State.ColorTint = Colors.Black;
                Utilities.DrawCross(c, GameObj.Transform.Pos, 10);
                _DrawInEditor(c);
            }
        }

        bool ICmpRenderer.IsVisible(global::Duality.Drawing.IDrawDevice device)
        {
            bool result = false;

            if (Utilities.IsDualityEditor && AffectedEmitter != null)
            {
                result = (AffectedEmitter as ICmpRenderer).IsVisible(device);
            }

            return result;
        }
    }
}