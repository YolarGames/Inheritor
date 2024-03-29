﻿using System.Threading.Tasks;
using Configs;
using UnityEngine;
using Utils;

namespace GameCore.GameServices
{
	public sealed class AudioService : IAudioService
	{
		private const string k_musicVolume = "musicVolume";
		private const string k_sfxVolume = "sfxVolume";

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

		public void PlaySfxClipOneShot(AudioClip clip) =>
			_sfxSource.PlayOneShot(clip);

		public void PlayMusic(AudioClip clip)
		{
			if (_musicSource.clip == clip)
				return;

			_musicSource.clip = clip;
			_musicSource.Play();
		}

		public void SetMusicVolume(float value)
		{
			Debug.Log($"SetMusicVolume {value}");
			float dbVolume = YolarUtils.Sound.ConvertLinearToDecibel(value);
			_sfxSource.outputAudioMixerGroup.audioMixer.SetFloat(k_musicVolume, dbVolume);
		}

		public void SetSfxVolume(float value)
		{
			Debug.Log($"SetSfxVolume {value}");
			float dbVolume = YolarUtils.Sound.ConvertLinearToDecibel(value);
			_sfxSource.outputAudioMixerGroup.audioMixer.SetFloat(k_sfxVolume, dbVolume);
		}

		public void PlayClickButton() =>
			PlaySfxClipOneShot(_config.ClickButtonSfx);

		public void PlaySelectButton() =>
			PlaySfxClipOneShot(_config.SelectButtonSfx);

		private (AudioSource musicSource, AudioSource sfxSource) CreateAudioSourceObject()
		{
			var obj = new GameObject("AudioListeners");
			obj.AddComponent<DontDestroyOnLoad>();
			return (
				obj.AddComponent<AudioSource>(),
				obj.AddComponent<AudioSource>());
		}
	}

	public interface IAudioService : IService
	{
		void PlaySfxClipOneShot(AudioClip clip);
		void PlayMusic(AudioClip music);
		void SetMusicVolume(float value);
		void SetSfxVolume(float value);
		void PlayClickButton();
		void PlaySelectButton();
	}
}