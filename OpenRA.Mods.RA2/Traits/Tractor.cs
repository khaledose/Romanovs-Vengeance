using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits
{
	public class TractorInfo : ConditionalTraitInfo
	{
		public override object Create(ActorInitializer init) { return new Tractor(this); }
	}

	public class Tractor : ConditionalTrait<TractorInfo>, INotifyAiming, INotifyAttack
	{
		AttackFollow attackFollow;
		TractorInfo info;

		public Tractor(TractorInfo info) : base(info)
		{
			this.info = info;
		}

		protected override void Created(Actor self)
		{
			attackFollow = self.TraitOrDefault<AttackFollow>();
			base.Created(self);
		}

		void INotifyAiming.StartedAiming(Actor self, AttackBase attack)
		{
			var victim = attackFollow.RequestedTarget.Actor;
			if (victim is null) return;

			var tractable = victim.TraitOrDefault<Tractable>();
			if (tractable is null) return;

			var tractor = tractable.Tractor;
			if (tractor is not null && self != tractor)
			{
				attackFollow.ClearRequestedTarget();
			}
		}

		void INotifyAiming.StoppedAiming(Actor self, AttackBase attack)
		{
		}

		void INotifyAttack.Attacking(Actor self, in Target target, Armament a, Barrel barrel)
		{
		}

		void INotifyAttack.PreparingAttack(Actor self, in Target target, Armament a, Barrel barrel)
		{
			var victim = target.Actor;
			if (victim is null) return;

			var tractable = victim.TraitOrDefault<Tractable>();
			if (tractable is null) return;

			var tractor = tractable.Tractor;
			if ((tractor is not null && self != tractor) || IsTraitDisabled)
			{
				attackFollow.ClearRequestedTarget();
			}
		}
	}

}
