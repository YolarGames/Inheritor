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

		public override void Go()
		{
			base.Go();

			_gameMenuRoot = _document.GetVisualElement(k_gameMenuRoot);
			_gameMenu = _document.GetVisualElement(k_gameMenu);

			_showHideHandler = new ShowHideHandler(_gameMenuRoot, this, OnToggle);
			_settingsController = new GameSettingsController(_document, ShowGameMenu);

			_btnContinue = _gameMenu.GetButton(k_continueButton);
			_btnSettings = _gameMenu.GetButton(k_settingsButton);
			_btnMainMenu = _gameMenu.GetButton(k_mainMenuButton);

			_btnContinue.RegisterClickEvent(HideGameMenu);
			_btnSettings.RegisterClickEvent(ShowSettings);
			_btnMainMenu.RegisterClickEvent(GoToMainMenu);

			PlayerInputEvents.OnBackPressed += _showHideHandler.Toggle;
		}

		private void OnToggle(bool isShown) =>
			Game.PauseGame(isShown);

		private void OnDisable()
		{
			_settingsController.Dispose();
			UnregisterCallbacks();
		}

		private void UnregisterCallbacks()
		{
			PlayerInputEvents.OnBackPressed -= _showHideHandler.Toggle;

			_btnContinue.UnregisterClickEvent(HideGameMenu);
			_btnSettings.UnregisterClickEvent(ShowSettings);
			_btnMainMenu.UnregisterClickEvent(GoToMainMenu);
		}

		private void ShowGameMenu() =>
			_gameMenu.RemoveFromClassList(k_hideLeft);

		private void HideGameMenu(ClickEvent evt) =>
			_showHideHandler.Toggle();

		private void ShowSettings(ClickEvent evt)
		{
			_gameMenu.AddToClassList(k_hideLeft);
			_settingsController.Show();
		}

		private async void GoToMainMenu(ClickEvent evt)
		{
			UnregisterCallbacks();
			await SceneManagerInstance.StartNewScene<UiMainMenuRoot>();
			Game.PauseGame(false);
		}
	}
}