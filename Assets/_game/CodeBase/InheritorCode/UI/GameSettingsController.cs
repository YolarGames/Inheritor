using System;
using InheritorCode.Audio;
using InheritorCode.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using AudioSettings = InheritorCode.Audio.AudioSettings;

namespace InheritorCode.UI
{
	public sealed class GameSettingsController : IDisposable
	{
		private const string k_hideRight = "translate-hided-right";
		private const string k_settings = "settingsMenu";
		private const string k_musicSlider = "sli_music";
		private const string k_sfxSlider = "sli_sfx";
		private const string k_authGoogle = "btn_google";
		private const string k_authFacebook = "btn_facebook";
		private const string k_authMail = "btn_mail";
		private const string k_back = "btn_back";
		private const string k_unityDragger = "unity-dragger";

		private readonly VisualElement _settings;
		private readonly Button _authGoogle;
		private readonly Button _authFacebook;
		private readonly Button _authMail;
		private readonly Slider _musicSlider;
		private readonly Slider _sfxSlider;
		private readonly Button _back;
		private readonly Action _onSettingsHide;
		private readonly AudioSettings _audioSettings;
		private readonly AudioSfxPlayer _sfxPlayer;
		private readonly VisualElement _musicDragger;
		private readonly VisualElement _sfxDragger;

		public GameSettingsController(UIDocument document, Action onSettingsHide)
		{
			if (!document)
				Debug.LogError("UI Document must be provided");

			_sfxPlayer = new AudioSfxPlayer();
			_onSettingsHide = onSettingsHide;
			_settings = document.GetVisualElement(k_settings);

			if (_settings == null)
				Debug.LogError("cant find Settings visual element");

			_authGoogle = _settings.GetButton(k_authGoogle);
			_authFacebook = _settings.GetButton(k_authFacebook);
			_authMail = _settings.GetButton(k_authMail);
			_musicSlider = _settings.GetVisualElement(k_musicSlider).GetSlider();
			_sfxSlider = _settings.GetVisualElement(k_sfxSlider).GetSlider();
			_back = _settings.GetButton(k_back);

			_musicDragger = _musicSlider.GetVisualElement(k_unityDragger);
			_sfxDragger = _sfxSlider.GetVisualElement(k_unityDragger);

			_audioSettings = new AudioSettings();
			_musicSlider.value = _audioSettings.MusicVolume;
			_sfxSlider.value = _audioSettings.SfxVolume;

			RegisterCallbacks();
		}

		public void Show() =>
			_settings.RemoveFromClassList(k_hideRight);

		public void Dispose()
		{
			_authGoogle.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_authFacebook.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_authMail.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_musicDragger.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_sfxDragger.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_back.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);

			_authGoogle.UnregisterClickEvent(_sfxPlayer.PlayClickButton);
			_authFacebook.UnregisterClickEvent(_sfxPlayer.PlayClickButton);
			_authMail.UnregisterClickEvent(_sfxPlayer.PlayClickButton);
			_back.UnregisterClickEvent(Hide);

			_musicSlider.UnregisterValueChangedCallback(OnMusicChanged);
			_sfxSlider.UnregisterValueChangedCallback(OnSfxChanged);
		}

		private void RegisterCallbacks()
		{
			_authGoogle.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_authFacebook.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_authMail.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_musicDragger.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_sfxDragger.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_back.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);

			_authGoogle.RegisterClickEvent(_sfxPlayer.PlayClickButton);
			_authFacebook.RegisterClickEvent(_sfxPlayer.PlayClickButton);
			_authMail.RegisterClickEvent(_sfxPlayer.PlayClickButton);
			_back.RegisterClickEvent(Hide);

			_musicSlider.RegisterValueChangedCallback(OnMusicChanged);
			_sfxSlider.RegisterValueChangedCallback(OnSfxChanged);
		}

		private void OnMusicChanged(ChangeEvent<float> evt) =>
			_audioSettings.SetMusicVolume(Mathf.Clamp(evt.newValue, 0f, 1f));

		private void OnSfxChanged(ChangeEvent<float> evt) =>
			_audioSettings.SetSfxVolume(Mathf.Clamp(evt.newValue, 0f, 1f));

		private void Hide(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			SaveAudioSettings();
			_settings.AddToClassList(k_hideRight);
			_onSettingsHide?.Invoke();
		}

		private void SaveAudioSettings() =>
			_audioSettings.Save();
	}
}