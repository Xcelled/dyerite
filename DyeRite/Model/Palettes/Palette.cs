using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyeRite.Model.Palettes
{
	public abstract class Palette
	{
		public string Name { get; }

		public int Width { get; }
		public int Height { get; }

		public byte[] Data { get; }

		protected Palette(string name, int width, int height, byte[] data)
		{
			Name = name;
			Width = width;
			Height = height;
			Data = data;
		}

		public unsafe Bitmap ToImage()
		{
			for (var i = 0; i < Data.Length; i += 4)
			{
				var tmp2 = Data[i];

				Data[i] = Data[i + 2];
				Data[i + 2] = tmp2;
			}

			fixed (byte* d = &Data[0])
			{
				return new Bitmap(Width, Height, Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, (IntPtr)d);
			}
		}
	}
}
