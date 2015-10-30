using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorMine.ColorSpaces;

namespace DyeRite.Model.Palettes
{
	public class DistortedPalette : Palette
	{
		private readonly Lazy<Lab[,]> _labPalette;
		public Lab[,] LabPalette { get { return _labPalette.Value; } }

		public DistortedPalette(string name, int[,] data) : base(name, data)
		{
			_labPalette = new Lazy<Lab[,]>(ToLab, false);
		}

		private unsafe Lab[,] ToLab()
		{
			var lab = new Lab[Height, Width];

			var x = 0;
			var y = 0;

			fixed (int* scan0 = &Data[0,0])
			{
				for (var i = 0; i < Data.Length; i++)
				{
					var ptr = (byte*)scan0 + i * sizeof(int);
					lab[y, x] = (new Rgb {R = ptr[2], G = ptr[1], B = ptr[0]}).To<Lab>();

					if (++x == Width)
					{
						x = 0;
						y++;
					}
				}
			}

			return lab;
		}
	}
}
