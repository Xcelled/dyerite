using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DyeRite.Model.Palettes
{
	public class DistortionEngine
	{
		public DistortionMap Map { get; }

		public DistortionEngine(DistortionMap map)
		{
			Map = map;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int DistortionFunction(double rate, int index, int indexOffset)
		{
			return (int)(rate * Map.Data[(index + indexOffset) % Map.Data.Length]);
		}

		public byte[] Distort(RawPalette palette, int offset, int outWidth, int outHeight)
		{
			var iter = palette.Distortions.GetEnumerator();
			var output = new byte[outWidth * outHeight * 4];

			if (!iter.MoveNext())
			{
				Distort(palette.Data, output, palette.Width, outWidth, outHeight, a => 0, a => 0);
				return output;
			}

			Func<int, int> distortFunc = d => DistortionFunction(iter.Current.Rate, d, offset);

			if (iter.Current.Type == 1)
				Distort(palette.Data, output, palette.Width, outWidth, outHeight, a => distortFunc(a), a => 0);
			else
				Distort(palette.Data, output, palette.Width, outWidth, outHeight, a => 0, a => distortFunc(a));

			while (iter.MoveNext())
			{
				var input = output;
				output = new byte[input.Length];

				distortFunc = d => DistortionFunction(iter.Current.Rate, d, offset);

				if (iter.Current.Type == 1)
					Distort(input, output, outWidth, outWidth, outHeight, a => distortFunc(a), a => 0);
				else
					Distort(input, output, outWidth, outWidth, outHeight, a => 0, a => distortFunc(a));
			}

			return output;
		}

		private unsafe void Distort(byte[] input, byte[] output, int inWidth, int outWidth, int outHeight, Func<int, int> xDistort, Func<int, int> yDistort)
		{
			if (input.Length % 4 != 0 || output.Length % 4 != 0)
				throw new ArgumentException();

			var inStride = inWidth * 4;
			var outStride = outWidth * 4;

			fixed (byte* inPtr = &input[0])
			fixed (byte* outPtr = &output[0])
			for (var y = 0; y < outHeight; y++)
			{
				for (var x = 0; x < outWidth; x++)
				{
					var dx = (x + xDistort(y)) % outWidth;
					var dy = (y + yDistort(x)) % outHeight;

					var intmp = inPtr + ((y * inStride) + (x * 4)) % input.Length;
					var outtmp = outPtr + (dy * outStride) + (dx * 4);

					*((int*)outtmp) = *((int*)intmp);
				}
			}
		}
	}
}
