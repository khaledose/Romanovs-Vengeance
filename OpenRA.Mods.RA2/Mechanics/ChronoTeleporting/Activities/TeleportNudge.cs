using System.Collections.Generic;
using OpenRA.Activities;
using OpenRA.Mods.RA2.Mechanics.ChronoTeleporting.Traits;

namespace OpenRA.Mods.RA2.Mechanics.ChronoTeleporting.Activities
{
	public class TeleportNudge : Activity
	{
		private readonly Actor self;
		private readonly ChronoMobile mobile;

		public TeleportNudge(Actor self)
		{
			this.self = self;
			mobile = self.TraitOrDefault<ChronoMobile>();
		}

		protected override void OnFirstRun(Actor self)
		{
			if (!mobile.CanTeleport)
				return;

			var cell = mobile.GetAdjacentCell(self.Location);
			if (cell != null)
				QueueChild(mobile.MoveTo(cell.Value, evaluateNearestMovableCell: true));
		}

		public override IEnumerable<TargetLineNode> TargetLineNodes(Actor self)
		{
			if (ChildActivity != null)
				foreach (var n in ChildActivity.TargetLineNodes(self))
					yield return n;

			yield break;
		}
	}
}
