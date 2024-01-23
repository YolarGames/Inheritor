using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace SceneInjection
{
	public static class SceneInjector
	{
		private static readonly Queue<ASceneRoot> _injectionQueue = new();
		private static Task _injectSceneTask;

		public static async void InjectScene(ASceneRoot sceneRoot)
		{
			_injectionQueue.Enqueue(sceneRoot);
			await AwaitInjectionFinished();
			_injectSceneTask = StartSceneInjection(_injectionQueue.Peek());
		}

		private static async Task AwaitInjectionFinished()
		{
			if (_injectSceneTask != null)
				while (!_injectSceneTask.IsCompleted)
					await _injectSceneTask;
		}

		private static async Task StartSceneInjection(ASceneRoot sceneRoot)
		{
			Debug.Log($"{sceneRoot.GetType().Name}: scene injection started");

			if (!HasSceneInjectableFields(sceneRoot, out HashSet<FieldInfo> fieldInfos)
			    || TryFastSceneResolve(sceneRoot, fieldInfos))
			{
				_injectionQueue.Dequeue();

				if (_injectionQueue.Count == 0)
					await ASceneRoot.SceneManagerInstance.InvokeGo();

				return;
			}

			foreach (FieldInfo field in fieldInfos)
				await LoadScene(sceneRoot, field);

			_injectionQueue.Dequeue();

			if (_injectionQueue.Count == 0)
				await ASceneRoot.SceneManagerInstance.InvokeGo();
		}

		private static bool HasSceneInjectableFields(ASceneRoot sceneRoot, out HashSet<FieldInfo> fieldInfos)
		{
			fieldInfos = new HashSet<FieldInfo>();
			bool hasFields = false;

			foreach (FieldInfo fieldInfo in GetFields())
			{
				if (!fieldInfo.HasAttribute(typeof(InjectSceneAttribute)))
					continue;

				Debug.Log($"{sceneRoot.GetType().Name}: {fieldInfo.FieldType.Name} to inject");

				fieldInfos.Add(fieldInfo);
				hasFields = true;
			}

			return hasFields;

			FieldInfo[] GetFields()
			{
				const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
				return sceneRoot.GetType().GetFields(BINDING_FLAGS);
			}
		}

		private static bool TryFastSceneResolve(ASceneRoot sceneRoot, HashSet<FieldInfo> fieldInfos)
		{
			bool canProcessWithoutLoading = true;
			var injectedFields = new HashSet<FieldInfo>();

			foreach (FieldInfo fieldInfo in fieldInfos)
			{
				if (TryFastSceneInject(sceneRoot, fieldInfo))
				{
					injectedFields.Add(fieldInfo);
					Debug.Log($"{sceneRoot.GetType().Name}: field {fieldInfo.FieldType.Name} fast resolved");
				}
				else
					canProcessWithoutLoading = false;
			}

			fieldInfos.ExceptWith(injectedFields);

			return canProcessWithoutLoading;
		}

		private static bool TryFastSceneInject(ASceneRoot sceneRoot, FieldInfo fieldInfo)
		{
			if (!ASceneRoot.SceneManagerInstance.IsSceneLoaded(fieldInfo.FieldType, out ASceneRoot loadedRoot))
				return false;

			fieldInfo.SetValue(sceneRoot, loadedRoot);
			return true;
		}

		private static async Task LoadScene(ASceneRoot sceneRoot, FieldInfo fieldInfo)
		{
			Debug.Log($"{sceneRoot.GetType().Name}: loading {fieldInfo.FieldType.Name}");

			ASceneRoot sceneComponent = await ASceneRoot.SceneManagerInstance.Load(fieldInfo.FieldType);
			fieldInfo.SetValue(sceneRoot, sceneComponent);

			Debug.Log($"{sceneRoot.GetType().Name}: loaded {fieldInfo.FieldType.Name}");
		}
	}

	[AttributeUsage(AttributeTargets.Field)]
	public sealed class InjectSceneAttribute : Attribute { }
}