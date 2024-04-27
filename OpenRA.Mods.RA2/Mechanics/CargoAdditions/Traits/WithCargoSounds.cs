using System;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Mechanics.CargoAdditions.Traits
{
	public class WithCargoSoundsInfo : ConditionalTraitInfo, Requires<CargoInfo>
	{
		[Desc("Speech notification played when an actor enters this cargo.")]
		public readonly string EnterNotification = null;

		[Desc("Speech notification played when an actor leaves this cargo.")]
		public readonly string ExitNotification = null;

		[Desc("List of sounds to be randomly played when an actor enters this cargo.")]
		public readonly string[] EnterSounds = Array.Empty<string>();

		[Desc("List of sounds to be randomly played when an actor exits this cargo.")]
		public readonly string[] ExitSounds = Array.Empty<string>();

		[Desc("Does the sound play under shroud or fog.")]
		public readonly bool AudibleThroughFog = false;

		[Desc("Volume the EnterSounds and ExitSounds played at.")]
		public readonly float SoundVolume = 1f;

		public readonly bool PlayOnce = false;

		public override object Create(ActorInitializer init) { return new WithCargoSounds(init.Self, this); }
	}

	public class WithCargoSounds : ConditionalTrait<WithCargoSoundsInfo>, INotifyPassengerEntered, INotifyPassengerExited
	{
		readonly Cargo cargo;
		bool played;

		public WithCargoSounds(Actor self, WithCargoSoundsInfo info)
			: base(info)
		{
			cargo = self.TraitOrDefault<Cargo>();
		}

		void INotifyPassengerEntered.OnPassengerEntered(Actor self, Actor passenger)
		{
			if (played)
				return;

			PlaySound(self, Info.EnterSounds);
			Game.Sound.PlayNotification(self.World.Map.Rules, passenger.Owner, "Speech", Info.EnterNotification, passenger.Owner.Faction.InternalName);
			played = Info.PlayOnce;
		}

		void INotifyPassengerExited.OnPassengerExited(Actor self, Actor passenger)
		{
			if (Info.PlayOnce && cargo.PassengerCount > 0)
				return;

			PlaySound(self, Info.ExitSounds);
			Game.Sound.PlayNotification(self.World.Map.Rules, passenger.Owner, "Speech", Info.ExitNotification, passenger.Owner.Faction.InternalName);
			played = cargo.PassengerCount > 0;
		}

		void PlaySound(Actor self, string[] sounds)
		{
			if (sounds.Length == 0)
				return;

			var pos = self.CenterPosition;

			if (!Info.AudibleThroughFog && (self.World.ShroudObscures(pos) || self.World.FogObscures(pos)))
				return;

			Game.Sound.Play(SoundType.World, sounds, self.World, pos, null, Info.SoundVolume);
		}
	}
}
