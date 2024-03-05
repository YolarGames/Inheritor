using UnityEngine;
using UnityEngine.Audio;

namespace InheritorCode.Configs
{
	[CreateAssetMenu(fileName = "AudioServiceConfig", menuName = "Configs/Services/AudioService config")]
	public class AudioServiceConfig : ScriptableObject
	{
		public AudioMixerGroup MusicMixer;
		public AudioMixerGroup SfxMixer;
		
		[Space, Header("Sfx")]
		public AudioClip ClickButtonSfx;
		public AudioClip SelectButtonSfx;
	}
}