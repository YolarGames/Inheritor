using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

namespace InheritorCode.GameCore.Firebase
{
	public class FirebaseAuthInteraction
	{
		private readonly FirebaseAuth _auth;
		public FirebaseUser CurrentUser => _auth.CurrentUser;

		public FirebaseAuthInteraction(FirebaseAuth auth) =>
			_auth = auth;

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
	}
}