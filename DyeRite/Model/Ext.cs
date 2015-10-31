using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyeRite.Model
{
	static class Ext
	{
		public static Point TileOffset(this Point p, int dx, int dy, int maxWidth, int maxHeight)
		{
			dx = dx + p.X;
			dy = dy + p.Y;

			while (dx < 0)
				dx += maxWidth;
			while (dy < 0)
				dy += maxHeight;

			return new Point(dx % maxWidth, dy % maxHeight);
		}
	}
}
