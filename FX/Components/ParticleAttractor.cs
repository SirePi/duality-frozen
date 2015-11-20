﻿using Duality;
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
	[EditorHintImage(ResNames.ImageFXAttractor)]
	[EditorHintCategory(ResNames.CategoryFX)]
	public class ParticleAttractor : ParticleAlterator
	{
		internal override void AlterParticle(Particle inParticle, float inSecondsPast)
		{
			Vector3 direction = this.GameObj.Transform.Pos - inParticle.Position;

			if (direction.Length < Radius)
			{
				float strength = (Radius - direction.Length) / Radius * Strength * inSecondsPast;

				if(AlterParticleDirection)
				{
					float originalAngle = inParticle.Direction;
					float targetAngle = direction.Xy.Angle - MathF.PiOver2;

					inParticle.Direction += MathF.CircularDist(originalAngle, targetAngle) *
						MathF.TurnDir(originalAngle, targetAngle) * strength;
				}
				else
				{
					inParticle.Position += direction * strength;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="inCanvas"></param>
		protected override void _DrawInEditor(global::Duality.Drawing.Canvas inCanvas)
		{
			inCanvas.State.ColorTint = Colors.DarkGreen;
			inCanvas.DrawCircle(this.GameObj.Transform.Pos.X, this.GameObj.Transform.Pos.Y, Radius);
		}
	}
}
