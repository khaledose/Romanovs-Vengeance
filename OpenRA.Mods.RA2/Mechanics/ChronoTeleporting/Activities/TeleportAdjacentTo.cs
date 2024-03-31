using System.Linq;
using OpenRA;
using OpenRA.Activities;
using OpenRA.Mods.Common;
using OpenRA.Mods.RA2.Mechanics.ChronoTeleporting.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Mechanics.ChronoTeleporting.Activities
{
	public class TeleportAdjacentTo : Activity
	{
		private readonly ChronoMobile mobile;
		private Target target;

		public TeleportAdjacentTo(Actor self, in Target target)
		{
			this.target = target;
			mobile = self.TraitOrDefault<ChronoMobile>();
		}

		protected override void OnFirstRun(Actor self)
		{
			var cell = Util.AdjacentCells(self.World, target)
							.Where(cell => mobile.CanStayInCell(cell) && mobile.CanEnterCell(cell))
							.FirstOrDefault();

			if (cell != CPos.Zero)
			{
				QueueChild(mobile.MoveTo(cell));
			}
		}
	}
}
