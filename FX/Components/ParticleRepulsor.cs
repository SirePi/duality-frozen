using Duality;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.FX.Properties;
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
    [EditorHintImage(ResNames.ImageFXRepulsor)]
    [EditorHintCategory(ResNames.CategoryFX)]
    public class ParticleRepulsor : ParticleAlterator
    {
        internal override void AlterParticle(Particle inParticle, float inSecondsPast)
        {
            Vector3 direction = this.GameObj.Transform.Pos - inParticle.Position;

            if (direction.Length < Radius)
            {
                direction *= (Radius - direction.Length) / Radius * Strength * inSecondsPast;
                direction = -direction;

                if (AlterParticleDirection)
                {
                    inParticle.AlterDirection(direction.Xy);

                    //float originalAngle = inParticle.Direction;
                    //float targetAngle = direction.Xy.Angle - MathF.PiOver2;
                    /*
					inParticle.Direction += MathF.CircularDist(originalAngle, targetAngle) *
						MathF.TurnDir(originalAngle, targetAngle) * strength;
                    */
                }
                else
                {
                    inParticle.Position += direction;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inCanvas"></param>
        protected override void _DrawInEditor(global::Duality.Drawing.Canvas inCanvas)
        {
            inCanvas.State.ColorTint = Colors.DarkRed;
            inCanvas.DrawCircle(this.GameObj.Transform.Pos.X, this.GameObj.Transform.Pos.Y, Radius);
        }
    }
}
