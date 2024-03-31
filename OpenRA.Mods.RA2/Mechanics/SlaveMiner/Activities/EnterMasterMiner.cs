﻿using OpenRA.Mods.Common.Activities;
using OpenRA.Mods.RA2.Mechanics.SlaveMiner.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Mechanics.SlaveMiner.Activities
{
	class EnterMasterMiner : Enter
	{
		readonly Actor enterActor;
		readonly MasterMiner masterMiner;
		readonly INotifySlaveEntering notifySlaveEntering;

		public EnterMasterMiner(Actor self, in Target target, Color? targetLineColor)
			: base(self, target, targetLineColor)
		{
			enterActor = target.Actor;
			masterMiner = enterActor.TraitOrDefault<MasterMiner>();
			notifySlaveEntering = enterActor.TraitsImplementing<INotifySlaveEntering>().FirstEnabledTraitOrDefault();
		}

		protected override bool TryStartEnter(Actor self, Actor targetActor)
		{
			if (masterMiner == null || masterMiner.IsTraitDisabled)
			{
				Cancel(self, true);
				return false;
			}

			notifySlaveEntering.OnSlaveEntering(enterActor, self);
			return true;
		}

		protected override void TickInner(Actor self, in Target target, bool targetIsDeadOrHiddenActor)
		{
			if (masterMiner != null && masterMiner.IsTraitDisabled)
			{
				Cancel(self, true);
			}
		}

		protected override void OnEnterComplete(Actor self, Actor targetActor)
		{
			self.World.AddFrameEndTask(w =>
			{
				if (self.IsDead || targetActor != enterActor)
				{
					return;
				}

				notifySlaveEntering.OnSlaveEntered(enterActor, self);
				w.Remove(self);
			});
		}
	}
}
