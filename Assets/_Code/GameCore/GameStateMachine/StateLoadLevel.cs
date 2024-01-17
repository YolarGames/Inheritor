using UI;

namespace GameCore
{
	public class StateLoadLevel : IStatePayloaded<string>
	{
		private readonly LoadingScreen _loadingScreen;
		private readonly SceneLoader _sceneLoader;

		public StateLoadLevel(LoadingScreen loadingScreen)
		{
			_loadingScreen = loadingScreen;
			_sceneLoader = new SceneLoader();
		}

		public async void Enter(string sceneName)
		{
			await _loadingScreen.Show(() => { });
			await _sceneLoader.Load(sceneName);
			await _loadingScreen.Hide();
		}

		public void Exit() { }
	}
}