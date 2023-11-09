using OpenRA.Mods.AS.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits
{
	public class ChangeOwnerOnGarrisonInfo : TraitInfo, Requires<GarrisonableInfo>
	{
		public override object Create(ActorInitializer init) { return new ChangeOwnerOnGarrison(init.Self, this); }
	}

	public class ChangeOwnerOnGarrison : TraitInfo<ChangeOwnerOnGarrisonInfo>, INotifyGarrisonerEntered, INotifyGarrisonerExited
	{
		Player originalOwner;
		readonly Garrisonable garrison;

		public ChangeOwnerOnGarrison(Actor self, ChangeOwnerOnGarrisonInfo info)
		{
			originalOwner = self.Owner;
			garrison = self.Trait<Garrisonable>();
		}

		void INotifyGarrisonerEntered.OnGarrisonerEntered(Actor self, Actor garrisoner)
		{
			var newOwner = garrisoner.Owner;
			if (self.Owner != originalOwner || self.Owner == newOwner || self.Owner.IsAlliedWith(garrisoner.Owner))
				return;

			self.ChangeOwner(newOwner);
		}

		void INotifyGarrisonerExited.OnGarrisonerExited(Actor self, Actor garrisoner)
		{
			if (garrison.GarrisonerCount > 0)
				return;

			self.ChangeOwner(originalOwner);
		}
	}
}
