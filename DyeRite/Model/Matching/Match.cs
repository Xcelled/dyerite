using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DyeRite.Model.Matching
{
	public class Match
	{
		public Point PickerLocation { get; }

		public IReadOnlyList<SelectedColor> Colors { get; }

		public double Score { get; }
		public int NumberOfSuccess { get; }

		public Match(IEnumerable<SelectedColor> colors, Point pickerLocation)
		{
			PickerLocation = pickerLocation;
			Colors = new List<SelectedColor>(colors);

			Score = Colors.Sum(c => c.DeltaE);
			NumberOfSuccess = Colors.Count(c => c.MeetsFilter);
		}

		public override string ToString()
		{
			return $"DeltaE: {Score}, Successes: {NumberOfSuccess}, Location: ({PickerLocation.X}, {PickerLocation.Y})";
		}
	}
}