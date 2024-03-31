using System.Collections.Generic;
using System.Linq;
using OpenRA.Activities;
using OpenRA.Mods.RA2.Mechanics.ChronoTeleporting.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Mechanics.ChronoTeleporting.Activities
{
	public class TeleportWithinRange : Activity
	{
		private readonly Actor self;
		private readonly ChronoMobile mobile;
		private readonly Map map;
		private Target target;
		private CPos toCell = CPos.Zero;
		private CPos targetCell;
		private readonly Color? targetLineColor;
		private readonly WDist maxRange;
		private readonly WDist minRange;
		private readonly int maxCells;
		private readonly int minCells;

		public TeleportWithinRange(Actor self, in Target target, WDist minRange, WDist maxRange, Color? targetLineColor = null)
		{
			this.self = self;
			mobile = self.TraitOrDefault<ChronoMobile>();
			map = self.World.Map;
			this.target = target;
			this.targetLineColor = targetLineColor;
			this.minRange = minRange;
			this.maxRange = maxRange;
			maxCells = (maxRange.Length + 1023) / 1024;
			minCells = minRange.Length / 1024;
			targetCell = self.World.Map.CellContaining(target.CenterPosition);
		}

		protected override void OnFirstRun(Actor self)
		{
			toCell = CalculatePathToTarget();

			QueueChild(mobile.MoveTo(toCell, evaluateNearestMovableCell: true, targetLineColor: targetLineColor));
		}

		private CPos CalculatePathToTarget()
		{
			var validCells = map.FindTilesInAnnulus(targetCell, minCells, maxCells)
				.Where(cell =>
					mobile.CanStayInCell(cell) &&
					mobile.CanEnterCell(cell) &&
					AtCorrectRange(map.CenterOfSubCell(cell, mobile.FromSubCell))
				)
				.ToList();

			return FindNearestCell(validCells, self.World.Map.CellContaining(self.CenterPosition));
		}

		private bool AtCorrectRange(WPos origin)
		{
			return target.IsInRange(origin, maxRange) && !target.IsInRange(origin, minRange);
		}

		private CPos FindNearestCell(List<CPos> positions, CPos target)
		{
			if (positions == null || positions.Count == 0)
			{
				return CPos.Zero;
			}

			var nearest = positions.First();
			var minDistance = mobile.CalculateDistance(nearest, target);

			foreach (var pos in positions)
			{
				var distance = mobile.CalculateDistance(pos, target);
				if (distance < minDistance)
				{
					minDistance = distance;
					nearest = pos;
				}
			}

			return nearest;
		}
	}
}
