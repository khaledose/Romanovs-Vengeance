using OpenRA.Activities;
using OpenRA.Mods.Common.Effects;
using OpenRA.Mods.RA2.Mechanics.ChronoTeleporting.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Mechanics.ChronoTeleporting.Activities
{
	public class Teleport : Activity
	{
		readonly Actor self;
		readonly ChronoMobile mobile;
		readonly ChronoMobileInfo info;
		CPos toCell;
		SubCell subCell;
		CPos position;
		WPos centerPosition;
		int distance;
		bool teleported;
		bool retry;

		public Teleport(Actor self, CPos dest, bool retryInvalidCells)
		{
			this.self = self;
			toCell = dest;
			mobile = self.TraitOrDefault<ChronoMobile>();
			info = mobile.Info;
			retry = retryInvalidCells;
		}

		protected override void OnFirstRun(Actor self)
		{
			if (!mobile.CanTeleport)
			{
				return;
			}

			centerPosition = self.CenterPosition;
			position = self.World.Map.CellContaining(centerPosition);
			var isValid = SetDestination(false);

			if (!isValid && retry)
			{
				isValid = SetDestination(true);
			}

			if (!isValid)
			{
				return;
			}

			PlayTeleportSound();

			mobile.SetPosition(self, toCell, subCell);

			if (info.HasTeleportEffect && !string.IsNullOrEmpty(info.TeleportEffectSequence) && !string.IsNullOrEmpty(info.TeleportEffect))
			{
				AddTeleportEffects();
			}

			teleported = true;
		}

		protected override void OnLastRun(Actor self)
		{
			if (teleported)
			{
				mobile.ResetChargeTime(distance);
			}
		}

		private void PlayTeleportSound()
		{
			if (!string.IsNullOrEmpty(info.ChronoshiftSound))
			{
				Game.Sound.Play(SoundType.World, info.ChronoshiftSound, self.CenterPosition);
				Game.Sound.Play(SoundType.World, info.ChronoshiftSound, self.World.Map.CenterOfCell(toCell));
			}
		}

		private void AddTeleportEffects()
		{
			self.World.AddFrameEndTask(w =>
			{
				w.Add(new SpriteEffect(centerPosition, w, "explosion", info.TeleportEffectSequence, info.TeleportEffect));
				w.Add(new SpriteEffect(self.CenterPosition, w, "explosion", info.TeleportEffectSequence, info.TeleportEffect));
			});
		}

		private bool SetDestination(bool retry)
		{
			toCell = retry ? mobile.NearestMoveableCell(toCell) : toCell;
			subCell = mobile.GetAvailableSubCell(toCell);
			distance = mobile.CalculateDistance(position, toCell);

			return toCell != CPos.Zero && subCell != SubCell.Invalid && mobile.CanEnterCell(toCell) && mobile.CanStayInCell(toCell);
		}
	}
}
