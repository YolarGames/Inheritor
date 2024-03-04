namespace GameCore.GameStateMachine
{
	public interface IStatePayloaded<TPayload> : IExitableState
	{
		void Enter(TPayload payload);
	}

	public interface IState : IExitableState
	{
		void Enter();
	}

	public interface IExitableState
	{
		void Exit();
	}
}