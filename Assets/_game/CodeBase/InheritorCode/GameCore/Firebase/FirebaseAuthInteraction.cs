using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

namespace InheritorCode.GameCore.Firebase
{
	public class FirebaseAuthInteraction
	{
		private readonly FirebaseAuth _auth;
		public FirebaseUser CurrentUser => _auth.CurrentUser;

		public FirebaseAuthInteraction(FirebaseAuth auth) =>
			_auth = auth;

		public async Task AuthWithGooglePlay()
		{
			float yieldTimeout = Time.time + 5f;

			if (!PlayGamesPlatform.Instance.IsAuthenticated() || Time.time < yieldTimeout)
				await Task.Yield();

			PlayGamesPlatform.Instance.Authenticate(HandleAuthStatus);

			return;

			void HandleAuthStatus(SignInStatus status)
			{
				if (status == SignInStatus.Success)
					PlayGamesPlatform.Instance.RequestServerSideAccess(true, HandleServerSideAccess);
				else
					Debug.LogError("FirebaseService: Can't authenticate with Google Play. Status: " + status);
			}

			void HandleServerSideAccess(string code)
			{
				Credential credential = PlayGamesAuthProvider.GetCredential(code);
				_auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(HandleSignInResult);
			}

			void HandleSignInResult(Task<FirebaseUser> task)
			{
				if (task.IsFaulted)
					Debug.LogError("FirebaseService: Can't sign in with Google Play. " + task.Exception);
				else
					Debug.Log($"FirebaseService: User {task.Result.Email} authenticated successfully!");
			}
		}

		public async Task SignInWithEmailAndPasswordAsync(string email, string password)
		{
			await _auth.SignInWithEmailAndPasswordAsync(email, password)
				.ContinueWithOnMainThread(HandleSignInResult);

			return;

			void HandleSignInResult(Task<AuthResult> task)
			{
				if (task.IsFaulted)
					Debug.Log("FirebaseService: Sign in is faulted. Reason: " + task.Exception);
				else
					Debug.Log($"FirebaseService: User {task.Result.User.Email} authenticated successfully!");
			}
		}

		public async Task CreateUserWithEmailAndPasswordAsync(string email, string password)
		{
			await _auth.CreateUserWithEmailAndPasswordAsync(email, password)
				.ContinueWithOnMainThread(HandleCreateUserResult);

			return;

			void HandleCreateUserResult(Task<AuthResult> task)
			{
				if (task.IsFaulted)
					Debug.Log("FirebaseService: User creation is faulted. Reason: " + task.Exception);
				else
					Debug.Log($"FirebaseService: User {task.Result.User.Email} created successfully!");
			}
		}

		public async Task ReloadUserAsync()
		{
			if (_auth.CurrentUser != null)
				await _auth.CurrentUser.ReloadAsync().ContinueWithOnMainThread(HandleUserReloadResult);

			return;

			void HandleUserReloadResult(Task task)
			{
				if (task.IsFaulted)
					Debug.LogError("FirebaseService: Can't reload user. " + task.Exception);
			}
		}
	}
}