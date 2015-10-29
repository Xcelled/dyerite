using System;
using System.Collections.Generic;
using System.IO;

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
		public RawPalette(string name, int width, int height, byte[] data)
			: this (name, LinearTo2D(data, width * 4, height))
		{	
		}

		public RawPalette(string name, byte[,] data)
			: base(name, data)
		{
		}

		/// <summary>
		/// Loads a palette from a .raw file. (32bpp RGBA)
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="normalizeFlashy">If true, converts alphas of 0 to alphas of 0xff</param>
		/// <returns>System.Byte[].</returns>
		public static byte[] LoadRaw(string filename, bool normalizeFlashy = true)
		{
			var bytes = File.ReadAllBytes(filename);

			if (normalizeFlashy)
			{
				for (var i = 3; i < bytes.Length; i += 4)
				{
					if (bytes[i] == 0)
					{
						bytes[i] = 0xff;
					}
				}
			}

			return bytes;
		}

		private static byte[,] LinearTo2D(byte[] arr, int width, int height)
		{
			var tmp = new byte[height, width];
			Buffer.BlockCopy(arr, 0, tmp, 0, tmp.Length);
			return tmp;
		}
	}
}
