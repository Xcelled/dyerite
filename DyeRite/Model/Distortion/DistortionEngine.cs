using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DyeRite.Model.Palettes;
using static DyeRite.PerformanceTracker;

namespace DyeRite.Model.Distortion
{
	/// <summary>
	/// An engine for applying distortion transforms to a palette.
	/// </summary>
	public class DistortionEngine
	{
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

		public DistortedPalette Distort(RawPalette palette, int offset, int outWidth, int outHeight)
		{
			BeginTrack(nameof(Distort), "Distorting palette");

			var dist = DistortInternal(palette, offset, outWidth, outHeight);

			EndTrack(nameof(Distort), "Distorted palette");

			return new DistortedPalette(palette.Name, dist);
		}

		private int[,] DistortInternal(RawPalette palette, int offset, int outWidth, int outHeight)
		{
			var iter = palette.Distortions.GetEnumerator();

			if (!iter.MoveNext())
			{
				return DistortInternal(palette.Data, outWidth, outHeight, a => 0, a => 0);
			}

			var dist = iter.Current;

			var map = dist.DistortMapId;
			var type = dist.Type;

			var output = DistortInternal(palette.Data, dist.Rate, offset, outWidth, outHeight, map, type);

			while (iter.MoveNext())
			{
				dist = iter.Current;
				var input = output;

				map = dist.DistortMapId;
				type = dist.Type;

				output = DistortInternal(input, dist.Rate, offset, outWidth, outHeight, map, type);
			}

			return output;
		}

		/// <summary>
		/// Runs the distortion transformation.
		/// </summary>
		/// <param name="input">The input data to read</param>
		/// <param name="rate">The rate.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="outWidth">Width of the output array (in elements)</param>
		/// <param name="outHeight">Height of the input array (in elements)</param>
		/// <param name="mapId">The distortion map identifier.</param>
		/// <param name="type">The type of distortion.</param>
		/// <returns>Distorted input</returns>
		/// <exception cref="Exception">Unsupported distort type</exception>
		private int[,] DistortInternal(int[,] input, double rate, int offset, int outWidth, int outHeight, int mapId, int type)
		{
			if (type == 0 || mapId == 0)
				return input;

			var map = FindMap(mapId);

			Func<int, int> distortFunc = d => DistortionFunction(rate, d, offset, map);

			const int horizDistortion = 1;
			const int verticalDistortion = 2;

			if (type == horizDistortion)
				return DistortInternal(input, outWidth, outHeight, a => distortFunc(a), a => 0);
			if (type == verticalDistortion)
				return DistortInternal(input, outWidth, outHeight, a => 0, a => distortFunc(a));

			throw new Exception("Unsupported distort type");
		}

		/// <summary>
		/// Runs the distortion transformation.
		/// </summary>
		/// <param name="input">The input data to read</param>
		/// <param name="outWidth">Width of the output array (in elements)</param>
		/// <param name="outHeight">Height of the input array (in elements)</param>
		/// <param name="xDistort">X distortion function. Passed Y coord.</param>
		/// <param name="yDistort">Y Distortion function. Passed X coord</param>
		/// <returns>Distorted input</returns>
		private static unsafe int[,] DistortInternal(int[,] input, int outWidth, int outHeight, Func<int, int> xDistort, Func<int, int> yDistort)
		{
			var inStride = input.GetLength(1);
			var outStride = outWidth;

			var output = new int[outHeight, outWidth];

			fixed (int* inPtr = &input[0, 0])
			fixed (int* outPtr = &output[0, 0])
			for (var y = 0; y < outHeight; y++)
			{
				for (var x = 0; x < outWidth; x++)
				{
					var dx = (x + xDistort(y)) % outWidth;
					var dy = (y + yDistort(x)) % outHeight;

					outPtr[dy * outStride + dx] = inPtr[((y * inStride) + x) % input.Length];
				}
			}

			return output;
		}
	}
}
