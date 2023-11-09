#region Copyright & License Information
/*
 * Copyright 2015- OpenRA.Mods.AS Developers (see AUTHORS)
 * This file is a part of a third-party plugin for OpenRA, which is
 * free software. It is made available to you under the terms of the
 * GNU General Public License as published by the Free Software
 * Foundation. For more information, see COPYING.
 */
#endregion

using System.Collections.Generic;
using OpenRA.Activities;
using OpenRA.Mods.RA2.Traits;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Activities
{
	public class SlaveMinerMasterHarvest : Activity
	{
		readonly SlaveMinerMaster harv;
		readonly SlaveMinerMasterInfo harvInfo;
		readonly ResourceClaimLayer claimLayer;
		private Transforms transforms;

		public SlaveMinerMasterHarvest(Actor self)
		{
			harv = self.Trait<SlaveMinerMaster>();
			harvInfo = self.Info.TraitInfo<SlaveMinerMasterInfo>();
			claimLayer = self.World.WorldActor.TraitOrDefault<ResourceClaimLayer>();
			transforms = self.Trait<Transforms>();
			ChildHasPriority = false;
		}

		Activity Mining(out MiningState state)
		{
			// Let the harvester become idle so it can shoot enemies.
			// Tick in SpawnerHarvester trait will kick activity back to KickTick.
			state = MiningState.Mining;
			return ChildActivity;
		}

		Activity Kick(Actor self, out MiningState state)
		{
			var closestHarvestablePosition = ClosestHarvestablePos(self, harvInfo.KickScanRadius);
			if (closestHarvestablePosition is not null)
			{
				// I may stay mining.
				state = MiningState.Mining;
				return ChildActivity;
			}

			IsInterruptible = false;
			state = MiningState.TryUndeploy;
			transforms.DeployTransform(false);

			return this;
		}

		public override bool Tick(Actor self)
		{
			/*
			 We just need to confirm one thing: when the nearest resource is finished,
			 just find the next resource point and transform and move to that location
			 */

			if (IsCanceling)
				return true;

			// Erm... looking at this, I could split these into separte activites...
			// I prefer finite state machine style though...
			// I can see what is going on at high level in this single place -_-
			// I think this is less horrible than OpenRA FindResources... stuff.
			// We are losing one tick, but so what?
			// If this loss isn't acceptable, call ATick() from BTick() or something.
			switch (harv.MiningState)
			{
				case MiningState.Mining:
					QueueChild(Mining(out harv.MiningState));
					return false;
				case MiningState.Scan:
					QueueChild(Kick(self, out harv.MiningState));
					return false;
			}

			return true;
		}

		/// <summary>
		/// Using LastOrderLocation and self.Location as starting points,
		/// perform A* search to find the nearest accessible and harvestable cell.
		/// </summary>
		CPos? ClosestHarvestablePos(Actor self, int searchRadius)
		{
			if (harv.CanHarvestCell(self.Location) && claimLayer.CanClaimCell(self, self.Location))
			{
				return self.Location;
			}

			// Determine where to search from and how far to search:
			var searchFromLoc = self.Location;

			var cells = self.World.Map.FindTilesInCircle(searchFromLoc, searchRadius);

			foreach (var cell in cells)
			{
				if (harv.CanHarvestCell(cell) && claimLayer.CanClaimCell(self, cell))
				{
					return cell;
				}
			}

			return null;
		}

		public override IEnumerable<Target> GetTargets(Actor self)
		{
			yield return Target.FromCell(self.World, self.Location);
		}
	}
}
