using SceneInjection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Roots
{
	public sealed class UiMainMenuRoot : ASceneRoot
	{
		[SerializeField] private UIDocument _mainMenuDoc;

		private const string PLAY = "play";
		private const string SETTINGS = "settings";
		private const string EXIT = "exit";

		public override void Go()
		{
			VisualElement root = _mainMenuDoc.rootVisualElement;
			var play = root.Q<Button>(PLAY);
			var settings = root.Q<Button>(SETTINGS);
			var exit = root.Q<Button>(EXIT);

			play.RegisterCallback(new EventCallback<ClickEvent>(LoadGame));
			settings.RegisterCallback(new EventCallback<ClickEvent>(GoToSettings));
			exit.RegisterCallback(new EventCallback<ClickEvent>(ExitGame));
		}

		private async void LoadGame(ClickEvent evt) =>
			await SceneManagerInstance.StartNewScene<GameRoot>();

		private void GoToSettings(ClickEvent evt) =>
			Debug.Log("Go to settings");

		private void ExitGame(ClickEvent evt) =>
#if UNITY_EDITOR
			EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif
	}
}