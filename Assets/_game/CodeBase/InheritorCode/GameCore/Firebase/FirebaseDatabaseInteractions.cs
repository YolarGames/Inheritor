using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

namespace InheritorCode.GameCore.Firebase
{
	public class FirebaseDatabaseInteractions
	{
		private readonly FirebaseDatabase _database;
		private readonly FirebaseAuthInteraction _auth;

		public FirebaseDatabaseInteractions(FirebaseDatabase database, FirebaseAuthInteraction auth) =>
			(_database, _auth) = (database, auth);

		public async Task UploadGameState(string stateJson)
		{
			await _database.RootReference
				.Child("users")
				.Child(_auth.CurrentUser.UserId)
				.SetRawJsonValueAsync(stateJson).ContinueWithOnMainThread(HandleUpdateGameStateResult);
			return;

			void HandleUpdateGameStateResult(Task task)
			{
				if (task.IsFaulted)
					Debug.LogError("FirebaseService: Can't update game state. " + task.Exception);
			}
		}
	}
}