using System;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits
{
	[Desc("Grants condition as long as an actor is resupplying another actor.")]
	public class GrantConditionOnReserveInfo : ConditionalTraitInfo
	{
		[FieldLoader.Require]
		[GrantedConditionReference]
		[Desc("Condition to grant.")]
		public readonly string Condition = null;

		public override object Create(ActorInitializer init) { return new GrantConditionOnReserve(init.Self, this); }
	}

	public class GrantConditionOnReserve : ConditionalTrait<GrantConditionOnReserveInfo>, INotifyResupply
	{
		int token = Actor.InvalidConditionToken;

		public GrantConditionOnReserve(Actor self, GrantConditionOnReserveInfo info) : base(info)
		{
		}

		public void BeforeResupply(Actor host, Actor target, ResupplyType types)
		{
			GrantReservedCondition(host);
		}

		public void ResupplyTick(Actor host, Actor target, ResupplyType types)
		{
			GrantReservedCondition(host);
		}

		public void Tick(Actor self)
		{
			// if (Reservable.IsReserved(self))
			// {
			// 	Console.WriteLine($"{self} a7aaaaaaaaaaaaaaaaaaaaaaaaaaaa");
			// 	GrantReservedCondition(self);
			// }
			// else
			// {
			// 	Console.WriteLine($"{self} a7aaaaaaaaaaaaaaaaaaaaaaaaaaaa la2");
			// 	RevokeReservedCondition(self);
			// }
		}

		protected override void TraitDisabled(Actor self)
		{
			RevokeReservedCondition(self);
		}

		private void GrantReservedCondition(Actor self)
		{
			if (token == Actor.InvalidConditionToken)
			{
				token = self.GrantCondition(Info.Condition);
			}
		}

		private void RevokeReservedCondition(Actor self)
		{
			if (token != Actor.InvalidConditionToken)
			{
				token = self.RevokeCondition(token);
			}
		}
	}
}
