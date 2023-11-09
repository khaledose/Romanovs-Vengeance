using OpenRA.Activities;
using OpenRA.Mods.Common;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.RA2.Traits;
using OpenRA.Traits;
using System.Linq;

namespace OpenRA.Mods.RA2.Activities.Move
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
