using System.Threading.Tasks;
using Configs;
using UnityEngine;

namespace GameCore.GameServices
{
	public sealed class AudioService : IAudioService
	{
		private readonly AudioServiceConfig _config;
		private AudioSource _musicSource;
		private AudioSource _sfxSource;

		public AudioService(AudioServiceConfig audioServiceConfig) =>
			_config = audioServiceConfig;

		public Task Init()
		{
			(_musicSource, _sfxSource) = CreateAudioSourceObject();

			_musicSource.loop = true;
			_musicSource.playOnAwake = true;
			_musicSource.outputAudioMixerGroup = _config.MusicMixer;

			_sfxSource.loop = false;
			_sfxSource.playOnAwake = false;
			_sfxSource.outputAudioMixerGroup = _config.SfxMixer;

			return Task.CompletedTask;
		}

		private (AudioSource musicSource, AudioSource sfxSource) CreateAudioSourceObject()
		{
			var obj = new GameObject("AudioListeners");
			obj.AddComponent<DontDestroyOnLoad>();
			return (
				obj.AddComponent<AudioSource>(),
				obj.AddComponent<AudioSource>());
		}
	}

	public interface IAudioService : IService { }
}