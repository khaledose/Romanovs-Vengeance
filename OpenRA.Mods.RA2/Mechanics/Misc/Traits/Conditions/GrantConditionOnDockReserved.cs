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
using System.Linq;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Mechanics.Misc.Traits.Conditions
{
	[Desc("Grants a condition when this actor produces a docked actor.")]
	public class GrantConditionOnDockReservedInfo : TraitInfo
	{
		[FieldLoader.Require]
		[GrantedConditionReference]
		[Desc("The condition to grant")]
		public readonly string Condition = null;

		[ActorReference]
		[Desc("The actors to grant condition for. If empty condition will be granted for all actors.")]
		public readonly HashSet<string> Actors = new();

		public override object Create(ActorInitializer init) { return new GrantConditionOnDockReserved(this); }
	}

	public class GrantConditionOnDockReserved : INotifyProduction, ITick
	{
		readonly GrantConditionOnDockReservedInfo info;

		Actor dockedActor;
		int token = Actor.InvalidConditionToken;

		public GrantConditionOnDockReserved(GrantConditionOnDockReservedInfo info)
		{
			this.info = info;
		}

		void INotifyProduction.UnitProduced(Actor self, Actor other, CPos exit)
		{
			if (info.Actors.Count > 0 && !info.Actors.Select(a => a.ToLowerInvariant()).Contains(other.Info.Name))
				return;

			if (other.IsDead)
				return;

			if (token == Actor.InvalidConditionToken)
				token = self.GrantCondition(info.Condition);

			dockedActor = other;
		}

		void ITick.Tick(Actor self)
		{
			if ((dockedActor is null || dockedActor.IsDead) && token != Actor.InvalidConditionToken)
			{
				token = self.RevokeCondition(token);
				dockedActor = null;
			}
		}
	}
}
