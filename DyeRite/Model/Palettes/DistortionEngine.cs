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
	/// <summary>
	/// An engine for applying distortion transforms to a palette.
	/// </summary>
	public class DistortionEngine
	{
		private const int BytesPerPixel = 4;
		private readonly List<DistortionMap> _maps;

		public DistortionEngine(IEnumerable<DistortionMap> maps)
		{
			_maps = new List<DistortionMap>(maps);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int DistortionFunction(double rate, int index, int indexOffset, byte[] map)
		{
			return (int)(rate * map[(index + indexOffset) % map.Length]);
		}

		private byte[] FindMap(int id)
		{
			return _maps.First(m => m.MapId == id).Data;
		}

		public byte[] Distort(RawPalette palette, int offset, int outWidth, int outHeight)
		{
			var iter = palette.Distortions.GetEnumerator();

			if (!iter.MoveNext())
			{
				return Distort(palette.Data, palette.Width, outWidth, outHeight, a => 0, a => 0);
			}

			var dist = iter.Current;

			var map = dist.DistortMapId;
			var type = dist.Type;

			var output = Distort(palette.Data, dist.Rate, palette.Width, offset, outWidth, outHeight, map, type);

			while (iter.MoveNext())
			{
				dist = iter.Current;
				var input = output;

				map = dist.DistortMapId;
				type = dist.Type;

				output = Distort(input, dist.Rate, outWidth, offset, outWidth, outHeight, map, type);
			}

			return output;
		}

		private byte[] Distort(byte[] input, double rate, int inWidth, int offset, int outWidth, int outHeight, int mapId, int type)
		{
			if (type == 0 || mapId == 0)
				return input;

			var map = FindMap(mapId);

			Func<int, int> distortFunc = d => DistortionFunction(rate, d, offset, map);

			const int horizDistortion = 1;
			const int verticalDistortion = 2;

			if (type == horizDistortion)
				return Distort(input, inWidth, outWidth, outHeight, a => distortFunc(a), a => 0);
			if (type == verticalDistortion)
				return Distort(input, inWidth, outWidth, outHeight, a => 0, a => distortFunc(a));

			throw new Exception("Unsupported distort type");
		}

		private static unsafe byte[] Distort(byte[] input, int inWidth, int outWidth, int outHeight, Func<int, int> xDistort, Func<int, int> yDistort)
		{
			if (input.Length % BytesPerPixel != 0)
				throw new ArgumentException();

			var inStride = inWidth * BytesPerPixel;
			var outStride = outWidth * BytesPerPixel;

			var output = new byte[outWidth * outHeight * BytesPerPixel];

			fixed (byte* inPtr = &input[0])
			fixed (byte* outPtr = &output[0])
			for (var y = 0; y < outHeight; y++)
			{
				for (var x = 0; x < outWidth; x++)
				{
					var dx = (x + xDistort(y)) % outWidth;
					var dy = (y + yDistort(x)) % outHeight;

					var intmp = inPtr + ((y * inStride) + (x * BytesPerPixel)) % input.Length;
					var outtmp = outPtr + (dy * outStride) + (dx * BytesPerPixel);

					*((int*)outtmp) = *((int*)intmp);
				}
			}

			return output;
		}
	}
}
