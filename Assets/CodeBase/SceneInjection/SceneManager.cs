using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.GameServices;
using Roots;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneInjection
{
	public sealed class SceneManager : IDisposable
	{
		private readonly Dictionary<Type, ASceneRoot> _loadedScenes = new();
		private Task _loadingTask;

		public static string SetSceneName(Type type) =>
			type.Name.AsSceneRootName();

		public async Task<ASceneRoot> Load(Type sceneType)
		{
			string sceneName = SetSceneName(sceneType);
			await LoadScene(sceneName);

			GameObject gameObject = GameObject.Find(sceneName);
			Component component = gameObject.GetComponent(sceneType);

			return component as ASceneRoot;
		}

		public async Task<ASceneRoot> Load<TScene>() where TScene : ASceneRoot
		{
			string sceneName = SetSceneName(typeof(TScene));
			await LoadScene(sceneName);

			GameObject gameObject = GameObject.Find(sceneName);
			Component component = gameObject.GetComponent(typeof(TScene));

			return component as ASceneRoot;
		}

		public async Task UnLoad(ASceneRoot sceneRoot)
		{
			// check if has scene dependencies
			// check if dependencies is needed for another scenes
			// unload unnecessary dependencies
		}

		public async Task InvokeGo()
		{
			await ServiceLocator.Container.InitServices();

			Debug.Log($"{GetType().Name} invoking GO");

			foreach (KeyValuePair<Type, ASceneRoot> keyValuePair in _loadedScenes)
				keyValuePair.Value.Go();
		}

		public bool IsSceneLoaded(Type getType, out ASceneRoot loadedComponent) =>
			_loadedScenes.TryGetValue(getType, out loadedComponent);

		public void AddSceneAsLoaded(Type type, ASceneRoot sceneRoot) =>
			_loadedScenes.Add(type, sceneRoot);

		public void Dispose() =>
			_loadedScenes.Clear();

		private static async Task LoadScene(string sceneName)
		{
			AsyncOperation loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

			while (!loading.isDone)
				await Task.Yield();
		}
	}
}