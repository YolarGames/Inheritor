using System;
using InheritorCode.Audio;
using InheritorCode.GameCore.Firebase;
using InheritorCode.GameCore.GameServices;
using InheritorCode.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using AudioSettings = InheritorCode.Audio.AudioSettings;

namespace InheritorCode.UI
{
	public sealed class GameSettingsController : IDisposable
	{
		#region UXML fields

		private const string k_hideRight = "translate-hided-right";
		private const string k_hideSubmenu = "translate-submenu-hided";
		private const string k_settingsMenu = "settingsMenu";
		private const string k_audioSubmenu = "audioSubmenu";
		private const string k_emailSubmenu = "emailSubmenu";
		private const string k_musicSlider = "sli_music";
		private const string k_sfxSlider = "sli_sfx";
		private const string k_authGoogle = "btn_google";
		private const string k_authFacebook = "btn_facebook";
		private const string k_authEmail = "btn_email";
		private const string k_emailField = "field_email";
		private const string k_passwordField = "field_password";
		private const string k_login = "btn_login";
		private const string k_register = "btn_register";
		private const string k_back = "btn_back";

		private readonly VisualElement _settings;
		private readonly VisualElement _audioSubmenu;
		private readonly VisualElement _emailSubmenu;
		private readonly Button _authGoogleButton;
		private readonly Button _authFacebookButton;
		private readonly Button _authEmailButton;
		private readonly Button _loginButton;
		private readonly Button _registerButton;
		private readonly Button _backButton;
		private readonly Slider _musicSlider;
		private readonly Slider _sfxSlider;
		private readonly TextField _emailField;
		private readonly TextField _passwordField;

		#endregion

		private readonly Action _onSettingsHide;
		private readonly AudioSettings _audioSettings;
		private readonly AudioSfxPlayer _sfxPlayer;
		private readonly IFirebaseService _firebaseService;

		public GameSettingsController(UIDocument document, Action onSettingsHide)
		{
			if (!document)
				Debug.LogError("UI Document must be provided");

			_onSettingsHide = onSettingsHide;
			_settings = document.GetVisualElement(k_settingsMenu);

			if (_settings == null)
				Debug.LogError("cant find Settings visual element");

			_audioSubmenu = _settings.GetVisualElement(k_audioSubmenu);
			_emailSubmenu = _settings.GetVisualElement(k_emailSubmenu);

			_authGoogleButton = _settings.GetButton(k_authGoogle);
			_authFacebookButton = _settings.GetButton(k_authFacebook);
			_authEmailButton = _settings.GetButton(k_authEmail);
			_loginButton = _settings.GetButton(k_login);
			_registerButton = _settings.GetButton(k_register);
			_backButton = _settings.GetButton(k_back);
			_musicSlider = _settings.GetVisualElement(k_musicSlider).GetSlider();
			_sfxSlider = _settings.GetVisualElement(k_sfxSlider).GetSlider();
			_emailField = _emailSubmenu.Q<TextField>(k_emailField);
			_passwordField = _emailSubmenu.Q<TextField>(k_passwordField);

			_firebaseService = ServiceLocator.Container.GetService<IFirebaseService>();
			_sfxPlayer = new AudioSfxPlayer();
			_audioSettings = new AudioSettings();
			_musicSlider.value = _audioSettings.MusicVolume;
			_sfxSlider.value = _audioSettings.SfxVolume;

			_authEmailButton.style.unityBackgroundImageTintColor =
				_firebaseService.IsUserLoggedInWithEmail ? Color.green : Color.white;

			RegisterCallbacks();
		}

		public void Show() =>
			_settings.RemoveFromClassList(k_hideRight);

		public void Dispose()
		{
			_authGoogleButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_authGoogleButton.UnregisterClickEvent(OnAuthGoogleClick);
			_authFacebookButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_authFacebookButton.UnregisterClickEvent(OnAuthFacebookClick);
			_authEmailButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_authEmailButton.UnregisterClickEvent(OnAuthMailClick);
			_loginButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_loginButton.UnregisterClickEvent(OnLoginClick);
			_registerButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_registerButton.UnregisterClickEvent(OnRegisterClick);
			_backButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_backButton.UnregisterClickEvent(OnBackClick);

			_musicSlider.UnregisterValueChangedCallback(OnMusicChanged);
			_sfxSlider.UnregisterValueChangedCallback(OnSfxChanged);
		}

		private void RegisterCallbacks()
		{
			_authGoogleButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_authGoogleButton.RegisterClickEvent(OnAuthGoogleClick);
			_authFacebookButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_authFacebookButton.RegisterClickEvent(OnAuthFacebookClick);
			_authEmailButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_authEmailButton.RegisterClickEvent(OnAuthMailClick);
			_loginButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_loginButton.RegisterClickEvent(OnLoginClick);
			_registerButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_registerButton.RegisterClickEvent(OnRegisterClick);
			_backButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_backButton.RegisterClickEvent(OnBackClick);

			_musicSlider.RegisterValueChangedCallback(OnMusicChanged);
			_sfxSlider.RegisterValueChangedCallback(OnSfxChanged);
		}

		private void OnAuthGoogleClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			ResetSubmenus();
		}

		private void OnAuthFacebookClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			ResetSubmenus();
		}

		private void OnAuthMailClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			ToggleSubmenus();
		}

		private void OnLoginClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			_firebaseService.SignInWithEmailAndPassword(_emailField.value, _passwordField.value);
			ResetSubmenus();
		}

		private void OnRegisterClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			_firebaseService.CreateUserWithEmailAndPassword(_emailField.value, _passwordField.value);
			ResetSubmenus();
		}

		private void OnBackClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			_audioSettings.Save();
			ResetSubmenus();
			_settings.AddToClassList(k_hideRight);
			_onSettingsHide?.Invoke();
		}

		private void OnMusicChanged(ChangeEvent<float> evt) =>
			_audioSettings.SetMusicVolume(Mathf.Clamp(evt.newValue, 0f, 1f));

		private void OnSfxChanged(ChangeEvent<float> evt) =>
			_audioSettings.SetSfxVolume(Mathf.Clamp(evt.newValue, 0f, 1f));

		private void ToggleSubmenus()
		{
			if (_audioSubmenu.ClassListContains(k_hideSubmenu))
			{
				_audioSubmenu.RemoveFromClassList(k_hideSubmenu);
				_emailSubmenu.AddToClassList(k_hideSubmenu);
			}
			else
			{
				_audioSubmenu.AddToClassList(k_hideSubmenu);
				_emailSubmenu.RemoveFromClassList(k_hideSubmenu);
			}
		}

		private void ResetSubmenus()
		{
			_audioSubmenu.RemoveFromClassList(k_hideSubmenu);
			_emailSubmenu.AddToClassList(k_hideSubmenu);
		}
	}
}