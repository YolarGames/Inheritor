using System;
using InheritorCode.Audio;
using InheritorCode.GameCore.Firebase;
using InheritorCode.GameCore.GameServices;
using InheritorCode.Roots;
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
			_firebaseService.AuthWithGooglePlay();
			_sfxPlayer = new AudioSfxPlayer();
			_audioSettings = new AudioSettings();
			_musicSlider.value = _audioSettings.MusicVolume;
			_sfxSlider.value = _audioSettings.SfxVolume;

			RegisterCallbacks();
		}

		public void Show()
		{
			SetEmailAuthColor();
			SetGoogleAuthColor();
			SetFacebookAuthColor();

			_settings.RemoveFromClassList(k_hideRight);
		}

		public void Hide()
		{
			_audioSettings.Save();
			ResetSubmenus();
			_settings.AddToClassList(k_hideRight);
			_onSettingsHide?.Invoke();
		}

		public void Dispose()
		{
			_authGoogleButton.UnregisterClickEvent(OnAuthGoogleClick);
			_authFacebookButton.UnregisterClickEvent(OnAuthFacebookClick);
			_authEmailButton.UnregisterClickEvent(OnAuthMailClick);
			_loginButton.UnregisterClickEvent(OnLoginClick);
			_registerButton.UnregisterClickEvent(OnRegisterClick);
			_backButton.UnregisterClickEvent(OnBackClick);

			if (Adaptation.IsDesktop)
			{
				_authGoogleButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
				_authFacebookButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
				_authEmailButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
				_loginButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
				_registerButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
				_backButton.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			}

			_musicSlider.UnregisterValueChangedCallback(OnMusicChanged);
			_sfxSlider.UnregisterValueChangedCallback(OnSfxChanged);
		}

		private void RegisterCallbacks()
		{
			_authGoogleButton.RegisterClickEvent(OnAuthGoogleClick);
			_authFacebookButton.RegisterClickEvent(OnAuthFacebookClick);
			_authEmailButton.RegisterClickEvent(OnAuthMailClick);
			_loginButton.RegisterClickEvent(OnLoginClick);
			_registerButton.RegisterClickEvent(OnRegisterClick);
			_backButton.RegisterClickEvent(OnBackClick);

			if (Adaptation.IsDesktop)
			{
				_authGoogleButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
				_authFacebookButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
				_authEmailButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
				_loginButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
				_registerButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
				_backButton.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			}

			_musicSlider.RegisterValueChangedCallback(OnMusicChanged);
			_sfxSlider.RegisterValueChangedCallback(OnSfxChanged);
		}

		private async void OnAuthGoogleClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			await _firebaseService.AuthWithGooglePlay();
			SetGoogleAuthColor();
			ResetSubmenus();
		}

		private async void OnAuthFacebookClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			await _firebaseService.AuthWithFacebook();
			SetFacebookAuthColor();
			ResetSubmenus();
		}

		private void OnAuthMailClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			ToggleSubmenus();
		}

		private async void OnLoginClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			await _firebaseService.SignInWithEmailAndPassword(_emailField.value, _passwordField.value);

			if (_firebaseService.IsUserLoggedWithEmail)
				ResetSubmenus();

			SetEmailAuthColor();
		}

		private async void OnRegisterClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			await _firebaseService.CreateUserWithEmailAndPassword(_emailField.value, _passwordField.value);

			if (_firebaseService.IsUserLoggedWithEmail)
				ResetSubmenus();

			SetEmailAuthColor();
		}

		private void OnBackClick(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			Hide();
		}

		private void OnMusicChanged(ChangeEvent<float> evt) =>
			_audioSettings.SetMusicVolume(Mathf.Clamp(evt.newValue, 0f, 1f));

		private void OnSfxChanged(ChangeEvent<float> evt) =>
			_audioSettings.SetSfxVolume(Mathf.Clamp(evt.newValue, 0f, 1f));

		private void SetGoogleAuthColor() =>
			_authGoogleButton.style.unityBackgroundImageTintColor =
				_firebaseService.IsUserLoggedWithGooglePlay ? Color.green : Color.white;

		private void SetEmailAuthColor() =>
			_authEmailButton.style.unityBackgroundImageTintColor =
				_firebaseService.IsUserLoggedWithEmail ? Color.green : Color.white;

		private void SetFacebookAuthColor() =>
			_authFacebookButton.style.unityBackgroundImageTintColor =
				_firebaseService.IsUserLoggedWithFacebook ? Color.green : Color.white;

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