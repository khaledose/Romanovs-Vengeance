using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Mechanics.CargoAdditions.Traits
{
	public class ChangeOwnerOnGarrisonInfo : TraitInfo, Requires<CargoInfo>
	{
		public override object Create(ActorInitializer init) { return new ChangeOwnerOnGarrison(init.Self, this); }
	}

	public class ChangeOwnerOnGarrison : TraitInfo<ChangeOwnerOnGarrisonInfo>, INotifyPassengerEntered, INotifyPassengerExited
	{
		readonly Player originalOwner;
		readonly Cargo cargo;

		public ChangeOwnerOnGarrison(Actor self, ChangeOwnerOnGarrisonInfo info)
		{
			originalOwner = self.Owner;
			cargo = self.Trait<Cargo>();
		}

		void INotifyPassengerEntered.OnPassengerEntered(Actor self, Actor garrisoner)
		{
			var newOwner = garrisoner.Owner;
			if (self.Owner != originalOwner || self.Owner == newOwner || self.Owner.IsAlliedWith(garrisoner.Owner))
				return;

			self.ChangeOwner(newOwner);
		}

		void INotifyPassengerExited.OnPassengerExited(Actor self, Actor garrisoner)
		{
			if (cargo.PassengerCount > 0)
				return;

			self.ChangeOwner(originalOwner);
		}
	}
}
