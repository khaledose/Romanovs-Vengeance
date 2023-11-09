using OpenRA.Activities;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.RA2.Traits;
using OpenRA.Traits;
using System.Linq;
using System.Reflection;

namespace OpenRA.Mods.RA2.Activities
{
	public class TractorFall : Activity
	{
		private readonly Tractable tractable;
		private readonly TractableInfo info;
		private readonly Actor tractor;

		public TractorFall(Tractable tractable, Actor tractor)
		{
			this.tractable = tractable;
			this.tractor = tractor;
			info = tractable.Info;
			IsInterruptible = false;
		}

		protected override void OnLastRun(Actor self)
		{
			var terrain = self.World.Map.GetTerrainInfo(self.Location);
			var mobile = self.TraitOrDefault<Mobile>();
			var actors = self.World.FindActorsInCircle(self.CenterPosition, new WDist(2048)).Where(a => a != self);

			actors = actors.Where(a => a.OccupiesSpace.OccupiedCells().Any(c => c.Cell == self.Location));

			if (actors.Any() || mobile is null || !mobile.Locomotor.Info.TerrainSpeeds.ContainsKey(terrain.Type) || mobile.Locomotor.Info.TerrainSpeeds[terrain.Type].Speed == 0)
			{
				var health = self.Trait<Health>();
				var damage = new Damage(health.MaxHP, info.DamageTypes);

				self.Kill(tractor, info.DamageTypes);

				foreach (var actor in actors)
				{
					actor.InflictDamage(tractor, damage);
				}
			}

			tractable.RevokeTractingCondition(self);
		}

		public override bool Tick(Actor self)
		{
			if (self.World.Map.DistanceAboveTerrain(self.CenterPosition).Length <= 0)
			{
				return true;
			}

			var fallSpeed = -info.FallGravity.Length;
			var move = new WVec(0, 0, fallSpeed);

			var pos = self.CenterPosition + move;

			if (pos.Z < 0)
			{
				pos = pos + new WVec(0, 0, -pos.Z);
			}

			tractable.SetPosition(self, pos);

			return false;
		}
	}
}
