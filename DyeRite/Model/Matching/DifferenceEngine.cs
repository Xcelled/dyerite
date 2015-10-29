using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorMine.ColorSpaces;
using ColorMine.ColorSpaces.Comparisons;

namespace DyeRite.Model.Matching
{
	public class DifferenceEngine
	{
		private static readonly CieDe2000Comparison CieDe2000 = new CieDe2000Comparison();

		public ColorMap Calculate(Lab target, Lab[,] palette)
		{
			var height = palette.GetLength(0);
			var width = palette.GetLength(1);

			var results = new double[height, width];

			Parallel.For(0, height, i =>
			{
				for (var j = 0; j < width; j++)
					results[i, j] = Math.Max(0, Math.Min(palette[i, j].Compare(target, CieDe2000), 100));
			});

			return new ColorMap(width, height, results);
		}
	}
}
