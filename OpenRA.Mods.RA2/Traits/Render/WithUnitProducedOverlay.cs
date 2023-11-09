using OpenRA.Mods.Common.Traits.Render;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;
using OpenRA.Graphics;

namespace OpenRA.Mods.RA2.Traits.Render
{
	[Desc("Rendered when the actor constructed a building.")]
	public class WithUnitProducedOverlayInfo : PausableConditionalTraitInfo, Requires<RenderSpritesInfo>, Requires<BodyOrientationInfo>
	{
		[SequenceReference]
		[Desc("Sequence name to use")]
		public readonly string Sequence = "produced-overlay";

		[Desc("Position relative to body")]
		public readonly WVec Offset = WVec.Zero;

		[PaletteReference(nameof(IsPlayerPalette))]
		[Desc("Custom palette name")]
		public readonly string Palette = null;

		[Desc("Custom palette is a player palette BaseName")]
		public readonly bool IsPlayerPalette = false;

		[GrantedConditionReference]
		[Desc("The condition to grant")]
		public readonly string ProductionCondition = null;

		public override object Create(ActorInitializer init) { return new WithUnitProducedOverlay(init.Self, this); }
	}

	public class WithUnitProducedOverlay : ConditionalTrait<WithUnitProducedOverlayInfo>, INotifyDamageStateChanged, INotifyProduction
	{
		readonly Animation overlay;
		readonly WithUnitProducedOverlayInfo info;
		bool visible;
		int token = Actor.InvalidConditionToken;


		public WithUnitProducedOverlay(Actor self, WithUnitProducedOverlayInfo info) : base(info)
		{
			this.info = info;
			var rs = self.Trait<RenderSprites>();
			var body = self.Trait<BodyOrientation>();

			overlay = new Animation(self.World, rs.GetImage(self));

			var anim = new AnimationWithOffset(overlay,
				() => body.LocalToWorld(info.Offset.Rotate(body.QuantizeOrientation(self.Orientation))),
				() => !visible || IsTraitDisabled);

			overlay.PlayThen(info.Sequence, () => visible = false);
			rs.Add(anim, info.Palette, info.IsPlayerPalette);
		}

		public void DamageStateChanged(Actor self, AttackInfo e)
		{
			overlay.ReplaceAnim(RenderSprites.NormalizeSequence(overlay, e.DamageState, overlay.CurrentSequence.Name));
		}

		public void UnitProduced(Actor self, Actor other, CPos exit)
		{
			if (info.ProductionCondition is not null && token == Actor.InvalidConditionToken)
			{
				token = self.GrantCondition(info.ProductionCondition);
			}

			visible = true;
			overlay.PlayThen(overlay.CurrentSequence.Name,
				() =>
				{
					visible = false;
					if (info.ProductionCondition is not null && token != Actor.InvalidConditionToken)
					{
						token = self.RevokeCondition(token);
					}
				});
		}
	}
}
