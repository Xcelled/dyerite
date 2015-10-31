using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyeRite.Model.Pickers
{
	/// <summary>
	/// Collection of offsets (cursors). The picker can be moved around the palette.
	/// </summary>
	public class Picker
	{
		private readonly List<Point> _cursors;

		public IReadOnlyList<Point> Cursors { get; }
		public Point FirstCursor { get; }

		public Picker(IEnumerable<Point> cursors)
		{
			_cursors = new List<Point>(cursors);
			Cursors = _cursors.AsReadOnly();
			FirstCursor = _cursors.First();
		}

		/// <summary>
		/// Returns the cursors positions if the picker is positioned at the specified points.
		/// </summary>
		/// <param name="x">The x position of the picker.</param>
		/// <param name="y">The y position of the picker.</param>
		/// <param name="paletteWidth"></param>
		/// <param name="paletteHeight"></param>
		/// <returns>Enumerable of the cursor positions on the palette.</returns>
		public IEnumerable<Point> GetCursors(int x, int y, int paletteWidth, int paletteHeight)
		{
			return _cursors.Select(c => c.TileOffset(x, y, paletteWidth, paletteHeight));
		}
	}
}
