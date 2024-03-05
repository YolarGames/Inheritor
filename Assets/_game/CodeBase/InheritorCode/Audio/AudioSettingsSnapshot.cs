using UnityEngine;

namespace InheritorCode.Audio
{
	public struct AudioSettingsSnapshot
	{
		[Range(0f, 1f)] public float MusicVolume;
		[Range(0f, 1f)] public float SfxVolume;
	}
}