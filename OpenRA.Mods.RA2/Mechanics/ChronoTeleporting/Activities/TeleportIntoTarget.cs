using OpenRA.Activities;
using OpenRA.Mods.RA2.Mechanics.ChronoTeleporting.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Mechanics.ChronoTeleporting.Activities
{
	public class TeleportIntoTarget : Activity
	{
		private readonly ChronoMobile mobile;
		private Target target;

		public TeleportIntoTarget(Actor self, in Target target)
		{
			this.target = target;
			mobile = self.TraitOrDefault<ChronoMobile>();
		}

		protected override void OnFirstRun(Actor self)
		{
			var targetPos = target.Positions.ClosestToIgnoringPath(self.CenterPosition);
			mobile.SetCenterPosition(self, targetPos);
		}
	}
}
