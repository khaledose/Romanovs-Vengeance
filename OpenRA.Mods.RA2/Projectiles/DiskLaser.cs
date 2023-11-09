#region Copyright & License Information
/*
 * Copyright (c) The OpenRA Developers and Contributors
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System.Collections.Generic;
using OpenRA.GameRules;
using OpenRA.Graphics;
using OpenRA.Mods.Common;
using OpenRA.Mods.Common.Effects;
using OpenRA.Mods.RA2.Graphics;
using OpenRA.Mods.Common.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;
using Util = OpenRA.Mods.Common.Util;
using System;

namespace OpenRA.Mods.RA2.Projectiles
{
	[Desc("Not a sprite, but an engine effect.")]
	public class DiskLaserInfo : IProjectileInfo
	{
		[Desc("The width of the zap.")]
		public readonly WDist Width = new(3);

		[Desc("Color of the beam.")]
		public readonly Color Color = Color.Red;

		[Desc("Overrides the color of the beam to player's color.")]
		public readonly bool UsePlayerColor = false;

		[Desc("Draw a second beam (for 'glow' effect).")]
		public readonly bool GlowBeam = true;

		[Desc("The width of the zap.")]
		public readonly WDist GlowBeamWidth = new(1);

		[Desc("Color of the secondary beam.")]
		public readonly Color GlowBeamColor = Color.White;

		[Desc("Overrides the color of the beam to player's color.")]
		public readonly bool GlowBeamUsePlayerColor = false;

		[Desc("The maximum duration (in ticks) of the beam's existence.")]
		public readonly int AttackDuration = 10;

		[Desc("Allows the beam to charge first before launching.")]
		public readonly bool UseChargeRing = true;

		[Desc("The radius of the beam's charging ring.")]
		public readonly WDist ChargeRingRadius = new(1024);

		[Desc("The maximum duration (in ticks) of the beam's charging ring existence.")]
		public readonly int ChargeRingDuration = 10;

		[Desc("Total time-frame in ticks that the beam deals damage every DamageInterval.")]
		public readonly int DamageDuration = 1;

		[Desc("The number of ticks between the beam causing warhead impacts in its area of effect.")]
		public readonly int DamageInterval = 1;

		[Desc("Beam follows the target.")]
		public readonly bool TrackTarget = false;

		[Desc("The maximum/constant/incremental inaccuracy used in conjunction with the InaccuracyType property.")]
		public readonly WDist Inaccuracy = WDist.Zero;

		[Desc("Controls the way inaccuracy is calculated. Possible values are 'Maximum' - scale from 0 to max with range, 'PerCellIncrement' - scale from 0 with range and 'Absolute' - use set value regardless of range.")]
		public readonly InaccuracyType InaccuracyType = InaccuracyType.Maximum;

		[Desc("Beam can be blocked.")]
		public readonly bool Blockable = false;

		[Desc("Impact animation.")]
		public readonly string HitAnim = null;

		[SequenceReference(nameof(HitAnim), allowNullImage: true)]
		[Desc("Sequence of impact animation to use.")]
		public readonly string HitAnimSequence = "idle";

		[PaletteReference]
		public readonly string HitAnimPalette = "effect";

		[Desc("Image containing launch effect sequence.")]
		public readonly string LaunchEffectImage = null;

		[SequenceReference(nameof(LaunchEffectImage), allowNullImage: true)]
		[Desc("Launch effect sequence to play.")]
		public readonly string LaunchEffectSequence = null;

		[PaletteReference]
		[Desc("Palette to use for launch effect.")]
		public readonly string LaunchEffectPalette = "effect";

		public IProjectile Create(ProjectileArgs args)
		{
			return new DiskLaser(this, args);
		}
	}

	public class DiskLaser : IProjectile, ISync
	{
		readonly ProjectileArgs args;
		readonly DiskLaserInfo info;
		readonly Animation hitanim;
		readonly Color color;
		readonly Color glowColor;
		readonly bool hasLaunchEffect;
		int duration;
		int chargeDuration;
		int attackDuration;
		int ticks;
		int interval;
		bool showHitAnim;

		[Sync]
		WPos target;

		[Sync]
		WPos source;

		public DiskLaser(DiskLaserInfo info, ProjectileArgs args)
		{
			this.args = args;
			this.info = info;
			color = info.UsePlayerColor ? args.SourceActor.Owner.Color : info.Color;
			glowColor = info.GlowBeamUsePlayerColor ? args.SourceActor.Owner.Color : info.GlowBeamColor;
			target = args.PassiveTarget;
			source = args.Source;

			chargeDuration = info.UseChargeRing ? info.ChargeRingDuration : 0;
			attackDuration = info.AttackDuration;
			duration = chargeDuration + attackDuration;

			if (info.Inaccuracy.Length > 0)
			{
				var maxInaccuracyOffset = Util.GetProjectileInaccuracy(info.Inaccuracy.Length, info.InaccuracyType, args);
				target += WVec.FromPDF(args.SourceActor.World.SharedRandom, 2) * maxInaccuracyOffset / 1024;
			}

			if (!string.IsNullOrEmpty(info.HitAnim))
			{
				hitanim = new Animation(args.SourceActor.World, info.HitAnim);
				showHitAnim = true;
			}

			hasLaunchEffect = !string.IsNullOrEmpty(info.LaunchEffectImage) && !string.IsNullOrEmpty(info.LaunchEffectSequence);
		}

		public void Tick(World world)
		{
			source = args.CurrentSource();

			if (hasLaunchEffect && ticks == 0)
				world.AddFrameEndTask(w => w.Add(new SpriteEffect(args.CurrentSource, args.CurrentMuzzleFacing, world,
					info.LaunchEffectImage, info.LaunchEffectSequence, info.LaunchEffectPalette)));

			// Beam tracks target
			if (info.TrackTarget && args.GuidedTarget.IsValidFor(args.SourceActor))
				target = args.Weapon.TargetActorCenter ? args.GuidedTarget.CenterPosition : args.GuidedTarget.Positions.ClosestToIgnoringPath(source);

			// Check for blocking actors
			if (info.Blockable && BlocksProjectiles.AnyBlockingActorsBetween(world, args.SourceActor.Owner, source, target, info.Width, out var blockedPos))
			{
				target = blockedPos;
			}

			if (ticks >= chargeDuration && ticks - chargeDuration < info.DamageDuration && --interval <= 0)
			{
				var warheadArgs = new WarheadArgs(args)
				{
					ImpactOrientation = new WRot(WAngle.Zero, Util.GetVerticalAngle(source, target), args.CurrentMuzzleFacing()),
					ImpactPosition = target,
				};

				args.Weapon.Impact(Target.FromPos(target), warheadArgs);
				interval = info.DamageInterval;
			}

			if (showHitAnim)
			{
				if (ticks == 0)
					hitanim.PlayThen(info.HitAnimSequence, () => showHitAnim = false);

				hitanim.Tick();
			}

			if (++ticks >= duration && !showHitAnim)
				world.AddFrameEndTask(w => w.Remove(this));
		}

		private ChargingRingRenderable GetBeam(WPos position)
		{
			double take = info.UseChargeRing ? (double)ticks / chargeDuration : 2;
			double skip = info.UseChargeRing ? 0.667 * take - 0.334 : 2;
			var rc = color;

			if (ticks >= chargeDuration && ticks <= duration)
			{
				rc = Color.FromArgb((int)((1 - ((double)(ticks - chargeDuration) / attackDuration)) * color.A), color);
			}

			return new ChargingRingRenderable(position, target, info.ChargeRingRadius, info.Width, rc, take, Math.Max(skip, 0));
		}

		private ChargingRingRenderable GetGlow(WPos position)
		{
			double take = info.UseChargeRing ? (double)ticks / chargeDuration : 2;
			double skip = info.UseChargeRing ? 0.667 * take - 0.334 : 2;
			var rc = glowColor;

			if (ticks >= chargeDuration && ticks <= duration)
			{
				rc = Color.FromArgb((int)((1 - ((double)(ticks - chargeDuration) / attackDuration)) * glowColor.A), glowColor);
			}

			return new ChargingRingRenderable(position, target, info.ChargeRingRadius, info.GlowBeamWidth, rc, take, Math.Max(skip, 0));
		}

		public IEnumerable<IRenderable> Render(WorldRenderer wr)
		{
			if (wr.World.FogObscures(target) &&
				wr.World.FogObscures(source))
				yield break;

			var position = args.SourceActor.CenterPosition - new WVec(new WDist(20), new WDist(510), new WDist(50));

			if (ticks <= duration)
				yield return GetBeam(position);

			if (info.GlowBeam && ticks <= duration)
				yield return GetGlow(position);

			if (showHitAnim)
				foreach (var r in hitanim.Render(target, wr.Palette(info.HitAnimPalette)))
					yield return r;
		}
	}
}
