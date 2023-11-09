using OpenRA.Traits;
using System.Collections.Generic;
using System.Linq;

namespace OpenRA.Mods.RA2.Traits.Render
{
	public class WithInfantryBodyInfo : Common.Traits.Render.WithInfantryBodyInfo
	{
		[SequenceReference(dictionaryReference: LintDictionaryReference.Keys)]
		public readonly Dictionary<string, string[]> SequenceSounds = null;

		[SequenceReference(dictionaryReference: LintDictionaryReference.Keys)]
		public readonly Dictionary<string, float?> SequenceVolume = null;

		[Desc("Do the sounds play under shroud or fog.")]
		public readonly bool AudibleThroughFog = false;

		public override object Create(ActorInitializer init) => new WithInfantryBody(init, this);
	}

	public class WithInfantryBody : Common.Traits.Render.WithInfantryBody
	{
		WithInfantryBodyInfo Info;
		ISound MovingSound;
		ISound IdleSound;

		public WithInfantryBody(ActorInitializer init, WithInfantryBodyInfo info)
			: base(init, info)
		{
			Info = info;
		}

		protected override void Tick(Actor self)
		{
			base.Tick(self);
			var world = self.World;

			if (state != AnimationState.Moving && (MovingSound is not null && !MovingSound.Complete))
			{
				Game.Sound.StopSound(MovingSound);
			}

			if (!Info.AudibleThroughFog && (world.ShroudObscures(self.CenterPosition) || world.FogObscures(self.CenterPosition)))
			{
				return;
			}

			var currentSequence = DefaultAnimation.CurrentSequence.Name;
			var sound = Info.SequenceSounds?.GetValueOrDefault(currentSequence);
			if (sound is null || !sound.Any())
			{
				return;
			}

			var volume = Info.SequenceVolume?.GetValueOrDefault(currentSequence) ?? 1f;

			if (state == AnimationState.IdleAnimating && (IdleSound is null || IdleSound.Complete))
			{
				IdleSound = Game.Sound.Play(SoundType.World, sound.Random(world.LocalRandom), self.CenterPosition, volume);
			}

			if (state == AnimationState.Moving && (MovingSound is null || MovingSound.Complete))
			{
				MovingSound = Game.Sound.PlayLooped(SoundType.World, sound.Random(world.LocalRandom), self.CenterPosition, volume);
			}
		}
	}
}
