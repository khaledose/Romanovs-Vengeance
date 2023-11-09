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

using OpenRA.Graphics;
using OpenRA.Primitives;
using System;

namespace OpenRA.Mods.RA2.Graphics
{
	public class ChargingRingRenderable : IRenderable, IFinalizedRenderable
	{
		const int CircleSegments = 32;
		readonly WVec[] FacingOffsets; //= Exts.MakeArray(CircleSegments, i => new WVec(0, 1024, 0).Rotate(WRot.FromFacing(i * 256 / CircleSegments)));
		readonly WPos target;
		readonly WDist radius;
		readonly WDist width;
		readonly Color color;
		readonly double take;
		readonly double skip;

		public ChargingRingRenderable(WPos centerPosition, WPos target, WDist radius, WDist width, Color color, double take, double skip)
		 {
			Pos = centerPosition;
			this.radius = radius;
			this.width = width;
			this.color = color;
			this.take = take;
			this.skip = skip;
			this.target = target;
		}

		public WPos Pos { get; }
		public int ZOffset => 0;
		public bool IsDecoration => true;

		public IRenderable WithZOffset(int newOffset) { return new ChargingRingRenderable(Pos, target, radius, width, color, take, skip); }
		public IRenderable OffsetBy(in WVec vec) { return new ChargingRingRenderable(Pos + vec, target, radius, width, color, take, skip); }
		public IRenderable AsDecoration() { return this; }

		public IFinalizedRenderable PrepareRender(WorldRenderer wr) { return this; }

		public WVec CalculatePoint()
		{
			var r = radius.Length;
			// Circle information
			double h = 0;  // Circle center x-coordinate
			double k = 0;  // Circle center y-coordinate

			// Convert given point to cartesian space
			double x = target.X - Pos.X; // Given point x-coordinate
			double y = Pos.Y - target.Y; // Given point y-coordinate

			// Calculate slope of the dividing line
			double m = (y - k) / (x - h);
			var b = y - m * x;

			var A = 1 + m * m;
			var B = -2 * h;
			var C = h * h - r * r;

			var x1 = Math.Round((-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A));
			var x2 = Math.Round((-B - Math.Sqrt(B * B - 4 * A * C)) / (2 * A));

			var y1 = Math.Round(m * x1 + b);
			var y2 = Math.Round(m * x2 + b);

			// Calculate distance between both points and the target point
			var d1 = Math.Sqrt(Math.Pow(x - x1, 2) + Math.Pow(y - y1, 2) + Math.Pow(Pos.Z, 2));
			var d2 = Math.Sqrt(Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2) + Math.Pow(Pos.Z, 2));

			// Return the point that is closer to the target point

			return d1 >= d2 ? new WVec((int)x1, (int)y2, 0) : new WVec((int)x2, (int)y1, 0);
		}

		public void Render(WorldRenderer wr)
		{
			var r = radius.Length;
			var chargePoint = CalculatePoint();

			var firstHalf = Exts.MakeArray(CircleSegments / 2, i => chargePoint.Rotate(WRot.FromFacing(i * 256 / CircleSegments)));
			var secondHalf = Exts.MakeArray(CircleSegments/2, i => chargePoint.Rotate(WRot.FromFacing(-i * 256 / CircleSegments)));

			var start = Math.Min((int)(CircleSegments * skip), (CircleSegments / 2) - 1);
			var a1 = wr.Screen3DPosition(Pos + r * firstHalf[start] / 1024);
			var a2 = wr.Screen3DPosition(Pos + r * secondHalf[start] / 1024);

			int segsCount = Math.Min(CircleSegments / 2, (int)(take * CircleSegments / 2));
			for (var i = start + 1; i < segsCount; i++)
			{
				var b1 = wr.Screen3DPosition(Pos + r * firstHalf[i] / 1024);
				Game.Renderer.WorldRgbaColorRenderer.DrawLine(a1, b1, width.Length, color);

				var b2 = wr.Screen3DPosition(Pos + r * secondHalf[i] / 1024);
				Game.Renderer.WorldRgbaColorRenderer.DrawLine(a2, b2, width.Length, color);
				a1 = b1;
				a2 = b2;
			}

			if (take == 1)
			{
				Game.Renderer.WorldRgbaColorRenderer.DrawLine(a1, a2, width.Length, color);
			}

			if (take > 1)
			{
				var b = wr.Screen3DPosition(target);
				Game.Renderer.WorldRgbaColorRenderer.DrawLine(a1, b, width.Length, color);
			}
		}

		public void RenderDebugGeometry(WorldRenderer wr) { }

		public Rectangle ScreenBounds(WorldRenderer wr) { return Rectangle.Empty; }
	}
}
