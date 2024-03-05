using UnityEngine;

namespace InheritorCode
{
	public sealed class Timer
	{
		private readonly float _time;
		private float _timeOut;

		public bool IsReady => Time.time - _timeOut >= 0;

		public Timer(float time) =>
			_time = time;

		public void Reset() =>
			_timeOut = Time.time + _time;
	}
}