using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyeRite.Model.Palettes
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
