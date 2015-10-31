using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DyeRite.Model.Difference;
using DyeRite.Model.Palettes;
using DyeRite.Model.Pickers;
using static DyeRite.PerformanceTracker;

namespace DyeRite.Model.Matching
{
	public class MatchingEngine
	{
		public List<Match> Match(DistortedPalette palette, DeltaEMap scoreMap, FilteredMap filter, Picker picker)
		{
			var results = new ConcurrentBag<Match>();

			BeginTrack(nameof(Match), "Beginning brute force matching.");

			Parallel.ForEach(filter.Index, p =>
			{
				results.Add(DoMatch(p, palette, scoreMap, filter, picker));
			});

			EndTrack(nameof(Match), "Calculated {0:N0} matches", results.Count);

			return new List<Match>(results);
		}

		private Match DoMatch(Point index, Palette palette, DeltaEMap scoreMap, FilteredMap filter, Picker picker)
		{
			// We'll choose our centerpoint so the first picker lines up with the found color
			var zeroPoint = new Point(index.X - picker.FirstCursor.X, index.Y - picker.FirstCursor.Y);

			var selectedColors = new List<SelectedColor>();

			foreach (var cursor in picker.Cursors)
			{
				var p = zeroPoint.TileOffset(cursor.X, cursor.Y, palette.Width, palette.Height);
				selectedColors.Add(new SelectedColor(p, Color.FromArgb(palette[p.Y, p.X]), scoreMap[p.Y, p.X], filter[p.Y, p.X]));
			}

			return new Match(selectedColors, zeroPoint);
		}
	}
}
