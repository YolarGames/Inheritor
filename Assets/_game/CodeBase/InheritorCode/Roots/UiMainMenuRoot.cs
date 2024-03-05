using InheritorCode.Audio;
using InheritorCode.GameCore;
using InheritorCode.SceneInjection;
using InheritorCode.UI;
using InheritorCode.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace InheritorCode.Roots
{
	public sealed class UiMainMenuRoot : ASceneRoot
	{
		[SerializeField] private UIDocument _document;
		[SerializeField] private AudioClip _bgMusic;

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
		private AudioSfxPlayer _sfxPlayer;

		public override void Go()
		{
			base.Go();
			var musicPlayer = new AudioMusicPlayer(_bgMusic);
			musicPlayer.Play();
			_sfxPlayer = new AudioSfxPlayer();

			_mainMenu = _document.GetVisualElement(k_mainMenu);
			_settingsController = new GameSettingsController(_document, ShowMainMenu);

			_btnPlay = _mainMenu.GetButton(k_play);
			_btnExit = _mainMenu.GetButton(k_exit);
			_btnSettings = _mainMenu.GetButton(k_settings);

			RegisterCallbacks();
		}

		private void RegisterCallbacks()
		{
			_btnPlay.RegisterClickEvent(LoadGame);
			_btnExit.RegisterClickEvent(ExitGame);
			_btnSettings.RegisterClickEvent(GoToSettings);

			_btnPlay.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_btnExit.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_btnSettings.RegisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
		}

		private void OnDisable() =>
			_settingsController?.Dispose();

		private void ShowMainMenu() =>
			_mainMenu.RemoveFromClassList(k_hideLeft);

		private void UnregisterCallbacks()
		{
			_btnPlay.UnregisterCallback(new EventCallback<ClickEvent>(LoadGame));
			_btnExit.UnregisterCallback(new EventCallback<ClickEvent>(ExitGame));
			_btnSettings.UnregisterCallback(new EventCallback<ClickEvent>(GoToSettings));

			_btnPlay.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_btnExit.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
			_btnSettings.UnregisterMouseEnterEvent(_sfxPlayer.PlaySelectButton);
		}

		private async void LoadGame(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			UnregisterCallbacks();
			await SceneManagerInstance.StartNewScene<GameRoot>();
		}

		private void GoToSettings(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			_mainMenu.AddToClassList(k_hideLeft);
			_settingsController.Show();
		}

		private void ExitGame(ClickEvent evt)
		{
			_sfxPlayer.PlayClickButton();
			Game.Quit();
		}
	}
}