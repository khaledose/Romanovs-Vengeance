using OpenRA.Mods.AS.Traits;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;
using System.Collections.Generic;

namespace OpenRA.Mods.RA2.Traits
{
	public class PrismSupportModifierInfo : ConditionalTraitInfo, Requires<AttackPrismSupportedInfo>
	{
		[FieldLoader.Require]
		[Desc("Percentage modifier to apply.")]
		public readonly int Modifier = 100;

		[Desc("Weapon types to applies to. Leave empty to apply to all weapons.")]
		public readonly HashSet<string> Types = new();

		public override object Create(ActorInitializer init)
		{
			return new PrismSupportModifier(init.Self, this);
		}
	}

	public class PrismSupportModifier : ConditionalTrait<PrismSupportModifierInfo>, IFirepowerModifier
	{
		readonly AttackPrismSupported aps;

		public PrismSupportModifier(Actor self, PrismSupportModifierInfo info) : base(info)
		{
			aps = self.TraitOrDefault<AttackPrismSupported>();
		}

		public int GetFirepowerModifier(string armamentName)
		{
			if (IsTraitDisabled ||
				string.IsNullOrEmpty(armamentName) ||
				(Info.Types.Count > 0 && !Info.Types.Contains(armamentName)) ||
				aps.SupportersCount == 0)
			{
				return 100;
			}

			return aps.SupportersCount * Info.Modifier;
		}
	}
}
