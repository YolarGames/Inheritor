using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameStateManagement
{
	public sealed class GameStateService /*: IGameStateService*/
	{
		private static GameStateService _instance = new();
		public static GameStateService Instance => _instance ??= new GameStateService();
		public GameState State { get; private set; }

		// public GameStateService(IFirebaseService firebaseService)
		// {
		// 	firebaseService.Init();
		// }

		public async Task Init()
		{
			GameState loadedState = null; /*await _firebaseService.GetGameState()*/

			State = loadedState ?? new GameState();
		}


		public Transaction<GameState> StartTransaction()
		{
			if (State != null)
				return new Transaction<GameState>(State);

			Debug.LogError("GameStateManager: State is not initialized!");
			return null;
		}

		public GameStateObserver<GameState> CreateObserver(Action<GameState> onChanged, params string[] properties) =>
			new(State, onChanged, properties);
	}

	/*public interface IGameStateService: IService
	{
		GameState State { get; }
		Transaction<GameState> StartTransaction();
		GameStateObserver<GameState> CreateObserver(Action<GameState> onChanged, params string[] properties);
	}*/
}