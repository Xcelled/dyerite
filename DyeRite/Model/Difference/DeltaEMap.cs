using System;
using System.Collections.Generic;
using System.Drawing;

namespace DyeRite.Model.Difference
{
	public class DeltaEMap
	{
		public int Width { get; }
		public int Height { get; }

		public double[,] Data { get; }

		public DeltaEMap(double[,] data)
		{
			Width = data.GetLength(1);
			Height = data.GetLength(0);
			Data = data;
		}

		public FilteredMap Filter(double tolerance)
		{
			var clamped = new bool[Height, Width];

			var index = new List<Point>(20);

			for (var y = 0; y < Height; y++)
				for (var x = 0; x < Width; x++)
				{
					if (Data[y, x] <= tolerance)
					{
						clamped[y, x] = true;
						index.Add(new Point(x, y));
					}
				}

			return new FilteredMap(clamped, index.ToArray());
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
						var percent = Math.Max(0, Math.Min(100, Data[y, x])) / 100;
						var scale = (byte)((1 - percent) * 255);
						ptr[0] = scale;
						ptr[1] = scale;
						ptr[2] = scale;
						ptr[3] = 255;
						ptr += 4;
					}

				return new Bitmap(Width, Height, Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, (IntPtr)scan0);
			}
		}
	}
}
