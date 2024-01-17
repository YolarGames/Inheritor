using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore
{
	public sealed class SceneLoader
	{
		public async Task Load(string sceneName)
		{
			AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
			while (!loading.isDone)
				await Task.Yield();
		}
	}
}