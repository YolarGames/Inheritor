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

		public bool IsUserLoggedInWithEmail => _firebaseAuth.CurrentUser is { Email: not null };

		public async Task Init()
		{
			await CheckAndFixDependencies();
			_firebaseAuth = new FirebaseAuthInteraction(FirebaseAuth.DefaultInstance);
			await _firebaseAuth.ReloadUserAsync();
			_firebaseDatabase = new FirebaseDatabaseInteractions(FirebaseDatabase.DefaultInstance, _firebaseAuth);
		}

		public async Task CreateUserWithEmailAndPassword(string email, string password) =>
			await _firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);

		public async Task SignInWithEmailAndPassword(string email, string password) =>
			await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);

		public async Task<GameState> LoadGameState()
		{
			if (_firebaseAuth.CurrentUser is null)
				return null;

			return await _firebaseDatabase.LoadGameState();
		}

		public async Task UpdateGameState(GameState gameState) =>
			await _firebaseDatabase.UploadGameState(gameState);

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
		bool IsUserLoggedInWithEmail { get; }
		Task CreateUserWithEmailAndPassword(string email, string password);
		Task SignInWithEmailAndPassword(string email, string password);
		Task<GameState> LoadGameState();
		Task UpdateGameState(GameState gameState);
	}
}