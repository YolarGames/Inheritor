using UnityEngine;

namespace SceneInjection
{
	public class ASceneRoot : MonoBehaviour
	{
		private static SceneManager _instance;
		public static SceneManager SceneManagerInstance => _instance ??= new SceneManager();
		public SceneDependencies SceneDependencies { get; private set; }

		private void Reset() =>
			gameObject.name = SceneManager.GetSceneAsSceneName(GetType());

		private void Awake()
		{
			SceneDependencies = SceneDependencies.Create(this);
			SceneManagerInstance.AddSceneAsLoaded(GetType(), this);
		}

		private void Start() { }

		public virtual void Go() =>
			Debug.Log(GetType().Name + "___GO");
	}
}