using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits.Conditions
{
	[Desc("Grants condition as long as an actor is resupplying another actor.")]
	public class GrantConditionOnResupplyInfo : ConditionalTraitInfo
	{
		[FieldLoader.Require]
		[GrantedConditionReference]
		[Desc("Condition to grant.")]
		public readonly string Condition = null;

		[Desc("ResupplyTypes at which the condition is granted. Options are Rearm and Repair.")]
		public readonly ResupplyType ValidOn = ResupplyType.Rearm | ResupplyType.Repair;

		public override object Create(ActorInitializer init) { return new GrantConditionOnResupply(init.Self, this); }
	}

	public class GrantConditionOnResupply : ConditionalTrait<GrantConditionOnResupplyInfo>, INotifyResupply
	{
		int token = Actor.InvalidConditionToken;
		bool repair;
		bool rearm;

		public GrantConditionOnResupply(Actor self, GrantConditionOnResupplyInfo info) : base(info)
		{
			repair = info.ValidOn.HasFlag(ResupplyType.Repair);
			rearm = info.ValidOn.HasFlag(ResupplyType.Rearm);
		}

		public void BeforeResupply(Actor host, Actor target, ResupplyType types)
		{
		}

		public void ResupplyTick(Actor host, Actor target, ResupplyType types)
		{
			if ((repair && types.HasFlag(ResupplyType.Repair)) || (rearm && types.HasFlag(ResupplyType.Rearm)))
			{
				GrantResupplyCondition(host);
			}
			else
			{
				RevokeResupplyCondition(host);
			}
		}

		protected override void TraitDisabled(Actor self)
		{
			RevokeResupplyCondition(self);
		}

		private void GrantResupplyCondition(Actor self)
		{
			if (token == Actor.InvalidConditionToken)
			{
				token = self.GrantCondition(Info.Condition);
			}
		}

		private void RevokeResupplyCondition(Actor self)
		{
			if (token != Actor.InvalidConditionToken)
			{
				token = self.RevokeCondition(token);
			}
		}
	}
}
