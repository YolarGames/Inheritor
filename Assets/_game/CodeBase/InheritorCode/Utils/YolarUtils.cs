using UnityEngine;

namespace InheritorCode.Utils
{
	public static class YolarUtils
	{
		public static class Sound
		{
			/// <summary>
			///    Convert from the linear UI scale (0 to 1) tp logarithmic AudioMixer scale (-80dB to 0dB)
			/// </summary>
			/// <param name="linearVolume"></param>
			/// <returns></returns>
			public static float ConvertLinearToDecibel(float linearVolume) =>
				Mathf.Log10(Mathf.Max(0.0001f, linearVolume)) * 20.0f;

			/// <summary>
			///    Convert from the logarithmic AudioMixer scale (-80dB to 0dB) to linear UI scale (0 to 1)
			/// </summary>
			/// <param name="decibelVolume"></param>
			/// <returns></returns>
			public static float ConvertDecibelToLinear(float decibelVolume) =>
				Mathf.Pow(10, decibelVolume / 20.0f);
		}

		public static class Transform
		{
			/// <summary>
			///	Handles 2D space rotation of an object using Quaternion
			/// </summary>
			/// <param name="objectPos">
			///	Position of an object rotation should applied to
			/// </param>
			/// <param name="targetPos">
			///	Position of the point object rotating to
			/// </param>
			/// <param name="offset">
			///	Offset of rotation in degrees
			/// </param>
			/// <returns></returns>
			public static Quaternion Rotate(Vector2 objectPos, Vector2 targetPos, float offset = 0f)
			{
				Vector2 direction = (targetPos - objectPos).normalized;

				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				return Quaternion.Euler(new Vector3(0, 0, angle - offset));
			}
		}
	}
}