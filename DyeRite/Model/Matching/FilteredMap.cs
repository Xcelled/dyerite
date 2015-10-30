using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyeRite.Model.Matching
{
	public class FilteredMap
	{
		public int Width { get; }
		public int Height { get; }

		public bool[,] Filter { get; }

		public FilteredMap(bool[,] filter)
		{
			Filter = filter;

			Width = filter.GetLength(1);
			Height = filter.GetLength(0);
		}

		public unsafe Bitmap ToImage()
		{
			var pixels = new byte[Width * Height * 4];

			fixed (byte* scan0 = &pixels[0])
			{
				var ptr = scan0;

				for (var y = 0; y < Height; y++)
					for (var x = 0; x < Width; x++)
					{
						if (Filter[y, x])
						{
							ptr[0] = 255;
							ptr[1] = 255;
							ptr[2] = 255;
						}
						ptr[3] = 255;
						ptr += 4;
					}

				return new Bitmap(Width, Height, Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, (IntPtr)scan0);
			}
		}

	}
}
