namespace GameCore
{
	public class StateBootstrap : IState
	{
		public async void Enter()
		{
			await ServiceLocator.Container.InitServices();
			GameStateMachine.Instance.Enter<StateLoadProgress>();
		}

		public void Exit() { }
	}
}