using InheritorCode.GameCore.GameServices;

namespace InheritorCode.GameCore.GameStateMachine
{
	public class StateBootstrap : IState
	{
		public async void Enter()
		{
			await ServiceLocator.Container.StartServiceInitialization();
		}

		public void Exit() { }
	}
}