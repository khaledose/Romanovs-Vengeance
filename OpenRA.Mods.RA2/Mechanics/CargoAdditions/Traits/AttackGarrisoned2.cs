using System;
using System.Collections.Generic;
using System.Linq;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.Common.Traits.Render;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Mechanics.CargoAdditions.Traits
{
	[Desc("Implements the YR OpenTopped logic where transported actors used separate firing offsets, ignoring facing."
		+ "Compatible with both `Cargo`/`Passengers` or `Garrionable`/`Garrisoners` logic.")]
	public class AttackGarrisoned2Info : AttackFollowInfo
	{
		[FieldLoader.Require]
		[Desc("Fire port offsets in local coordinates.")]
		public readonly WVec[] PortOffsets = Array.Empty<WVec>();

		[Desc("Draws passenger muzzle sequences randomly.")]
		public readonly bool RandomMuzzleActivations = true;

		public override object Create(ActorInitializer init) { return new AttackGarrisoned2(init.Self, this); }

		public override void RulesetLoaded(Ruleset rules, ActorInfo ai)
		{
			if (PortOffsets.Length == 0)
				throw new YamlException("PortOffsets must have at least one entry.");

			base.RulesetLoaded(rules, ai);
		}
	}

	public class AttackGarrisoned2 : AttackFollow, IRender, INotifyPassengerEntered, INotifyPassengerExited
	{
		public new readonly AttackGarrisoned2Info Info;
		INotifyAttack[] notifyAttacks;
		readonly Lazy<BodyOrientation> coords;
		readonly List<Armament> armaments;
		readonly HashSet<(AnimationWithOffset Animation, string Sequence)> muzzles;
		readonly Dictionary<Actor, IFacing> paxFacing;
		readonly Dictionary<Actor, IPositionable> paxPos;
		readonly Dictionary<Actor, RenderSprites> paxRender;
		int tick;

		public AttackGarrisoned2(Actor self, AttackGarrisoned2Info info)
			: base(self, info)
		{
			Info = info;
			coords = Exts.Lazy(() => self.Trait<BodyOrientation>());
			armaments = new List<Armament>();
			muzzles = new HashSet<(AnimationWithOffset Animation, string Sequence)>();
			paxFacing = new Dictionary<Actor, IFacing>();
			paxPos = new Dictionary<Actor, IPositionable>();
			paxRender = new Dictionary<Actor, RenderSprites>();
		}

		protected override void Created(Actor self)
		{
			notifyAttacks = self.TraitsImplementing<INotifyAttack>().ToArray();
			base.Created(self);
		}

		protected override Func<IEnumerable<Armament>> InitializeGetArmaments(Actor self)
		{
			return () => armaments;
		}

		void INotifyPassengerEntered.OnPassengerEntered(Actor self, Actor passenger)
		{
			paxFacing.Add(passenger, passenger.Trait<IFacing>());
			paxPos.Add(passenger, passenger.Trait<IPositionable>());
			paxRender.Add(passenger, passenger.Trait<RenderSprites>());
			foreach (var a in passenger.TraitsImplementing<Armament>())
			{
				if (!a.IsTraitDisabled && Info.Armaments.Contains(a.Info.Name))
				{
					a.AddNotifyAttacks(self, notifyAttacks);
					armaments.Add(a);
				}
			}
		}

		void INotifyPassengerExited.OnPassengerExited(Actor self, Actor passenger)
		{
			paxFacing.Remove(passenger);
			paxPos.Remove(passenger);
			paxRender.Remove(passenger);
			foreach (var a in armaments.ToList())
			{
				if (a.Actor == passenger)
				{
					a.RemoveNotifyAttacks(notifyAttacks);
					armaments.Remove(a);
				}
			}
		}

		WVec PortOffset(Actor self, WVec offset)
		{
			var bodyOrientation = coords.Value.QuantizeOrientation(self.Orientation);
			return coords.Value.LocalToWorld(offset.Rotate(bodyOrientation));
		}

		public override void DoAttack(Actor self, in Target target)
		{
			if (!CanAttack(self, target))
				return;

			foreach (var a in armaments)
			{
				if (a.IsTraitDisabled || !a.CheckFire(a.Actor, facing, target))
				{
					continue;
				}

				if (!Info.RandomMuzzleActivations)
				{
					RenderMuzzle(self, a);
				}

				foreach (var npa in notifyAttacks)
				{
					npa.Attacking(self, target, a, null);
				}
			}
		}

		public void RenderMuzzle(Actor self, Armament a)
		{
			if (a is null || a.Info.MuzzleSequence is null)
			{
				return;
			}

			var pos = self.CenterPosition;
			var targetedPosition = GetTargetPosition(pos, RequestedTarget);
			var targetYaw = (targetedPosition - pos).Yaw;
			var port = Info.PortOffsets[armaments.IndexOf(a)];

			paxFacing[a.Actor].Facing = targetYaw;
			paxPos[a.Actor].SetCenterPosition(a.Actor, pos + PortOffset(self, port));
			var muzzleAnim = new Animation(self.World, paxRender[a.Actor].GetImage(a.Actor), () => targetYaw);
			var muzzleFlash = new AnimationWithOffset(muzzleAnim,
				() => PortOffset(self, port),
				() => false,
				p => RenderUtils.ZOffsetFromCenter(self, p, 1024));

			var muzzle = (muzzleFlash, a.Info.MuzzlePalette);
			muzzles.Add(muzzle);
			muzzleAnim.PlayThen(a.Info.MuzzleSequence, () => muzzles.Remove(muzzle));
			tick = a.Weapon.ReloadDelay;
		}

		IEnumerable<IRenderable> IRender.Render(Actor self, WorldRenderer wr)
		{
			// Display muzzle flashes
			foreach (var m in muzzles)
				foreach (var r in m.Animation.Render(self, wr.Palette(m.Sequence)))
					yield return r;
		}

		IEnumerable<Rectangle> IRender.ScreenBounds(Actor self, WorldRenderer wr)
		{
			// Muzzle flashes don't contribute to actor bounds
			yield break;
		}

		protected override void Tick(Actor self)
		{
			base.Tick(self);

			if (Info.RandomMuzzleActivations && RequestedTarget.Type != Target.Invalid.Type && tick-- <= 0)
			{
				RenderMuzzle(self, armaments.Where(a => !a.IsTraitDisabled).RandomOrDefault(self.World.LocalRandom));
			}

			// Take a copy so that Tick() can remove animations
			foreach (var m in muzzles.ToArray())
				m.Animation.Animation.Tick();
		}
	}
}
