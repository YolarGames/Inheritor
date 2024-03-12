using System.Linq;
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
		private const string GOOGLE_PLAY_PROVIDER_ID = "playgames.google.com";
		private const string FACEBOOK_PROVIDER_ID = "facebook.com";

		private FirebaseApp _app;
		private FirebaseAuthInteraction _firebaseAuth;
		private FirebaseDatabaseInteractions _firebaseDatabase;

		public FirebaseUser CurrentUser => _firebaseAuth.CurrentUser;
		public bool IsUserLoggedWithEmail => _firebaseAuth.CurrentUser is { Email: not null };
		public bool IsUserLoggedWithGooglePlay
		{
			get
			{
				if (_firebaseAuth.CurrentUser is null)
					return false;

				return _firebaseAuth.CurrentUser.ProviderData.Any(
					provider => provider.ProviderId == GOOGLE_PLAY_PROVIDER_ID);
			}
		}
		public bool IsUserLoggedWithFacebook
		{
			get
			{
				if (_firebaseAuth.CurrentUser is null)
					return false;

				return _firebaseAuth.CurrentUser.ProviderData.Any(
					provider => provider.ProviderId == FACEBOOK_PROVIDER_ID);
			}
		}

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

		public async Task AuthWithGooglePlay()
		{
			if (IsUserLoggedWithGooglePlay)
				return;

			await _firebaseAuth.AuthWithGooglePlay();
		}

		public async Task AuthWithFacebook()
		{
			if (IsUserLoggedWithFacebook)
				return;

			await _firebaseAuth.AuthWithFacebook();
		}

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
		bool IsUserLoggedWithEmail { get; }
		bool IsUserLoggedWithGooglePlay { get; }
		bool IsUserLoggedWithFacebook { get; }
		FirebaseUser CurrentUser { get; }
		Task CreateUserWithEmailAndPassword(string email, string password);
		Task SignInWithEmailAndPassword(string email, string password);
		Task<GameState> LoadGameState();
		Task UpdateGameState(GameState gameState);
		Task AuthWithGooglePlay();
		Task AuthWithFacebook();
	}
}