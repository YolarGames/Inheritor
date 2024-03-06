using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using InheritorCode.GameCore.GameServices;
using InheritorCode.GameCore.GameServices.GameStateManagement;
using UnityEngine;

namespace InheritorCode.GameCore.Firebase
{
	public class FirebaseService : IFirebaseService
	{
		private FirebaseApp _app;
		private FirebaseAuthInteraction _firebaseAuth;
		private FirebaseDatabaseInteractions _firebaseDatabase;

		public async Task Init()
		{
			await CheckAndFixDependencies();
			_firebaseAuth = new FirebaseAuthInteraction(FirebaseAuth.DefaultInstance);
			_firebaseDatabase = new FirebaseDatabaseInteractions(FirebaseDatabase.DefaultInstance, _firebaseAuth);
			await AuthUser();
		}

		public async Task<GameState> LoadGameState()
		{
			await Task.CompletedTask;
			return null;
		}

		public async Task UpdateGameState(GameState gameState)
		{
			string stateJson = JsonUtility.ToJson(gameState);
			Debug.Log(stateJson);

			await _firebaseDatabase.UploadGameState(stateJson);
		}

		private async Task AuthUser() =>
			await _firebaseAuth.SignInWithEmailAndPasswordAsync("test@test.com", "testing");

		private async Task CheckAndFixDependencies()
		{
			DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

			if (dependencyStatus == DependencyStatus.Available)
				_app = FirebaseApp.DefaultInstance;
			else
				Debug.LogError($"FirebaseService: Could not resolve all Firebase dependencies: {dependencyStatus}");
		}
	}

	public interface IFirebaseService : IService
	{
		Task<GameState> LoadGameState();
		Task UpdateGameState(GameState gameState);
	}
}