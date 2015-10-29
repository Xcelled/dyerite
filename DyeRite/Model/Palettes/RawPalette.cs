using System.Collections.Generic;

namespace DyeRite.Model.Palettes
{
	public class RawPalette
	{
		public string Name { get; }

		public int Width { get; }
		public int Height { get; }

		public byte[] Data { get; }

		public List<DistortionParameters> Distortions { get; } = new List<DistortionParameters>();

		/// <summary>
		/// Initializes a new instance of the <see cref="RawPalette" /> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="data">The data.</param>
		public RawPalette(string name, int width, int height, byte[] data)
		{
			Name = name;
			Width = width;
			Height = height;
			Data = data;
		}
	}
}
