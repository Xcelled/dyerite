using System.Threading.Tasks;
using ColorMine.ColorSpaces;
using ColorMine.ColorSpaces.Comparisons;
using static DyeRite.PerformanceTracker;

namespace DyeRite.Model.Difference
{
	public class DifferenceEngine
	{
		private static readonly CieDe2000Comparison CieDe2000 = new CieDe2000Comparison();

		public DeltaEMap Calculate(Lab target, Lab[,] palette)
		{
			var height = palette.GetLength(0);
			var width = palette.GetLength(1);

			BeginTrack(nameof(Calculate), "Calculating DeltaE");

			var results = new double[height, width];

			Parallel.For(0, height, i =>
			{
				for (var j = 0; j < width; j++)
					results[i, j] = palette[i, j].Compare(target, CieDe2000);
			});

			EndTrack(nameof(Calculate), "Calculated DeltaE map");

			return new DeltaEMap(results);
		}
	}
}
