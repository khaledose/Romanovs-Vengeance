using OpenRA.Activities;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.RA2.Traits;
using OpenRA.Traits;
using static OpenRA.Mods.Common.Traits.AttackFollow;

namespace OpenRA.Mods.RA2.Activities
{
	public class TractorLift : Activity
	{
		private readonly Tractable tractable;
		private readonly TractableInfo info;
		private readonly AttackFollow attack;
		private readonly Actor tractor;
		private readonly Mobile mobile;

		public TractorLift(Actor self, Actor tractor)
		{
			tractable = self.TraitOrDefault<Tractable>();
			mobile = tractor.TraitOrDefault<Mobile>();
			attack = tractor.TraitOrDefault<AttackFollow>();
			this.tractor = tractor;
			info = tractable.Info;
		}

		protected override void OnLastRun(Actor self)
		{
			if (NextActivity is null)
			{
				var dest = mobile.NearestCell(tractor.Location, p => mobile.CanEnterCell(p), 2, 6);
				Queue(new TractorCruise(self, tractor, dest));
			}
		}

		private bool IsTractorStoppedAttacking(Actor self) => tractor.IsDead || attack.RequestedTarget == Target.Invalid || attack.RequestedTarget.Actor != self;

		public override bool Tick(Actor self)
		{
			if (IsTractorStoppedAttacking(self))
			{
				Queue(new TractorFall(tractable, tractor));
				return true;
			}

			var altitude = self.World.Map.DistanceAboveTerrain(self.CenterPosition);

			if (altitude < info.CruiseAltitude)
			{
				int dz = tractable.CalcAltitudeDelta(self, altitude, info.CruiseAltitude);
				tractable.SetPosition(self, self.CenterPosition + new WVec(0, 0, dz));
			}

			return altitude >= info.CruiseAltitude;
		}
	}
}
