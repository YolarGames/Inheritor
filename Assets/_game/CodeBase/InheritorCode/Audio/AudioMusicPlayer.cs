using InheritorCode.GameCore.GameServices;
using UnityEngine;

namespace InheritorCode.Audio
{
	public sealed class AudioMusicPlayer
	{
		private readonly IAudioService _audioService = ServiceLocator.Container.GetService<AudioService>();
		private readonly AudioClip _music;

		public AudioMusicPlayer(AudioClip music) =>
			_music = music;

		public void Play() =>
			_audioService.PlayMusic(_music);
	}
}