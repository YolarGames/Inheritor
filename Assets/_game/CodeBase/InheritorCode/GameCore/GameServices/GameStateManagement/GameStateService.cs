using System;
using System.Threading.Tasks;
using InheritorCode.GameCore.Firebase;
using UnityEngine;

namespace InheritorCode.GameCore.GameServices.GameStateManagement
{
	public sealed class GameStateService : IGameStateService
	{
		private readonly IFirebaseService _firebaseService;
		public GameState State { get; private set; }

		public GameStateService(IFirebaseService firebaseService) =>
			_firebaseService = firebaseService;

		public async Task Init()
		{
			if (_firebaseService == null)
			{
				Debug.LogWarning("GameStateService: Firebase is not initialized. Creating empty game state.");
				State = new GameState();
				return;
			}

			State = await _firebaseService.LoadGameState();

			if (State == null)
			{
				Debug.LogWarning("GameStateService: Can't load game state from Firebase. Creating empty one.");
				State = new GameState();
			}
		}

		public Transaction<GameState> StartTransaction(params Action<GameState>[] onComplete) =>
			new(State, onComplete);

		public GameStateObserver<GameState> CreateObserver(Action<GameState> onChanged, params string[] properties) =>
			new(State, onChanged, properties);
	}

	public interface IGameStateService : IService
	{
		GameState State { get; }
		Transaction<GameState> StartTransaction(params Action<GameState>[] onComplete);
		GameStateObserver<GameState> CreateObserver(Action<GameState> onChanged, params string[] properties);
	}
}