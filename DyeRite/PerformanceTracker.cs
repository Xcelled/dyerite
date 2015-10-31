using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyeRite
{
	static class PerformanceTracker
	{
		private static readonly Dictionary<string, Stopwatch> Timers = new Dictionary<string, Stopwatch>();

		[Conditional("DEBUG")]
		public static void BeginTrack(string id, string message)
		{
			Debug.WriteLine(message);
			Timers[id] = Stopwatch.StartNew();
		}

		[Conditional("DEBUG")]
		public static void EndTrack(string id, string message, params object[] args)
		{
			var s = Timers[id];
			s.Stop();
			Timers.Remove(id);

			Debug.WriteLine("({0:N0} ms) {1}", s.ElapsedMilliseconds, string.Format(message, args));
		}
	}
}
