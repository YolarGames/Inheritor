using UnityEngine;
using UnityEngine.Audio;

namespace Configs
{
	[CreateAssetMenu(fileName = "AudioServiceConfig", menuName = "Configs/Services/AudioService config")]
	public class AudioServiceConfig : ScriptableObject
	{
		public AudioMixerGroup MusicMixer;
		public AudioMixerGroup SfxMixer;
	}
}