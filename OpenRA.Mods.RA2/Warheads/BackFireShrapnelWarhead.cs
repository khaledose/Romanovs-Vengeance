#region Copyright & License Information
/*
 * Copyright 2015- OpenRA.Mods.AS Developers (see AUTHORS)
 * This file is a part of a third-party plugin for OpenRA, which is
 * free software. It is made available to you under the terms of the
 * GNU General Public License as published by the Free Software
 * Foundation. For more information, see COPYING.
 */
#endregion

using System;
using System.Linq;
using OpenRA.GameRules;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Warheads
{
	[Desc("A creative warhead in MW, made by CombinE88")]
	public class BackFireShrapnelWarhead : WarheadAS, IRulesetLoaded<WeaponInfo>
	{
		[WeaponReference]
		[FieldLoader.Require]
		[Desc("Has to be defined in weapons.yaml as well.")]
		public readonly string Weapon = null;

		WeaponInfo weapon;

		public void RulesetLoaded(Ruleset rules, WeaponInfo info)
		{
			if (!rules.Weapons.TryGetValue(Weapon.ToLowerInvariant(), out weapon))
			throw new YamlException("Weapons Ruleset does not contain an entry '{0}'".F(Weapon.ToLowerInvariant()));
		}

		public override void DoImpact(in Target target, WarheadArgs args)
		{
			Actor firedBy = args.SourceActor;

			if (!IsValidImpact(target.CenterPosition, firedBy))
				return;

			Target shrapnelTarget = Target.Invalid;

			shrapnelTarget = Target.FromActor(firedBy);

			if (shrapnelTarget.Type != TargetType.Invalid)
			{
				var pargs = new ProjectileArgs
				{
					Weapon = weapon,
					Facing = (shrapnelTarget.CenterPosition - target.CenterPosition).Yaw,

					DamageModifiers = !firedBy.IsDead
						? firedBy.TraitsImplementing<IFirepowerModifier>()
							.Select(a => a.GetFirepowerModifier()).ToArray()
						: Array.Empty<int>(),

					InaccuracyModifiers = !firedBy.IsDead
						? firedBy.TraitsImplementing<IInaccuracyModifier>()
							.Select(a => a.GetInaccuracyModifier()).ToArray()
						: Array.Empty<int>(),

					RangeModifiers = !firedBy.IsDead
						? firedBy.TraitsImplementing<IRangeModifier>()
							.Select(a => a.GetRangeModifier()).ToArray()
						: Array.Empty<int>(),

					Source = target.CenterPosition,
					SourceActor = firedBy,
					GuidedTarget = shrapnelTarget,
					PassiveTarget = shrapnelTarget.CenterPosition
				};

				if (pargs.Weapon.Projectile != null)
				{
					var projectile = pargs.Weapon.Projectile.Create(pargs);
					if (projectile != null)
							firedBy.World.AddFrameEndTask(w => w.Add(projectile));

					if (pargs.Weapon.Report != null && pargs.Weapon.Report.Any())
					{
						var pos = target.CenterPosition;
						if (pargs.Weapon.AudibleThroughFog || (!firedBy.World.ShroudObscures(pos) && !firedBy.World.FogObscures(pos)))
							Game.Sound.Play(SoundType.World, pargs.Weapon.Report, firedBy.World, pos, null, pargs.Weapon.SoundVolume);
					}
				}
			}
		}
	}
}
