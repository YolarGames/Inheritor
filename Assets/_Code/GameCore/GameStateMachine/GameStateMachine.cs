using System;
using System.Collections.Generic;

namespace GameCore
{
	public class GameStateMachine
	{
		private static GameStateMachine _instance;
		public static GameStateMachine Instance => _instance ??= new GameStateMachine();

		private ServiceLocator _serviceLocator;
		private IExitableState _currentState;
		private Dictionary<Type, IExitableState> _states;

		public void CreateStates(LoadingScreen loadingScreen)
		{
			_states = new Dictionary<Type, IExitableState>
			{
				[typeof(StateBootstrap)] = new StateBootstrap(),
				[typeof(StateLoadProgress)] = new StateLoadProgress(),
				[typeof(StateLoadLevel)] = new StateLoadLevel(loadingScreen),
			};
		}

		public void Enter<TState>() where TState : class, IState =>
			ChangeState<TState>().Enter();

		public void Enter<TState, TPayload>(TPayload payload) where TState : class, IStatePayloaded<TPayload> =>
			ChangeState<TState>().Enter(payload);

		private TState ChangeState<TState>() where TState : class, IExitableState
		{
			_currentState?.Exit();

			var state = GetState<TState>();
			_currentState = state;

			return state;
		}

		private TState GetState<TState>() where TState : class, IExitableState =>
			_states[typeof(TState)] as TState;
	}
}