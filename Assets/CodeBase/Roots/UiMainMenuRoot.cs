using GameCore;
using SceneInjection;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace Roots
{
	public sealed class UiMainMenuRoot : ASceneRoot
	{
		[SerializeField] private UIDocument _document;
		
		private const string k_mainMenu = "mainMenu";
		private const string k_hideLeft = "translate-hided-left";
		private const string k_play = "btn_play";
		private const string k_exit = "btn_exit";
		private const string k_settings = "btn_settings";

		private VisualElement _mainMenu;
		private Button _btnPlay;
		private Button _btnSettings;
		private Button _btnExit;
		private GameSettingsController _settingsController;

		public override void Go()
		{
			base.Go();
			_mainMenu = _document.GetVisualElement(k_mainMenu);
			_settingsController = new GameSettingsController(_document, ShowMainMenu);

			_btnPlay = _mainMenu.GetButton(k_play);
			_btnExit = _mainMenu.GetButton(k_exit);
			_btnSettings = _mainMenu.GetButton(k_settings);

			_btnPlay.RegisterCallback(new EventCallback<ClickEvent>(LoadGame));
			_btnExit.RegisterCallback(new EventCallback<ClickEvent>(ExitGame));
			_btnSettings.RegisterCallback(new EventCallback<ClickEvent>(GoToSettings));
		}

		private void OnDisable() =>
			_settingsController.Dispose();

		private void ShowMainMenu() =>
			_mainMenu.RemoveFromClassList(k_hideLeft);

		private void UnregisterCallbacks()
		{
			_btnPlay.UnregisterCallback(new EventCallback<ClickEvent>(LoadGame));
			_btnExit.UnregisterCallback(new EventCallback<ClickEvent>(ExitGame));
			_btnSettings.UnregisterCallback(new EventCallback<ClickEvent>(GoToSettings));
		}

		private async void LoadGame(ClickEvent evt)
		{
			UnregisterCallbacks();
			await SceneManagerInstance.StartNewScene<GameRoot>();
		}

		private void GoToSettings(ClickEvent evt)
		{
			_mainMenu.AddToClassList(k_hideLeft);
			_settingsController.Show();
		}

		private void ExitGame(ClickEvent evt) =>
			Game.Quit();
	}
}