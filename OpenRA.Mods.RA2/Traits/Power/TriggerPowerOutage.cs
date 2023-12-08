using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits.Power
{
	public class TriggerPowerOutageInfo : ConditionalTraitInfo
	{
		public override object Create(ActorInitializer init)
		{
			return new TriggerPowerOutage(init.Self, this);
		}
	}

	public class TriggerPowerOutage : ConditionalTrait<TriggerPowerOutageInfo>, ITick
	{
		PowerManager playerPower;

		int duration => 100;

		public TriggerPowerOutage(Actor self, TriggerPowerOutageInfo info) : base(info)
		{
			playerPower = self.Owner.PlayerActor.Trait<PowerManager>();
		}

		protected override void TraitEnabled(Actor self)
		{
			playerPower.TriggerPowerOutage(duration);
		}

		protected override void TraitDisabled(Actor self)
		{
			playerPower.TriggerPowerOutage(0);
		}

		public void Tick(Actor self)
		{
			if (playerPower != null && !IsTraitDisabled && playerPower.PowerOutageRemainingTicks <= 1)
			{
				TraitEnabled(self);
			}
		}
	}
}
