using OpenRA.Activities;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.RA2.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Activities
{
	public class TractorCruise : Activity
	{
		private readonly Tractable tractable;
		private readonly TractableInfo info;
		private readonly Actor tractor;

		private readonly IPositionable positionable;
		private readonly AttackFollow attack;
		private CPos destination;

		public TractorCruise(Actor self, Actor tractor, CPos dest)
		{
			positionable = self.TraitOrDefault<IPositionable>();
			tractable = self.TraitOrDefault<Tractable>();
			attack = tractor.TraitOrDefault<AttackFollow>();
			this.tractor = tractor;
			info = tractable.Info;
			destination = dest;
		}

		protected override void OnLastRun(Actor self)
		{
			if (NextActivity is null)
			{
				Queue(new TractorFall(tractable, tractor));
			}

			if (!IsTractorStoppedAttacking(self))
			{
				attack.ClearRequestedTarget();
			}
		}

		private bool IsTractorStoppedAttacking(Actor self) => tractor.IsDead || attack.RequestedTarget == Target.Invalid || attack.RequestedTarget.Actor != self;

		public override bool Tick(Actor self)
		{
			if(IsTractorStoppedAttacking(self))
			{
				Queue(new TractorFall(tractable, tractor));
				return true;
			}

			var step = self.World.Map.CenterOfCell(destination) - self.CenterPosition;
			if (step.HorizontalLength <= 256)
			{
				return true;
			}

			step = new WVec(step.X, step.Y, 0);
			step = info.CruiseSpeed.Length * step / step.Length;

			var altitude = self.World.Map.DistanceAboveTerrain(self.CenterPosition);
			var altitudeDelta = new WVec(0, 0, tractable.CalcAltitudeDelta(self, altitude, info.CruiseAltitude));

			positionable.SetCenterPosition(self, self.CenterPosition + step + altitudeDelta);
			return false;
		}
	}
}
