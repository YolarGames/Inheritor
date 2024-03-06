using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using InheritorCode.GameCore.GameServices.GameStateManagement;
using UnityEngine;

namespace InheritorCode.GameCore.Firebase
{
	public class FirebaseDatabaseInteractions
	{
		private const string USERS_PATH = "users";
		private readonly FirebaseDatabase _database;
		private readonly FirebaseAuthInteraction _auth;

		public FirebaseDatabaseInteractions(FirebaseDatabase database, FirebaseAuthInteraction auth)
		{
			(_database, _auth) = (database, auth);
			_database.SetPersistenceEnabled(false);
		}

		public async Task UploadGameState(GameState gameState)
		{
			string stateJson = JsonUtility.ToJson(gameState);
			Debug.Log($"FirebaseDatabase: uploading {stateJson}");

			await _database.RootReference
				.Child(USERS_PATH)
				.Child(_auth.CurrentUser.UserId)
				.SetRawJsonValueAsync(stateJson).ContinueWithOnMainThread(HandleUpdateGameStateResult);
			return;

			void HandleUpdateGameStateResult(Task task)
			{
				if (task.IsFaulted)
					Debug.LogError("FirebaseService: Can't update game state. " + task.Exception);
			}
		}

		public async Task<GameState> LoadGameState()
		{
			string stateJson = await _database.RootReference
				.Child(USERS_PATH)
				.Child(_auth.CurrentUser.UserId)
				.GetValueAsync().ContinueWithOnMainThread(HandleLoadGameStateResult);

			Debug.Log($"FirebaseDatabase: loaded {stateJson}");
			return stateJson == string.Empty ? null : JsonUtility.FromJson<GameState>(stateJson);

			string HandleLoadGameStateResult(Task<DataSnapshot> task)
			{
				if (!task.IsFaulted)
					return task.Result.GetRawJsonValue();

				Debug.LogError("FirebaseService: Can't load game state. " + task.Exception);
				return string.Empty;
			}
		}
	}
}