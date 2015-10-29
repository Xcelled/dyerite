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

		public byte[,] Data { get; }

		protected Palette(string name, byte[,] data)
		{
			Name = name;

			Width = data.GetLength(1) / 4;
			Height = data.GetLength(0);

			Data = data;
		}

		public unsafe Bitmap ToImage()
		{
			var tmpData = new byte[Height, Width * 4];
			Buffer.BlockCopy(Data, 0, tmpData, 0, Data.Length);

			fixed (byte* d = &tmpData[0, 0])
			{
				for (var i = 0; i < tmpData.Length; i += 4)
				{
					var tmp = d[i];
					d[i] = d[i + 2];
					d[i + 2] = tmp;
				}

				return new Bitmap(Width, Height, Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, (IntPtr)d);
			}
		}
	}
}
