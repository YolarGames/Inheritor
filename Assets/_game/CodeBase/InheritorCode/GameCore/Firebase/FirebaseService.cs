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
		private FirebaseAuth _auth;
		private FirebaseDatabase _database;

		public async Task Init()
		{
			await CheckAndFixDependencies();
			InitFirebaseAuth();
			// InitFirebaseDatabase();
			await AuthUser();
		}

		public async Task<GameState> LoadGameState()
		{
			await Task.CompletedTask;
			return null;
		}

		private async Task AuthUser()
		{
			Task<AuthResult> task = _auth.SignInWithEmailAndPasswordAsync("test@test.com", "testing");

			await task;

			if (task.IsFaulted)
				Debug.Log("FirebaseService: Sign in is faulted");
			else
				Debug.Log($"FirebaseService: User {task.Result.User.Email} authenticated successfully!");
		}

		private async Task CheckAndFixDependencies()
		{
			DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

			if (dependencyStatus == DependencyStatus.Available)
				_app = FirebaseApp.DefaultInstance;
			else
				Debug.LogError($"FirebaseService: Could not resolve all Firebase dependencies: {dependencyStatus}");
		}

		private void InitFirebaseAuth()
		{
			if (FirebaseAuth.DefaultInstance == null)
				Debug.LogError("Firebase: Can't init FirebaseAuth");
			else
				_auth = FirebaseAuth.DefaultInstance;
		}

		private void InitFirebaseDatabase()
		{
			if (FirebaseDatabase.DefaultInstance == null)
				Debug.LogError("FirebaseService: Can't init FirebaseDatabase");
			else
				_database = FirebaseDatabase.DefaultInstance;
		}
	}

	public interface IFirebaseService : IService
	{
		Task<GameState> LoadGameState();
	}
}