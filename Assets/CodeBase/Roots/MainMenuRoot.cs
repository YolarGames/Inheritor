using SceneInjection;
using UnityEngine;
using UnityEngine.UI;

namespace Roots
{
	public class MainMenuRoot : ASceneRoot
	{
		[SerializeField] private Button _playButton;
		[SerializeField] private Button _settingsButton;
		[SerializeField] private Button _exitButton;

		public override void Go()
		{
			Debug.Log("MainMenu GO");
			_playButton.onClick.AddListener(PlayGame);
		}

		private async void PlayGame() =>
			await SceneManagerInstance.StartNewScene<GameRoot>();
	}
}