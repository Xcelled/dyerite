using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyeRite.Model.Matching
{
	public class ColorMap
	{
		public int Width { get; }
		public int Height { get; }

		public double[,] Data { get; }

		public ColorMap(int width, int height, double[,] data)
		{
			Width = width;
			Height = height;
			Data = data;
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
						var scale = (byte)(Data[y, x] * 255);
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
