namespace DyeRite.Model.Distortion
{
	public class DistortionParameters
	{
		public int Number { get; }

		public int Type { get; }
		public int DistortMapId { get; }
		public double Rate { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DistortionParameters"/> class.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="type">The type.</param>
		/// <param name="distortMapId">The distort map identifier.</param>
		/// <param name="rate">The rate.</param>
		public DistortionParameters(int number, int type, int distortMapId, double rate)
		{
			Number = number;
			Type = type;
			DistortMapId = distortMapId;
			Rate = rate;
		}
	}
}
