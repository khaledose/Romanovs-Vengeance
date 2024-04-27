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
	[Desc("Dock is reserved for produced rearmable actor")]
	public class RearmDockInfo : TraitInfo
	{
		[FieldLoader.Require]
		[GrantedConditionReference]
		[Desc("The condition to grant when an actor is linked to this dock.")]
		public readonly string DockedCondition = null;

		[ActorReference]
		[Desc("The actors to grant condition for. If empty condition will be granted for all actors.")]
		public readonly HashSet<string> Actors = new();

		[Desc("The docked actor will dispose when dock is disposed.")]
		public readonly bool DisposeOnDockDisposed = true;

		[Desc("The docked actor will change owner when dock owner is changed.")]
		public readonly bool ChangeOwner = true;

		public override object Create(ActorInitializer init) { return new RearmDock(this); }
	}

	public class RearmDock : INotifyProduction, INotifyActorDisposing, INotifyOwnerChanged, ITick
	{
		readonly RearmDockInfo info;

		Actor dockedActor;
		int token = Actor.InvalidConditionToken;

		public RearmDock(RearmDockInfo info)
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
				token = self.GrantCondition(info.DockedCondition);

			dockedActor = other;
		}

		void INotifyActorDisposing.Disposing(Actor self)
		{
			if (dockedActor is null || !info.DisposeOnDockDisposed)
				return;
			
			dockedActor.Dispose();
		}

		void INotifyOwnerChanged.OnOwnerChanged(Actor self, Player oldOwner, Player newOwner)
		{
			if (dockedActor is null || !info.ChangeOwner)
				return;
			
			dockedActor.ChangeOwnerSync(newOwner);
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
