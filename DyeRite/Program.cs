using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorMine.ColorSpaces;
using DyeRite.Model.Difference;
using DyeRite.Model.Distortion;
using DyeRite.Model.Matching;
using DyeRite.Model.Palettes;
using DyeRite.Model.Pickers;

namespace DyeRite
{
	class Program
	{
		public static void Main()
		{
			var p = new RawPalette("test", 256, 256, RawPalette.LoadRaw(@"C:\Users\Scott\Documents\Mabinogi\212\color\cloth.raw"));
			p.ToImage().Save("test.png");

			p.Distortions.Add(new DistortionParameters(1, 1, 1, .5));
			p.Distortions.Add(new DistortionParameters(1, 2, 1, .3));
			p.Distortions.Add(new DistortionParameters(1, 1, 1, .2));
			p.Distortions.Add(new DistortionParameters(1, 2, 1, .03));

			var map = new DistortionMap(1,
				File.ReadAllBytes(@"C:\Users\Scott\Documents\Mabinogi\212\color\displace\displace_2.raw"));

			var dst = new DistortionEngine(new [] { map });

			var b = dst.Distort(p, 0, 256, 256);

			b.ToImage().Save("test_dist.png");

			var diff = new DifferenceEngine();

			var cm = diff.Calculate(new Rgb {R=0, G=0, B=0}.To<Lab>(), b.LabPalette);

			cm.ToImage().Save("test_map.png");

			var fm = cm.Filter(0);

			fm.ToImage().Save("test_filter.png");

			var picker = new Picker(new[] {new Point(0, 0), new Point(-11, -11), new Point(10, -11), new Point(-11, 10), new Point(10, 10) });

			var matches = new MatchingEngine().Match(b, cm, fm, picker);

			foreach (var m in matches.OrderByDescending(m => m.NumberOfSuccess).ThenBy(m => m.Score))
				Console.WriteLine(m);
		}
	}
}
