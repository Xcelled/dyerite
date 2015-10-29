using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DyeRite.Model.Matching;
using DyeRite.Model.Palettes;

namespace DyeRite
{
	class Program
	{
		public static unsafe void Main()
		{

			var p = new RawPalette("test", 256, 256, RawPalette.LoadRaw(@"C:\Users\Scott\Documents\Mabinogi\212\color\cloth.raw"));
			p.Distortions.Add(new DistortionParameters(1, 1, 1, .5));
			p.Distortions.Add(new DistortionParameters(1, 2, 1, .3));
			p.Distortions.Add(new DistortionParameters(1, 1, 1, .2));
			p.Distortions.Add(new DistortionParameters(1, 2, 1, .03));

			var map = new DistortionMap(1,
				File.ReadAllBytes(@"C:\Users\Scott\Documents\Mabinogi\212\color\displace\displace_2.raw"));

			var dst = new DistortionEngine(new [] { map });

			var b = dst.Distort(p, 0, 256, 256);

			for (var i = 0; i < b.Length; i += 4)
			{
				var tmp2 = b[i];

				b[i] = b[i + 2];
				b[i + 2] = tmp2;
			}

			fixed (byte* d = &b[0])
			{
				var bmp = new Bitmap(256, 256, 256 * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, (IntPtr)d);
				bmp.Save("test.png");
			}

			new ColorMap(2, 2, new double[2, 2] {{0, .33}, {.66, 1}}).ToImage().Save("test_map.png");
		}
	}
}
