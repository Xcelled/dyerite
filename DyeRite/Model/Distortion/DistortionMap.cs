namespace DyeRite.Model.Distortion
{
	public class DistortionMap
	{
		public int MapId { get; }
		public byte[] Data { get; }

		public DistortionMap(int mapId, byte[] data)
		{
			MapId = mapId;
			Data = data;
		}
	}
}
