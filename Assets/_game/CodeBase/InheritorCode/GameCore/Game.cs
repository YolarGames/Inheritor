using UnityEditor;
using UnityEngine;

namespace InheritorCode.GameCore
{
	public static class Game
	{
		public static bool IsPaused { get; private set; }

		public static void PauseGame(bool value)
		{
			Time.timeScale = value ? 0 : 1;
			IsPaused = value;
		}

		public static void Quit()
		{
#if UNITY_EDITOR
			EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif
		}
	}
}