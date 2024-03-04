using System;
using GameCore.GameServices;
using UnityEngine;

namespace Audio
{
	[Serializable]
	public class AudioSettings
	{
		private const string SETTINGS_PREFS_PATH = "audioSettings";

		private IAudioService _audioService;
		private AudioSettingsSnapshot _settingsSnapshot;
		public float MusicVolume => _settingsSnapshot.MusicVolume;
		public float SfxVolume => _settingsSnapshot.SfxVolume;

		public AudioSettings(float musicVolume = 0.4f, float sfxVolume = 0.5f)
		{
			_audioService = ServiceLocator.Container.GetService<AudioService>();

			if (HaveSavedSettings())
				_settingsSnapshot = LoadSavedSettings();
			else
			{
				_settingsSnapshot.MusicVolume = musicVolume;
				_settingsSnapshot.SfxVolume = sfxVolume;
			}

			_audioService.SetMusicVolume(_settingsSnapshot.MusicVolume);
			_audioService.SetSfxVolume(_settingsSnapshot.SfxVolume);
		}

		public void SetMusicVolume(float value)
		{
			_settingsSnapshot.MusicVolume = value;
			_audioService.SetMusicVolume(value);
		}

		public void SetSfxVolume(float value)
		{
			_settingsSnapshot.SfxVolume = value;
			_audioService.SetSfxVolume(value);
		}

		public void Save() =>
			PlayerPrefs.SetString(SETTINGS_PREFS_PATH, JsonUtility.ToJson(_settingsSnapshot));

		private bool HaveSavedSettings() =>
			!string.IsNullOrEmpty(PlayerPrefs.GetString(SETTINGS_PREFS_PATH));

		private AudioSettingsSnapshot LoadSavedSettings() =>
			JsonUtility.FromJson<AudioSettingsSnapshot>(PlayerPrefs.GetString(SETTINGS_PREFS_PATH));
	}
}