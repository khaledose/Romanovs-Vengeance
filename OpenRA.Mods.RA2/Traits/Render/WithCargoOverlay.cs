#region Copyright & License Information
/*
 * Copyright (c) The OpenRA Developers and Contributors
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using OpenRA.Graphics;
using OpenRA.Mods.Common;
using OpenRA.Mods.Common.Graphics;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.Common.Traits.Render;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits.Render
{
	[Desc("Renders the cargo loaded into the unit.")]
	public class WithCargoOverlayInfo : PausableConditionalTraitInfo, Requires<CargoInfo>, Requires<BodyOrientationInfo>, Requires<RenderSpritesInfo>
	{
		[Desc("Cargo position relative to turret or body in (forward, right, up) triples. The default offset should be in the middle of the list.")]
		public readonly WVec[] LocalOffset = { WVec.Zero };

		[Desc("Facing for the passengers to face.")]
		public readonly WAngle Facing = WAngle.Zero;

		[Desc("Passenger CargoType to display.")]
		public readonly HashSet<string> DisplayTypes = new HashSet<string>();

		[SequenceReference]
		[Desc("Sequence to use upon resupply beginning.")]
		public readonly List<string> LoadingSequences = new();

		[FieldLoader.Require]
		[SequenceReference]
		[Desc("Sequence name to play once during resupply intervals or repeatedly if a start sequence is set.")]
		public readonly List<string> LoadedSequences = new();

		[SequenceReference]
		[Desc("Sequence to use after resupplying has finished.")]
		public readonly List<string> UnloadingSequences = new();

		[Desc("Position relative to body")]
		public readonly WVec Offset = WVec.Zero;

		[PaletteReference(nameof(IsPlayerPalette))]
		[Desc("Custom palette name")]
		public readonly string Palette = null;

		[Desc("Custom palette is a player palette BaseName")]
		public readonly bool IsPlayerPalette = false;

		public override object Create(ActorInitializer init) { return new WithCargoOverlay(init.Self, this); }
	}

	public class WithCargoOverlay : PausableConditionalTrait<WithCargoOverlayInfo>, ITick, IRender, INotifyDamageStateChanged, INotifyPassengerEntered, INotifyPassengerExited
	{
		readonly Cargo cargo;
		readonly BodyOrientation body;
		readonly List<CargoOverlay> loadingOverlays;
		readonly List<CargoOverlay> loadedOverlays;

		Dictionary<Actor, IActorPreview[]> previews = new Dictionary<Actor, IActorPreview[]>();

		public WithCargoOverlay(Actor self, WithCargoOverlayInfo info) 
			: base(info)
		{
			cargo = self.Trait<Cargo>();
			body = self.Trait<BodyOrientation>();

			loadingOverlays = new List<CargoOverlay>();
			loadedOverlays = new List<CargoOverlay>();

			foreach (var sequence in Info.LoadingSequences)
			{
				loadingOverlays.Add(new CargoOverlay(self, this, sequence));
			}

			foreach (var sequence in Info.LoadedSequences)
			{
				loadedOverlays.Add(new CargoOverlay(self, this, sequence));
			}
		}

		void ITick.Tick(Actor self)
		{
			foreach (var actorPreviews in previews.Values)
			{
				if (actorPreviews != null)
				{
					foreach (var preview in actorPreviews)
					{
						preview.Tick();
					}
				}
			}
		}

		IEnumerable<IRenderable> IRender.Render(Actor self, WorldRenderer wr)
		{
			var bodyOrientation = body.QuantizeOrientation(self.Orientation);
			var pos = self.CenterPosition;
			var i = 0;

			// Generate missing previews
			var missing = previews
				.Where(kv => kv.Value == null)
				.Select(kv => kv.Key)
				.ToList();

			foreach (var p in missing)
			{
				var passengerInits = new TypeDictionary()
				{
					new OwnerInit(p.Owner),
					new FacingInit(Info.Facing),
				};

				foreach (var api in p.TraitsImplementing<IActorPreviewInitModifier>())
				{
					api.ModifyActorPreviewInit(p, passengerInits);
				}

				var init = new ActorPreviewInitializer(p.Info, wr, passengerInits);
				previews[p] = p.Info.TraitInfos<IRenderActorPreviewInfo>()
					.SelectMany(rpi => rpi.RenderPreview(init))
					.ToArray();
			}

			foreach (var actorPreviews in previews.Values)
			{
				if (actorPreviews == null)
				{
					continue;
				}

				foreach (var p in actorPreviews)
				{
					var index = cargo.PassengerCount > 1 ? i++ % Info.LocalOffset.Length : Info.LocalOffset.Length / 2;
					var localOffset = Info.LocalOffset[index];

					foreach (var pp in p.Render(wr, pos + body.LocalToWorld(localOffset.Rotate(bodyOrientation))))
						yield return pp.WithZOffset(2);
				}
			}
		}

		IEnumerable<Rectangle> IRender.ScreenBounds(Actor self, WorldRenderer wr)
		{
			var pos = self.CenterPosition;
			foreach (var actorPreviews in previews.Values)
			{
				if (actorPreviews != null)
				{
					foreach (var p in actorPreviews)
					{
						foreach (var b in p.ScreenBounds(wr, pos))
						{
							yield return b;
						}
					}
				}
			}
		}

		void INotifyDamageStateChanged.DamageStateChanged(Actor self, AttackInfo e)
		{
			foreach (var overlay in loadingOverlays)
			{
				overlay.SetDamagedState(e);
			}

			foreach (var overlay in loadedOverlays)
			{
				overlay.SetDamagedState(e);
			}
		}

		void INotifyPassengerEntered.OnPassengerEntered(Actor self, Actor passenger)
		{
			if (!Info.DisplayTypes.Contains(passenger.Trait<Passenger>().Info.CargoType))
			{
				return;
			}

			LoadPassenger(self, passenger);

			if (Info.LoadingSequences.Any())
			{
				foreach (var overlay in loadingOverlays)
				{
					int index = loadingOverlays.IndexOf(overlay);
					overlay.IsVisible = true;
					string sequence = Info.LoadingSequences[index];
					overlay.Overlay.PlayThen(RenderSprites.NormalizeSequence(overlay.Overlay, self.GetDamageState(), sequence),
						() =>
						{
							var loadedOverlay = loadedOverlays[index];
							var loadedSequence = Info.LoadedSequences[index];
							loadedOverlay.IsVisible = true;
							loadedOverlay.Overlay.PlayRepeating(RenderSprites.NormalizeSequence(loadedOverlay.Overlay, self.GetDamageState(), loadedSequence));
						});
				}

				return;
			}

			foreach (var overlay in loadedOverlays)
			{
				overlay.IsVisible = true;
				string sequence = Info.LoadedSequences[loadedOverlays.IndexOf(overlay)];
				overlay.Overlay.PlayRepeating(RenderSprites.NormalizeSequence(overlay.Overlay, self.GetDamageState(), sequence));
			}
		}

		void INotifyPassengerExited.OnPassengerExited(Actor self, Actor passenger)
		{
			foreach (var overlay in loadedOverlays)
			{
				overlay.IsVisible = false;
			}

			if (!Info.LoadingSequences.Any() && !Info.UnloadingSequences.Any())
			{
				UnloadPassenger(self, passenger);
				return;
			}

			foreach (var overlay in loadingOverlays)
			{
				overlay.IsVisible = true;
				Action unloadCargo = () =>
				{
					overlay.IsVisible = false;
					UnloadPassenger(self, passenger);
				};

				if (Info.UnloadingSequences.Any())
				{
					string sequence = Info.UnloadingSequences[loadingOverlays.IndexOf(overlay)];
					overlay.Overlay.PlayThen(RenderSprites.NormalizeSequence(overlay.Overlay, self.GetDamageState(), sequence), unloadCargo);
				}

				if (Info.LoadingSequences.Any())
				{
					string sequence = Info.LoadingSequences[loadingOverlays.IndexOf(overlay)];
					overlay.Overlay.PlayBackwardsThen(RenderSprites.NormalizeSequence(overlay.Overlay, self.GetDamageState(), sequence), unloadCargo);
				}
			}
		}

		private void LoadPassenger(Actor self, Actor passenger)
		{
			if (previews.ContainsKey(passenger))
			{
				return;
			}

			previews.Add(passenger, null);
			self.World.ScreenMap.AddOrUpdate(self);
		}

		private void UnloadPassenger(Actor self, Actor passenger)
		{
			if (!previews.ContainsKey(passenger))
			{
				return;
			}

			previews.Remove(passenger);
			self.World.ScreenMap.AddOrUpdate(self);
		}
	}

	public class CargoOverlay
	{
		public Animation Overlay { get; set; }

		public bool IsVisible { get; set; }

		public CargoOverlay(Actor self, WithCargoOverlay trait, string sequence)
		{
			IsVisible = false;

			var render = self.TraitOrDefault<RenderSprites>();
			var body = self.Trait<BodyOrientation>();

			Overlay = new Animation(self.World, render.GetImage(self), () => trait.IsTraitPaused);
			Overlay.PlayThen(sequence, () => IsVisible = false);

			var anim = new AnimationWithOffset(Overlay,
				() => body.LocalToWorld(trait.Info.Offset.Rotate(body.QuantizeOrientation(self.Orientation))),
				() => trait.IsTraitDisabled || !IsVisible,
				p => RenderUtils.ZOffsetFromCenter(self, p, 1));

			render.Add(anim, trait.Info.Palette, trait.Info.IsPlayerPalette);
		}

		public void SetDamagedState(AttackInfo e)
		{
			Overlay.ReplaceAnim(RenderSprites.NormalizeSequence(Overlay, e.DamageState, Overlay.CurrentSequence.Name));
		}
	}
}
