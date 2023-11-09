using OpenRA.Mods.Common.Traits.Render;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;
using OpenRA.Graphics;

namespace OpenRA.Mods.RA2.Traits.Render
{
	public class WithResupplyOverlayInfo : PausableConditionalTraitInfo, Requires<RenderSpritesInfo>, Requires<BodyOrientationInfo>
	{
		[SequenceReference]
		[Desc("Sequence to use upon resupply beginning.")]
		public readonly string StartSequence = null;

		[SequenceReference]
		[Desc("Sequence name to play once during resupply intervals or repeatedly if a start sequence is set.")]
		public readonly string Sequence = "active";

		[SequenceReference]
		[Desc("Sequence to use after resupplying has finished.")]
		public readonly string EndSequence = null;

		[Desc("Position relative to body")]
		public readonly WVec Offset = WVec.Zero;

		[PaletteReference(nameof(IsPlayerPalette))]
		[Desc("Custom palette name")]
		public readonly string Palette = null;

		[Desc("Custom palette is a player palette BaseName")]
		public readonly bool IsPlayerPalette = false;

		[GrantedConditionReference]
		[Desc("Condition to grant while animating.")]
		public readonly string ResupplyCondition = null;

		[Desc("ResupplyTypes at which the condition is granted. Options are Rearm and Repair.")]
		public readonly ResupplyType ValidOn = ResupplyType.Rearm | ResupplyType.Repair;

		public override object Create(ActorInitializer init) { return new WithResupplyOverlay(init.Self, this); }
	}

	public class WithResupplyOverlay : PausableConditionalTrait<WithResupplyOverlayInfo>, INotifyDamageStateChanged, INotifyResupply
	{
		readonly Animation overlay;
		int token = Actor.InvalidConditionToken;
		bool repair;
		bool rearm;
		bool visible;
		bool resupplying;

		public WithResupplyOverlay(Actor self, WithResupplyOverlayInfo info)
			: base(info)
		{
			var rs = self.Trait<RenderSprites>();
			var body = self.Trait<BodyOrientation>();

			overlay = new Animation(self.World, rs.GetImage(self), () => IsTraitPaused);
			overlay.PlayThen(info.Sequence, () => visible = false);

			var anim = new AnimationWithOffset(overlay,
				() => body.LocalToWorld(info.Offset.Rotate(body.QuantizeOrientation(self.Orientation))),
				() => IsTraitDisabled || !visible,
				p => RenderUtils.ZOffsetFromCenter(self, p, 1));

			rs.Add(anim, info.Palette, info.IsPlayerPalette);

			repair = info.ValidOn.HasFlag(ResupplyType.Repair);
			rearm = info.ValidOn.HasFlag(ResupplyType.Rearm);
		}

		void INotifyDamageStateChanged.DamageStateChanged(Actor self, AttackInfo e)
		{
			overlay.ReplaceAnim(RenderSprites.NormalizeSequence(overlay, e.DamageState, overlay.CurrentSequence.Name));
		}

		void INotifyResupply.BeforeResupply(Actor self, Actor target, ResupplyType types)
		{
			resupplying = (repair && types.HasFlag(ResupplyType.Repair)) || (rearm && types.HasFlag(ResupplyType.Rearm));
			if (!resupplying)
			{
				return;
			}

			if (Info.StartSequence is not null)
			{
				visible = true;
				GrantResupplyCondition(self);
				overlay.PlayThen(RenderSprites.NormalizeSequence(overlay, self.GetDamageState(), Info.StartSequence),
					() => overlay.PlayRepeating(RenderSprites.NormalizeSequence(overlay, self.GetDamageState(), Info.Sequence)));
			}
		}

		void INotifyResupply.ResupplyTick(Actor self, Actor target, ResupplyType types)
		{
			var wasResupplying = resupplying;
			resupplying = (repair && types.HasFlag(ResupplyType.Repair)) || (rearm && types.HasFlag(ResupplyType.Rearm));

			if (resupplying && Info.StartSequence is null && !visible)
			{
				visible = true;
				GrantResupplyCondition(self);
				overlay.PlayThen(overlay.CurrentSequence.Name,
					() =>
					{
						if (Info.EndSequence is null)
						{
							RevokeResupplyCondition(self);
						}

						visible = false;
					});
			}

			if (!resupplying && wasResupplying && Info.EndSequence is not null)
			{
				visible = true;
				overlay.PlayThen(Info.EndSequence,
					() =>
					{
						RevokeResupplyCondition(self);
						visible = false;
					});
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
				token = self.GrantCondition(Info.ResupplyCondition);
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
