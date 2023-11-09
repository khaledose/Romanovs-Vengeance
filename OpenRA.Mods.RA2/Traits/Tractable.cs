using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.RA2.Activities;
using OpenRA.Primitives;
using OpenRA.Support;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits
{
	[Desc("Can this actor be tracted with a traction beam?")]
	public class TractableInfo : PausableConditionalTraitInfo
	{
		[Desc("Condition to grant when the unit is under tracting state")]
		[GrantedConditionReference]
		public readonly string TractingCondition = null;

		[Desc("Altitude this victim gets tracted at.")]
		public readonly WDist CruiseAltitude = new WDist(1280);

		[Desc("How fast does this actor get dragged?",
			"0 to only lift the target in air (Yuriko in RA3!).",
			"You can also have negative speed for push back.")]
		public readonly WDist CruiseSpeed = new WDist(20);

		[Desc("How fast this actor ascends when being pulled to TractionAltitude?")]
		public readonly WDist AltitudeVelocity = new WDist(20);

		[Desc("Acceleration this actor descends on traction done or cancel")]
		public readonly WDist FallGravity = new WDist(43);

		[Desc("Damage factor that deterimes the actor receives when it falls. (Damage = MaxHP * DamageFactor / 100")]
		public readonly int DamageFactor = 25;

		[Desc("Minimum altitude where this victim is considered airborne")]
		public readonly int MinAirborneAltitude = 1;

		[Desc("When the unit land on these terrain and their movement speed is 0 on there, we destroy this unit.")]
		public readonly string[] DeathTerrainTypes = { "Rock", "Debris", "Cliffs", "Water", "River" };

		[Desc("DamageTypes(s) that trigger the explosion.")]
		public readonly BitSet<DamageType> DamageTypes = default;

		[GrantedConditionReference]
		[Desc("The condition to grant to self while airborne.")]
		public readonly string AirborneCondition = null;

		[GrantedConditionReference]
		[Desc("The condition to grant to self while at \"cruise\" altitude.")]
		public readonly string CruisingCondition = null;

		public override object Create(ActorInitializer init) { return new Tractable(init.Self, this); }
	}

	public class Tractable : PausableConditionalTrait<TractableInfo>
	{
		public Actor Tractor { get; private set; }

		int airborneToken = Actor.InvalidConditionToken;
		int tractingToken = Actor.InvalidConditionToken;
		int cruisingToken = Actor.InvalidConditionToken;
		Actor self;
		IPositionable positionable;
		IOccupySpace ios;

		bool airborne = false;
		bool cruising = false;

		public Tractable(Actor self, TractableInfo info)
			: base(info)
		{
			this.self = self;
		}

		protected override void Created(Actor self)
		{
			positionable = self.TraitOrDefault<IPositionable>();
			ios = self.TraitOrDefault<IOccupySpace>();
			base.Created(self);
		}

		public int CalcAltitudeDelta(Actor self, WDist altitude, WDist targetAltitude)
		{
			if (altitude == targetAltitude)
				return 0;

			var delta = Info.AltitudeVelocity.Length;
			var dz = (targetAltitude - altitude).Length.Clamp(-delta, delta);

			return dz;
		}

		// CnP from Aircraft.cs + modified a little
		public void SetPosition(Actor self, WPos pos)
		{
			positionable.SetPosition(self, pos);

			if (!self.IsInWorld)
				return;

			self.World.UpdateMaps(self, ios);

			var altitude = self.World.Map.DistanceAboveTerrain(pos);
			var isAirborne = altitude.Length >= Info.MinAirborneAltitude;

			if (isAirborne && !airborne)
				OnAirborneAltitudeReached(self);
			else if (!isAirborne && airborne)
				OnAirborneAltitudeLeft(self);

			var isCruising = altitude == Info.CruiseAltitude;

			if (isCruising && !cruising)
				OnCruisingAltitudeReached(self);
			else if (!isCruising && cruising)
				OnCruisingAltitudeLeft(self);
		}

		public void Tract(Actor self, Actor tractor, int cruiseSpeedMultiplier)
		{
			if (this.self != self)
				return;

			if ((Tractor != null && Tractor != tractor) || IsTraitPaused)
			{
				return;
			}

			self.CancelActivity();

			GrantTractingCondition(self, tractor);

			self.QueueActivity(new TractorLift(self, tractor));
		}

		#region altitudes
		public void GrantTractingCondition(Actor self, Actor tractor)
		{
			if (tractingToken != Actor.InvalidConditionToken)
				return;

			tractingToken = self.GrantCondition(Info.TractingCondition);
			Tractor = tractor;
		}

		public void RevokeTractingCondition(Actor self)
		{
			if (tractingToken == Actor.InvalidConditionToken)
				return;

			tractingToken = self.RevokeCondition(tractingToken);
			Tractor = null;
		}

		// CnP from Aircraft.cs
		void OnAirborneAltitudeReached(Actor self)
		{
			if (airborne)
				return;

			airborne = true;

			if (airborneToken == Actor.InvalidConditionToken)
				airborneToken = self.GrantCondition(Info.AirborneCondition);
		}

		// CnP from Aircraft.cs
		void OnAirborneAltitudeLeft(Actor self)
		{
			if (!airborne)
				return;

			airborne = false;

			if (airborneToken != Actor.InvalidConditionToken)
				airborneToken = self.RevokeCondition(airborneToken);
		}

		// CnP from Aircraft.cs
		void OnCruisingAltitudeReached(Actor self)
		{
			if (cruising)
				return;

			cruising = true;

			if (cruisingToken == Actor.InvalidConditionToken)
				cruisingToken = self.GrantCondition(Info.CruisingCondition);
		}

		// CnP from Aircraft.cs
		void OnCruisingAltitudeLeft(Actor self)
		{
			if (!cruising)
				return;

			cruising = false;

			if (cruisingToken != Actor.InvalidConditionToken)
				cruisingToken = self.RevokeCondition(cruisingToken);
		}
		#endregion
	}
}
