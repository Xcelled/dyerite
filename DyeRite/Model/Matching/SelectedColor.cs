using System.Drawing;

namespace DyeRite.Model.Matching
{
	public class SelectedColor
	{
		public Point Location { get; }
		public Color Color { get; }
		public double DeltaE { get; }
		public bool MeetsFilter { get; }

		public SelectedColor(Point location, Color color, double deltaE, bool meetsFilter)
		{
			Location = location;
			Color = color;
			DeltaE = deltaE;
			MeetsFilter = meetsFilter;
		}
	}
}