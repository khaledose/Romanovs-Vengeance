using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;
using System.Linq;

namespace OpenRA.Mods.RA2.Traits.Sound
{
	public class WithTurretRotateSoundInfo : ConditionalTraitInfo, Requires<TurretedInfo>
	{
		[FieldLoader.Require]
		public readonly string[] RotatingSounds = null;

		[Desc("Does the sound play under shroud or fog.")]
		public readonly bool AudibleThroughFog = false;

		[Desc("Volume the EnterSound and ExitSound played at.")]
		public readonly float SoundVolume = 1f;

		public override object Create(ActorInitializer init) { return new WithTurretRotateSound(init.Self, this); }
	}

	public class WithTurretRotateSound : ConditionalTrait<WithTurretRotateSoundInfo>, INotifyAiming
	{
		private WithTurretRotateSoundInfo Info;
		private ISound sound;

		public WithTurretRotateSound(Actor self, WithTurretRotateSoundInfo info)
			: base(info)
		{
			Info = info;
		}

		public void StartedAiming(Actor self, AttackBase attack)
		{
			if (sound is not null && !sound.Complete)
			{
				return;
			}

			var pos = self.CenterPosition;
			if (Info.AudibleThroughFog || (!self.World.ShroudObscures(pos) && !self.World.FogObscures(pos)))
			{
				sound = Game.Sound.Play(SoundType.World, Info.RotatingSounds.RandomOrDefault(self.World.LocalRandom), self.CenterPosition, Info.SoundVolume);
			}
		}

		public void StoppedAiming(Actor self, AttackBase attack)
		{
			if (sound is not null)
			{
				Game.Sound.StopSound(sound);
			}
		}
	}
}
