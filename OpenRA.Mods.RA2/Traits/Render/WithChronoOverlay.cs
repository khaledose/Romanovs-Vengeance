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
using OpenRA.Mods.Common.Effects;
using OpenRA.Mods.Common.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits
{
	[Desc("Display a colored overlay when a timed condition is active.")]
	public class WithChronoOverlayInfo : ConditionalTraitInfo
	{
		[Desc("Measured in game ticks.")]
		public readonly int InitialDelay = 0;

		[PaletteReference(nameof(IsPlayerPalette))]
		public readonly string Palette = null;

		public readonly bool IsPlayerPalette = true;

		[SequenceReference(nameof(Image), allowNullImage: true)]
		[Desc("List of explosion sequences that can be used.")]
		public readonly string[] Explosions = Array.Empty<string>();

		[Desc("Image containing explosion effect sequence.")]
		public readonly string Image = "explosion";

		[Desc("Palette to use for explosion effect.")]
		public readonly string ExplosionPalette = "effect";

		public override object Create(ActorInitializer init) { return new WithChronoOverlay(this); }
	}

	public class WithChronoOverlay : ConditionalTrait<WithChronoOverlayInfo>, IRenderModifier, ITick
	{
		[Sync]
		int remainingTime;
		WithChronoOverlayInfo Info;

		public WithChronoOverlay(WithChronoOverlayInfo info)
			: base(info)
		{
			remainingTime = info.InitialDelay;
			Info = info;
		}

		IEnumerable<IRenderable> IRenderModifier.ModifyRender(Actor self, WorldRenderer wr, IEnumerable<IRenderable> r)
		{
			if (IsTraitDisabled || remainingTime > 0)
			{
				return r;
			}

			var palette = Info.IsPlayerPalette ? wr.Palette(Info.Palette + self.Owner.InternalName) : wr.Palette(Info.Palette);
			if (palette == null)
			{
				return r;
			}

			ApplyExplosions(self);

			return r.Select(a => !a.IsDecoration && a is IPalettedRenderable pr ? pr.WithPalette(palette) : a);
		}

		IEnumerable<Rectangle> IRenderModifier.ModifyScreenBounds(Actor self, WorldRenderer wr, IEnumerable<Rectangle> bounds)
		{
			return bounds;
		}

		private void ApplyExplosions(Actor self)
		{
			if (string.IsNullOrEmpty(Info.Image) || !Info.Explosions.Any() || string.IsNullOrEmpty(Info.ExplosionPalette))
			{
				return;
			}

			var world = self.World;

			// Scale numSprites, targetRadius, and chance based on numCells
			var numCells = self.OccupiesSpace.OccupiedCells().Length;
			var targetRadius = Math.Max(1, Math.Min(2, (int)(numCells * 0.25))); // radius based on number of cells
			var chance = Math.Min(50, numCells * 5); // probability of effect appearing (in percent) based on number of cells

			if (world.SharedRandom.Next(0, 100) < chance)
			{
				var explosion = Info.Explosions.RandomOrDefault(world.LocalRandom);
				var offset = WVec.FromPDF(world.SharedRandom, 2) * targetRadius; // random offset from target center
				offset += new WVec(0, 0, world.SharedRandom.Next(0, (targetRadius * 1024) + 1));
				var spritePos = self.CenterPosition + offset;

				world.AddFrameEndTask(w => w.Add(new SpriteEffect(spritePos, w, Info.Image, explosion, Info.ExplosionPalette)));
			}
		}

		public void Tick(Actor self)
		{
			if (!IsTraitDisabled && remainingTime >= 0) remainingTime--;
		}
	}
}
