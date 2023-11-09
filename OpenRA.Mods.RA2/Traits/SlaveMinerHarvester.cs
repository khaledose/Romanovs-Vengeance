#region Copyright & License Information
/*
 * Copyright 2015- OpenRA.Mods.AS Developers (see AUTHORS)
 * This file is a part of a third-party plugin for OpenRA, which is
 * free software. It is made available to you under the terms of the
 * GNU General Public License as published by the Free Software
 * Foundation. For more information, see COPYING.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using OpenRA.Mods.AS.Traits;
using OpenRA.Mods.Common.Activities;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.RA2.Activities;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits
{
	public enum MiningState
	{
		Scan,
		Moving,
		TryDeploy,
		Deploying,
		Mining,
		Packaging,
		TryUndeploy,
	}

	public class SpawnerHarvestResourceInfo : BaseSpawnerMasterInfo
	{
		[Desc("Which resources it can harvest. Make sure slaves can mine these too!")]
		public readonly HashSet<string> Resources = new HashSet<string>();
	}

	[Desc("This actor is a harvester that uses its spawns to indirectly harvest resources. i.e., Slave Miner.")]
	public class SlaveMinerHarvesterInfo : SpawnerHarvestResourceInfo, Requires<IOccupySpaceInfo>, Requires<TransformsInfo>
	{
		[VoiceReference]
		public readonly string HarvestVoice = "Action";

		[Desc("Automatically search for resources on creation?")]
		public readonly bool SearchOnCreation = true;

		[Desc("When deployed, use this scan radius.")]
		public readonly int ShortScanRadius = 8;

		[Desc("Look this far when Searching for Ore (in Cells)")]
		public readonly int LongScanRadius = 24;

		[Desc("Look this far when trying to find a deployable position from the target resource patch")]
		public readonly int DeployScanRadius = 8;

		[Desc("If no resource within range at each kick, move.")]
		public readonly int KickScanRadius = 5;

		[Desc("If the SlaveMiner is idle for this long, he'll try to look for ore again at SlaveMinerShortScan range to find ore and wake up (in ticks)")]
		public readonly int KickDelay = 301;

		[Desc("Play this sound when the slave is freed")]
		public readonly string FreeSound = null;

		public override object Create(ActorInitializer init) { return new SlaveMinerHarvester(init, this); }
	}

	public class SlaveMinerHarvester : BaseSpawnerMaster,
		ITick, IIssueOrder, IResolveOrder, IOrderVoice, INotifyTransform
	{
		private const string OrderID = "SlaveMinerHarvest";

		readonly SlaveMinerHarvesterInfo info;
		readonly Actor self;
		readonly IResourceLayer resLayer;
		readonly Mobile mobile;

		// Because activities don't remember states, we remember states here for them.
		public CPos? LastOrderLocation = null;
		public WPos? MasterLastOrderLocation = null;
		public MiningState MiningState = MiningState.Scan;

		public IEnumerable<IOrderTargeter> Orders
		{
			//get { yield break; }
			get { yield return new SlaveMinerHarvestOrderTargeter<SlaveMinerHarvesterInfo>(OrderID); }
		}

		int respawnTicks;
		int kickTicks;

		public SlaveMinerHarvester(ActorInitializer init, SlaveMinerHarvesterInfo info)
			: base(init, info)
		{
			self = init.Self;
			this.info = info;

			mobile = self.Trait<Mobile>();
			resLayer = self.World.WorldActor.Trait<IResourceLayer>();
			kickTicks = info.KickDelay;

			LastOrderLocation = MasterLastOrderLocation.HasValue ? ResolveHarvestLocation(self, MasterLastOrderLocation.Value) : null;
		}

		public override void OnSlaveKilled(Actor self, Actor slave)
		{
			if (respawnTicks <= 0)
				respawnTicks = Info.RespawnTicks;
		}

		void ITick.Tick(Actor self)
		{
			kickTicks--;

			if (self.IsIdle && kickTicks <= 0)
			{
				kickTicks = info.KickDelay;
				self.QueueActivity(new SlaveMinerHarvesterHarvest(self));
			}
		}

		public Order IssueOrder(Actor self, IOrderTargeter order, in Target target, bool queued)
		{
			if (order.OrderID == OrderID)
				return new Order(order.OrderID, self, target, queued);

			return null;
		}

		public void SetMasterTargetPosition(WPos? orderPosition)
		{
			LastOrderLocation = orderPosition.HasValue ? ResolveHarvestLocation(self, orderPosition.Value) : null;
		}

		CPos ResolveHarvestLocation(Actor self, WPos orderPosition)
		{
			if (self.World.Map.CellContaining(orderPosition) == CPos.Zero)
				return self.Location;

			var loc = self.World.Map.CellContaining(orderPosition);

			var territory = self.World.WorldActor.TraitOrDefault<ResourceClaimLayer>();
			if (territory != null)
			{
				// Find the nearest claimable cell to the order location (useful for group-select harvest):
				return mobile.NearestCell(loc, p => mobile.CanEnterCell(p), 1, 6);
			}

			// Find the nearest cell to the order location (useful for group-select harvest):
			return mobile.NearestCell(loc, p => mobile.CanEnterCell(p), 1, 6);
		}

		void HandleSpawnerHarvest(Actor self, Order order)
		{
			// state == Deploying implies order string of SpawnerHarvestDeploying
			// and must not cancel deploy activity!
			//if (MiningState != MiningState.Deploying)
			//{
			//	self.CancelActivity();
			//}

			MiningState = MiningState.Scan;
			self.CancelActivity();
			LastOrderLocation = ResolveHarvestLocation(self, order.Target.CenterPosition);
			self.QueueActivity(new SlaveMinerHarvesterHarvest(self));
			self.ShowTargetLines();

			//// Assign new targets for slaves too.
			foreach (var se in SlaveEntries)
			{
				if (se.IsValid && se.Actor.IsInWorld)
				{
					se.Actor.QueueActivity(new FindAndDeliverResources(se.Actor, LastOrderLocation.Value));
				}
			}

			MasterLastOrderLocation = null;
		}

		public void ResolveOrder(Actor self, Order order)
		{
			if (order.OrderString == OrderID)
				HandleSpawnerHarvest(self, order);
			else if (order.OrderString == "Stop" || order.OrderString == "Move")
			{
				MiningState = MiningState.Scan;
			}
		}

		public string VoicePhraseForOrder(Actor self, Order order)
		{
			return order.OrderString == OrderID ? info.HarvestVoice : null;
		}

		public bool CanHarvestCell(CPos cell)
		{
			// Resources only exist in the ground layer
			if (cell.Layer != 0)
				return false;

			var resType = resLayer.GetResource(cell).Type;
			if (resType == null)
				return false;

			// Can the harvester collect this kind of resource?
			return info.Resources.Contains(resType);
		}

		void INotifyTransform.BeforeTransform(Actor self) { }

		void INotifyTransform.OnTransform(Actor self) { }

		void INotifyTransform.AfterTransform(Actor toActor)
		{
			// When transform complete, assign the slaves to the transform actor
			SlaveMinerMaster refineryMaster = toActor.Trait<SlaveMinerMaster>();
			foreach (var se in SlaveEntries)
			{
				var slave = se.Actor;
				if (slave is null) continue;
				se.SpawnerSlave.LinkMaster(slave, toActor, refineryMaster);
				se.SpawnerSlave.Stop(slave);
				if (!slave.IsDead)
					slave.QueueActivity(new FindAndDeliverResources(slave));
			}

			refineryMaster.SlaveEntries = SlaveEntries;
			SlaveEntries = Array.Empty<BaseSpawnerSlaveEntry>();
		}

		protected override void Killed(Actor self, AttackInfo e)
		{
			base.Killed(self, e);

			if (!string.IsNullOrEmpty(info.FreeSound))
			{
				Game.Sound.Play(SoundType.World, info.FreeSound, self.CenterPosition);
			}
		}
	}

	class SlaveMinerHarvestOrderTargeter<T> : IOrderTargeter where T : SpawnerHarvestResourceInfo
	{
		private readonly string orderID;

		public SlaveMinerHarvestOrderTargeter(string orderID)
		{
			this.orderID = orderID;
		}

		public string OrderID { get { return orderID; } }

		public int OrderPriority { get { return 10; } }

		public bool IsQueued { get; protected set; }

		public bool CanTarget(Actor self, in Target target, ref TargetModifiers modifiers, ref string cursor)
		{
			if (target.Type != TargetType.Terrain)
				return false;

			if (modifiers.HasModifier(TargetModifiers.ForceMove))
				return false;

			var location = self.World.Map.CellContaining(target.CenterPosition);

			// Don't leak info about resources under the shroud
			if (!self.Owner.Shroud.IsExplored(location))
				return false;

			var res = self.World.WorldActor.Trait<IResourceRenderer>().GetRenderedResourceType(location);
			var info = self.Info.TraitInfo<T>();
			if (res == null || !info.Resources.Contains(res))
				return false;

			cursor = "harvest";
			IsQueued = modifiers.HasModifier(TargetModifiers.ForceQueue);

			return true;
		}

		public bool TargetOverridesSelection(Actor self, in Target target, List<Actor> actorsAt, CPos xy, TargetModifiers modifiers)
		{
			return false;
		}
	}
}
