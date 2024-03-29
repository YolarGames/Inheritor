﻿using Audio;
using GameCore;
using SceneInjection;
using SkillSystemPrototype;
using UI;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace Roots
{
	public sealed class UiGameMenuRoot : ASceneRoot
	{
		[SerializeField] private UIDocument _document;

		private const string k_gameMenuRoot = "gameMenuRoot";
		private const string k_gameMenu = "gameMenu";
		private const string k_continueButton = "btn_continue";
		private const string k_settingsButton = "btn_settings";
		private const string k_mainMenuButton = "btn_mainMenu";
		private const string k_hideLeft = "translate-hided-left";

		private VisualElement _gameMenuRoot;
		private VisualElement _gameMenu;
		private Button _btnContinue;
		private Button _btnSettings;
		private Button _btnMainMenu;
		private ShowHideHandler _showHideHandler;
		private GameSettingsController _settingsController;
		private AudioSfxPlayer _sfxPlayer;

		public override void Go()
		{
			base.Go();

			_sfxPlayer = new AudioSfxPlayer();
			_settingsController = new GameSettingsController(_document, ShowGameMenu);
			
			_gameMenuRoot = _document.GetVisualElement(k_gameMenuRoot);
			_gameMenu = _document.GetVisualElement(k_gameMenu);

			_showHideHandler = new ShowHideHandler(_gameMenuRoot, this, OnToggle);

			_btnContinue = _gameMenu.GetButton(k_continueButton);
			_btnSettings = _gameMenu.GetButton(k_settingsButton);
			_btnMainMenu = _gameMenu.GetButton(k_mainMenuButton);

			RegisterCallbacks();
		}

		private void OnDisable()
		{
			_settingsController.Dispose();
			UnregisterCallbacks();
		}

		private void OnToggle(bool isShown) =>
			Game.PauseGame(isShown);

		private void RegisterCallbacks()
		{
			_btnContinue.RegisterClickEvent(HideGameMenu);
			_btnSettings.RegisterClickEvent(ShowSettings);
			_btnMainMenu.RegisterClickEvent(GoToMainMenu);

			_btnContinue.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_btnSettings.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_btnMainMenu.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			
			PlayerInputEvents.OnBackPressed += _showHideHandler.Toggle;
		}

		private void UnregisterCallbacks()
		{
			_btnContinue.UnregisterClickEvent(HideGameMenu);
			_btnSettings.UnregisterClickEvent(ShowSettings);
			_btnMainMenu.UnregisterClickEvent(GoToMainMenu);
			
			_btnContinue.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_btnSettings.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_btnMainMenu.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			
			PlayerInputEvents.OnBackPressed -= _showHideHandler.Toggle;
		}

		private void ShowGameMenu() =>
			_gameMenu.RemoveFromClassList(k_hideLeft);

		private void HideGameMenu(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			_showHideHandler.Toggle();
		}

		private void ShowSettings(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			_gameMenu.AddToClassList(k_hideLeft);
			_settingsController.Show();
		}

		private async void GoToMainMenu(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			UnregisterCallbacks();
			await SceneManagerInstance.StartNewScene<UiMainMenuRoot>();
			Game.PauseGame(false);
		}
	}
}