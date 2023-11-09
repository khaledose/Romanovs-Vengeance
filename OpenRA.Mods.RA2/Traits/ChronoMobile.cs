using OpenRA.Activities;
using OpenRA.Mods.Common;
using OpenRA.Mods.Common.Activities;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.RA2.Activities;
using OpenRA.Mods.RA2.Activities.Move;
using OpenRA.Primitives;
using OpenRA.Traits;
using System;
using System.Linq;

namespace OpenRA.Mods.RA2.Traits
{
	public class ChronoMobileInfo : MobileInfo
	{
		[Desc("Cooldown in ticks until the unit can teleport.")]
		public readonly int ChargeDelay = 500;

		[Desc("Sound to play when teleporting.")]
		public readonly string ChronoshiftSound = "chrotnk1.aud";

		[GrantedConditionReference]
		[Desc("The condition to grant after teleporting.")]
		public readonly string TeleportCondition = null;

		[Desc("Show effect while teleporting.")]
		public readonly bool HasTeleportEffect = true;

		[Desc("Effect shown while teleporting.")]
		public readonly string TeleportEffect = "effect50alpha";

		[Desc("Sequence of the effect shown while teleporting.")]
		public readonly string TeleportEffectSequence = "temporal_explosion";

		public override object Create(ActorInitializer init) { return new ChronoMobile(init, this); }
	}

	public class ChronoMobile : Mobile, IResolveOrder, IMove, ITick, ICreationActivity, ISelectionBar, INotifyBlockingMove
	{
		public ChronoMobileInfo Info { get; }

		public bool CanTeleport => !IsTraitDisabled && !IsTraitPaused && chargeTick <= 0;

		[Sync]
		private int chargeTick = 0;
		private Actor Self;
		private int token = Actor.InvalidConditionToken;
		readonly CPos[] creationRallypoint;

		public ChronoMobile(ActorInitializer init, ChronoMobileInfo info)
			: base(init, info)
		{
			Self = init.Self;
			Info = info;
			creationRallypoint = init.GetOrDefault<RallyPointInit>()?.Value;
		}

		void ITick.Tick(Actor self)
		{
			if (--chargeTick < 0 && token != Actor.InvalidConditionToken)
			{
				token = self.RevokeCondition(token);
			}
		}

		void IResolveOrder.ResolveOrder(Actor self, Order order)
		{
			if (IsTraitDisabled)
				return;

			if (order.OrderString == "Move")
			{
				var cell = self.World.Map.Clamp(Self.World.Map.CellContaining(order.Target.CenterPosition));
				if (!Info.LocomotorInfo.MoveIntoShroud && !self.Owner.Shroud.IsExplored(cell))
					return;

				self.QueueActivity(order.Queued, MoveTo(cell, evaluateNearestMovableCell: true));
				self.ShowTargetLines();
			}
			else if (order.OrderString == "Stop")
			{
				self.CancelActivity();
			}
			else if (order.OrderString == "Scatter")
			{
				self.QueueActivity(order.Queued, new TeleportNudge(self));
				self.ShowTargetLines();
			}
		}

		public new Activity MoveTo(CPos cell, int nearEnough = 0, Actor ignoreActor = null, bool evaluateNearestMovableCell = false, Color? targetLineColor = null)
		{
			var targetCell = Self.World.Map.Clamp(cell);
			return new Teleport(Self, targetCell, evaluateNearestMovableCell);
		}

		public new Activity MoveWithinRange(in Target target, WDist range, WPos? initialTargetPosition = null, Color? targetLineColor = null)
		{
			var cell = target.Positions.ClosestToIgnoringPath(Self.CenterPosition);
			return new Teleport(Self, Self.World.Map.CellContaining(cell), true);
		}

		public new Activity MoveWithinRange(in Target target, WDist minRange, WDist maxRange, WPos? initialTargetPosition = null, Color? targetLineColor = null)
		{
			var cell = target.Positions.ClosestToIgnoringPath(Self.CenterPosition);
			return new Teleport(Self, Self.World.Map.CellContaining(cell), true);
		}

		public new Activity MoveFollow(Actor self, in Target target, WDist minRange, WDist maxRange, WPos? initialTargetPosition = null, Color? targetLineColor = null)
		{
			var targetCell = Self.World.Map.CellContaining(target.CenterPosition);
			return new Teleport(Self, targetCell, true);
		}

		public new Activity MoveToTarget(Actor self, in Target target, WPos? initialTargetPosition = null, Color? targetLineColor = null)
		{
			return new TeleportAdjacentTo(self, target);
		}

		public new Activity MoveIntoTarget(Actor self, in Target target)
		{
			if (target.Type == TargetType.Invalid)
				return null;

			return new TeleportIntoTarget(self, target);
		}

		public new Activity MoveOntoTarget(Actor self, in Target target, in WVec offset, WAngle? facing, Color? targetLineColor = null)
		{
			var targetCell = Self.World.Map.Clamp(Self.World.Map.CellContaining(target.CenterPosition));
			return new Teleport(Self, targetCell, true);
		}

		public new Activity LocalMove(Actor self, WPos fromPos, WPos toPos)
		{
			var targetCell = Self.World.Map.Clamp(Self.World.Map.CellContaining(toPos));
			return new Teleport(Self, targetCell, true);
		}

		public new Activity ReturnToCell(Actor self)
		{
			return MoveTo(ToCell, evaluateNearestMovableCell: true);
		}

		public new CPos NearestMoveableCell(CPos target)
		{
			if (CanEnterCell(target) && CanStayInCell(target))
			{
				return target;
			}

			foreach (var tile in Self.World.Map.FindTilesInAnnulus(target, 1, 10))
			{
				if (CanEnterCell(tile) && CanStayInCell(tile))
				{
					return tile;
				}
			}

			return CPos.Zero;
		}

		public new Activity GetCreationActivity()
		{
			if (creationRallypoint is null)
			{
				return null;
			}

			var rallypoint = creationRallypoint.FirstOrDefault();

			return MoveTo(rallypoint, evaluateNearestMovableCell: true);
		}

		public new void OnNotifyBlockingMove(Actor self, Actor blocking)
		{
			if (!self.AppearsFriendlyTo(blocking))
			{
				return;
			}

			if (self.IsIdle)
			{
				self.QueueActivity(false, new TeleportNudge(self));
			}
		}

		float ISelectionBar.GetValue()
		{
			var progress = chargeTick < 0 ? 0 : chargeTick;

			return (float)(Info.ChargeDelay - progress) / Info.ChargeDelay;
		}

		Color ISelectionBar.GetColor() { return Color.Cyan; }

		bool ISelectionBar.DisplayWhenEmpty { get { return false; } }

		public int CalculateDistance(CPos fromCell, CPos toCell)
		{
			double deltaX = fromCell.X - toCell.X;
			double deltaY = fromCell.Y - toCell.Y;
			return (int)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
		}

		public void ResetChargeTime(int distance)
		{
			if (string.IsNullOrEmpty(Info.TeleportCondition))
				return;

			chargeTick = distance < 100 ? (int)(Info.ChargeDelay * ((float)distance / 100)) : Info.ChargeDelay;

			if (token == Actor.InvalidConditionToken)
			{
				token = Self.GrantCondition(Info.TeleportCondition);
			}
		}
	}
}
