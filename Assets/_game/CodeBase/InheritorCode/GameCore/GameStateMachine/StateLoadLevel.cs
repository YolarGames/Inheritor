namespace InheritorCode.GameCore.GameStateMachine
{
	public class StateLoadLevel : IStatePayloaded<string>
	{
		private readonly SceneLoader _sceneLoader;

		public async void Enter(string sceneName) =>
			await _sceneLoader.Load(sceneName);

		public void Exit() { }
	}
}