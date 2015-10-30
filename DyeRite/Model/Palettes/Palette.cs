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

		public int[,] Data { get; }

		protected Palette(string name, int[,] data)
		{
			Name = name;

			Width = data.GetLength(1);
			Height = data.GetLength(0);

			Data = data;
		}

		public unsafe Bitmap ToImage()
		{
			var tmpData = new int[Data.Length];
			Buffer.BlockCopy(Data, 0, tmpData, 0, Data.Length * sizeof(int));

			fixed (int* d = &tmpData[0])
			{
				return new Bitmap(Width, Height, Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, (IntPtr)d);
			}
		}
	}
}
