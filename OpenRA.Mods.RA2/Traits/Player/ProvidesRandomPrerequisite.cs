using OpenRA.Mods.Common.Traits;
using System.Collections.Generic;
using System.Linq;

namespace OpenRA.Mods.RA2.Traits
{
	public class ProvidesRandomPrerequisiteInfo : ConditionalTraitInfo
	{
		public override object Create(ActorInitializer init)
		{
			return new ProvidesRandomPrerequisite(init.Self, this);
		}
	}

	public class ProvidesRandomPrerequisite : ConditionalTrait<ProvidesRandomPrerequisiteInfo>, ITechTreePrerequisite
	{
		readonly string[] prerequisites;

		bool enabled;
		TechTree techTree;
		string faction;

		public ProvidesRandomPrerequisite(Actor self, ProvidesRandomPrerequisiteInfo info)
			: base(info)
		{
			var actors = self.World.ActorsHavingTrait<Buildable>().Select(a => a.Info.TraitInfo<BuildableInfo>());
			prerequisites = actors.Where(a => a.BuildAtProductionType == "Soldier").Select(a => a.Prerequisites).RandomOrDefault(self.World.LocalRandom).ToArray();
		}

		public IEnumerable<string> ProvidesPrerequisites => prerequisites;
	}
}
