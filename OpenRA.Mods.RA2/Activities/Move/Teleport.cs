using OpenRA.Activities;
using OpenRA.Mods.Common.Effects;
using OpenRA.Mods.RA2.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Activities
{
	public class Teleport : Activity
	{
		private readonly Actor self;
		private readonly ChronoMobile mobile;
		private readonly ChronoMobileInfo info;
		private CPos toCell;
		private SubCell subCell;
		private CPos position;
		private WPos centerPosition;
		private int distance;
		private bool teleported;
		private bool retry;

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
			bool isValid = SetDestination(false);

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
