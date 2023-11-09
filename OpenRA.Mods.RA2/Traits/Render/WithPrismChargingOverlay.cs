using OpenRA.Graphics;
using OpenRA.Mods.AS.Traits;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.Common.Traits.Render;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits.Render
{
	public class WithPrismChargingOverlayInfo : PausableConditionalTraitInfo, Requires<BodyOrientationInfo>, Requires<RenderSpritesInfo>
	{
		[SequenceReference]
		[Desc("Sequence to use for charge animation.")]
		public readonly string Sequence = "active";

		[Desc("Position relative to body")]
		public readonly WVec Offset = WVec.Zero;

		[PaletteReference(nameof(IsPlayerPalette))]
		[Desc("Custom palette name")]
		public readonly string Palette = null;

		[Desc("Custom palette is a player palette BaseName")]
		public readonly bool IsPlayerPalette = false;

		[GrantedConditionReference]
		[Desc("Condition to grant while charging.")]
		public readonly string ChargingCondition = null;

		public override object Create(ActorInitializer init) { return new WithPrismChargingOverlay(init.Self, this); }
	}

	public class WithPrismChargingOverlay : PausableConditionalTrait<WithPrismChargingOverlayInfo>, INotifyDamageStateChanged, INotifyPrismCharging
	{
		readonly Animation overlay;
		int token = Actor.InvalidConditionToken;
		bool visible = false;

		public WithPrismChargingOverlay(Actor self, WithPrismChargingOverlayInfo info) : base(info)
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
		}

		public void DamageStateChanged(Actor self, AttackInfo e)
		{
			overlay.ReplaceAnim(RenderSprites.NormalizeSequence(overlay, e.DamageState, overlay.CurrentSequence.Name));
		}

		public void Charging(Actor self, in Target target)
		{
			GrantChargingCondition(self);
			visible = true;
			overlay.PlayThen(overlay.CurrentSequence.Name,
				() =>
				{
					visible = false;
					RevokeChargingCondition(self);
				});
		}

		protected override void TraitDisabled(Actor self)
		{
			RevokeChargingCondition(self);
		}

		private void GrantChargingCondition(Actor self)
		{
			if (token == Actor.InvalidConditionToken)
			{
				token = self.GrantCondition(Info.ChargingCondition);
			}
		}

		private void RevokeChargingCondition(Actor self)
		{
			if (token != Actor.InvalidConditionToken)
			{
				token = self.RevokeCondition(token);
			}
		}
	}
}
