using OpenRA.Activities;
using OpenRA.Mods.Common.Activities;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.RA2.Traits;

namespace OpenRA.Mods.RA2.Activities
{
	public class TeleportBackToCell : Activity
	{
		private readonly ChronoMobile mobile;
		private readonly Parachutable parachutable;
		private CPos cell;
		private WPos pos;

		public TeleportBackToCell(Actor self)
		{
			mobile = self.Trait<ChronoMobile>();
			parachutable = self.Trait<Parachutable>();
			IsInterruptible = false;
		}

		protected override void OnFirstRun(Actor self)
		{
			pos = self.CenterPosition;
			if (parachutable != null && self.World.Map.DistanceAboveTerrain(pos) > WDist.Zero)
				QueueChild(new Parachute(self));

			QueueChild(mobile.MoveTo(cell, evaluateNearestMovableCell: true));
		}
	}
}
