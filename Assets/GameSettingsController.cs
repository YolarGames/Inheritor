using System;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public sealed class GameSettingsController : IDisposable
{
	private const string k_hideRight = "translate-hided-right";
	private const string k_settings = "settingsMenu";
	private const string k_musicSlider = "sli_music";
	private const string k_sfxSlider = "sli_sfx";
	private const string k_back = "btn_back";
	private const string AUDIO_SETTINGS_PATH = "audioSettings";

	private readonly VisualElement _settings;
	private readonly Slider _musicSlider;
	private readonly Slider _sfxSlider;
	private readonly Button _back;
	private readonly Action _onSettingsHide;
	private AudioSettings _audioSettings;

	public GameSettingsController(UIDocument document, Action onSettingsHide)
	{
		if (!document)
			Debug.LogError("UI Document must be provided");

		_onSettingsHide = onSettingsHide;
		_settings = document.GetVisualElement(k_settings);

		if (_settings == null)
			Debug.LogError("cant find Settings visual element");

		_musicSlider = _settings.GetVisualElement(k_musicSlider).GetSlider();
		_sfxSlider = _settings.GetVisualElement(k_sfxSlider).GetSlider();
		_back = _settings.GetButton(k_back);

		_audioSettings = LoadAudioSettings();

		_musicSlider.value = _audioSettings.MusicVolume;
		_sfxSlider.value = _audioSettings.SfxVolume;

		_musicSlider.RegisterValueChangedCallback(OnMusicChanged);
		_sfxSlider.RegisterValueChangedCallback(OnSfxChanged);
		_back.RegisterCallback<ClickEvent>(Hide);
	}

	private void OnMusicChanged(ChangeEvent<float> evt) =>
		_audioSettings.MusicVolume = evt.newValue;

	private void OnSfxChanged(ChangeEvent<float> evt) =>
		_audioSettings.SfxVolume = evt.newValue;

	public void Show() =>
		_settings.RemoveFromClassList(k_hideRight);

	public void Dispose()
	{
		_musicSlider.UnregisterValueChangedCallback(OnMusicChanged);
		_sfxSlider.UnregisterValueChangedCallback(OnSfxChanged);
		_back.UnregisterCallback<ClickEvent>(Hide);
	}

	private void Hide(ClickEvent evt)
	{
		SaveAudioSettings();
		_settings.AddToClassList(k_hideRight);
		_onSettingsHide?.Invoke();
	}

	private void SaveAudioSettings()
	{
		PlayerPrefs.SetString(AUDIO_SETTINGS_PATH, JsonUtility.ToJson(_audioSettings));
		PlayerPrefs.Save();
	}

	private AudioSettings LoadAudioSettings()
	{
		string audioSettingsString = PlayerPrefs.GetString(AUDIO_SETTINGS_PATH);

		return string.IsNullOrEmpty(audioSettingsString)
			? new AudioSettings()
			: JsonUtility.FromJson<AudioSettings>(audioSettingsString);
	}
}

[Serializable]
public struct AudioSettings
{
	[Range(0f, 1f)] public float MusicVolume;
	[Range(0f, 1f)] public float SfxVolume;

	public AudioSettings(float musicVolume = 0.5f, float sfxVolume = 0.5f)
	{
		MusicVolume = musicVolume;
		SfxVolume = sfxVolume;
	}
}