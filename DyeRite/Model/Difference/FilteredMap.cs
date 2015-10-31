using System;
using System.Drawing;

namespace DyeRite.Model.Difference
{
	public class FilteredMap
	{
		public int Width { get; }
		public int Height { get; }

		public bool[,] Filter { get; }
		public Point[] Index { get; }

		public FilteredMap(bool[,] filter, Point[] index)
		{
			Filter = filter;
			Index = index;

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
