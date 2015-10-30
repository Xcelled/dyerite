using System;
using System.Collections.Generic;
using System.IO;
using DyeRite.Model.Distortion;

namespace DyeRite.Model.Palettes
{
	public class RawPalette : Palette
	{
		public List<DistortionParameters> Distortions { get; } = new List<DistortionParameters>();

		/// <summary>
		/// Initializes a new instance of the <see cref="RawPalette" /> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="data">The data.</param>
		public RawPalette(string name, int width, int height, int[] data)
			: this(name, LinearTo2D(data, width, height))
		{
		}

		public RawPalette(string name, int[,] data)
			: base(name, data)
		{
		}

		/// <summary>
		/// Loads a palette from a .raw file. (32bpp RGBA)
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="normalizeFlashy">If true, converts alphas of 0 to alphas of 0xff</param>
		/// <returns>ARGB array</returns>
		public static unsafe int[] LoadRaw(string filename, bool normalizeFlashy = true)
		{
			var bytes = File.ReadAllBytes(filename);

			fixed (byte* scan0 = &bytes[0])
			{
				// Put the bytes in BGRA
				for (var i = 0; i < bytes.Length; i += 4)
				{
					var tmp = scan0[i + 0];
					scan0[i + 0] = scan0[i + 2];
					scan0[i + 2] = tmp;

					if (normalizeFlashy)
					{
						if (bytes[i + 3] == 0)
						{
							bytes[i + 3] = 0xff;
						}
					}
				}

			}

			var output = new int[bytes.Length / 4];
			Buffer.BlockCopy(bytes, 0, output, 0, bytes.Length);

			return output;
		}

		private static int[,] LinearTo2D(int[] arr, int width, int height)
		{
			var tmp = new int[height, width];
			Buffer.BlockCopy(arr, 0, tmp, 0, tmp.Length * sizeof(int));
			return tmp;
		}
	}
}
