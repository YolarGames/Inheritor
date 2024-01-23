using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using GameCore.GameServices;
using Roots;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneInjection
{
	public sealed class SceneManager
	{
		private readonly Queue<ASceneRoot> _injectionQueue = new();
		private Task _injectSceneTask;
		private readonly Dictionary<Type, ASceneRoot> _newScenes = new();
		private Dictionary<Type, ASceneRoot> _loadedScenes = new();
		private LoadingScreenRoot _loadingScreen;

		public SceneManager() =>
			LoadLoadingScreen();

		public async Task StartNewScene<TScene>() where TScene : ASceneRoot
		{
			await AwaitInjectionFinished();

			await ShowLoadingScreen();

			foreach (KeyValuePair<Type, ASceneRoot> loadedScene in _loadedScenes)
				loadedScene.Value.gameObject.SetActive(false);

			await LoadScene<TScene>();
		}

		private async Task ShowLoadingScreen()
		{
			while (_loadingScreen == null)
				await Task.Yield();

			await _loadingScreen.Show();
		}

		public void AddSceneAsLoaded(Type type, ASceneRoot sceneRoot)
		{
			if (type == typeof(LoadingScreenRoot))
				return;

			Debug.Log($"Adding scene {type.Name}");

			_newScenes.Add(type, sceneRoot);
			ResolveDependencies(sceneRoot);
		}

		private async void ResolveDependencies(ASceneRoot sceneRoot)
		{
			_injectionQueue.Enqueue(sceneRoot);
			await AwaitInjectionFinished();

			_injectSceneTask = StartSceneResolution(_injectionQueue.Peek());
		}

		private async Task AwaitInjectionFinished()
		{
			if (_injectSceneTask != null)
				while (!_injectSceneTask.IsCompleted)
					await _injectSceneTask;
		}

		private async Task StartSceneResolution(ASceneRoot sceneRoot)
		{
			Debug.Log($"{sceneRoot.GetType().Name}: scene injection started");

			if (!sceneRoot.SceneDependencies.HasDependencies())
			{
				await Dequeue();
				return;
			}

			Debug.Log($"Awaiting dependencies resolution");

			foreach (FieldInfo field in sceneRoot.SceneDependencies.Dependencies)
			{
				Debug.Log($"Resolving dependency {field.FieldType.Name}");
				await ResolveScene(sceneRoot, field);
			}

			Debug.Log($"Dependencies resolved");

			await Dequeue();
			return;

			async Task Dequeue()
			{
				_injectionQueue.Dequeue();
				if (_injectionQueue.Count == 0)
					await InvokeGo();
			}
		}

		private async Task ResolveScene(ASceneRoot sceneRoot, FieldInfo fieldInfo)
		{
			if (!IsSceneInLoaded(fieldInfo.GetType(), out ASceneRoot loadedRoot)
			    && !IsSceneInNew(fieldInfo.GetType(), out loadedRoot))
			{
				Debug.Log($"{sceneRoot.GetType().Name}: loading {fieldInfo.FieldType.Name}");

				loadedRoot = await LoadScene(fieldInfo.FieldType);
			}

			if (!_newScenes.ContainsKey(loadedRoot.GetType()))
			{
				_loadedScenes.Remove(loadedRoot.GetType());
				_newScenes.Add(loadedRoot.GetType(), loadedRoot);
			}

			fieldInfo.SetValue(sceneRoot, loadedRoot);

			Debug.Log($"{sceneRoot.GetType().Name}: loaded {fieldInfo.FieldType.Name}");
			return;

			bool IsSceneInLoaded(Type getType, out ASceneRoot loadedComponent) =>
				_loadedScenes.TryGetValue(getType, out loadedComponent);

			bool IsSceneInNew(Type getType, out ASceneRoot loadedComponent) =>
				_newScenes.TryGetValue(getType, out loadedComponent);
		}

		private async Task<ASceneRoot> LoadScene<TScene>() where TScene : ASceneRoot =>
			await LoadScene(typeof(TScene));

		private async Task<ASceneRoot> LoadScene(Type sceneType)
		{
			string sceneName = GetSceneAsSceneName(sceneType);
			await SceneLoading(sceneName);

			GameObject gameObject = GameObject.Find(sceneName);
			Component component = gameObject.GetComponent(sceneType);

			return component as ASceneRoot;
		}

		private async Task UnloadScene(ASceneRoot sceneRoot)
		{
			AsyncOperation unloading = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(GetSceneAsSceneName(sceneRoot.GetType()));

			while (!unloading.isDone)
				await Task.Yield();
		}

		private async Task InvokeGo()
		{
			Debug.Log("Start services init");

			await ServiceLocator.Container.InitServices();

			foreach (KeyValuePair<Type, ASceneRoot> keyValuePair in _loadedScenes)
				await UnloadScene(keyValuePair.Value);

			Debug.Log("scenes unloaded");

			_loadedScenes = new Dictionary<Type, ASceneRoot>(_newScenes);
			_newScenes.Clear();

			foreach (KeyValuePair<Type, ASceneRoot> scene in _loadedScenes)
				scene.Value.Go();

			await _loadingScreen.Hide();
		}

		private async void LoadLoadingScreen()
		{
			if (_loadingScreen != null)
				return;

			_loadingScreen = await LoadScene<LoadingScreenRoot>() as LoadingScreenRoot;
		}

		private static async Task SceneLoading(string sceneName)
		{
			AsyncOperation loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

			while (!loading.isDone)
				await Task.Yield();
		}

		public static string GetSceneAsSceneName(Type type) =>
			type.Name.AsSceneRootName();
	}

	[AttributeUsage(AttributeTargets.Field)]
	public sealed class InjectSceneAttribute : Attribute { }
}