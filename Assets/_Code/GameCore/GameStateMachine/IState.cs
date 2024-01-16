namespace GameCore
{
	public interface IState : IExitableState
	{ void Enter(); }

	public interface IStatePayloaded : IExitableState
	{ void Enter<TPayload>(TPayload payload); }

	public interface IExitableState
	{ void Exit(); }
}