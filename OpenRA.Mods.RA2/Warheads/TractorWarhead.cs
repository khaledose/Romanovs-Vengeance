using OpenRA.GameRules;
using OpenRA.Mods.Common.Warheads;
using OpenRA.Mods.RA2.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Warheads
{
	[Desc("Can this warhead lift the actor that has Tractable trait and move it next to self by force?")]
	public class TractorWarhead : Warhead
	{
		[Desc("Let his be -1, 0, 1, or anything else to modify the traction speed.")]
		public readonly int CruiseSpeedMultiplier = 1;

		public override void DoImpact(in Target target, WarheadArgs args)
		{
			var victim = args.WeaponTarget.Actor;
			var targetTractable = victim.TraitOrDefault<Tractable>();
			if (targetTractable == null)
				return;

			targetTractable.Tract(victim, args.SourceActor, CruiseSpeedMultiplier);
		}
	}
}
