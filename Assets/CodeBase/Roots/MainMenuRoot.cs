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
			base.Go();
			_playButton.onClick.AddListener(PlayGame);
		}

		private async void PlayGame()
		{
			await SceneManagerInstance.Load<GameRoot>();
			await SceneManagerInstance.UnLoad(this);

		}

	}
}