using UnityEngine;

namespace SceneInjection
{
	public class ASceneRoot : MonoBehaviour
	{
		private static SceneManager _instance;
		public static SceneManager SceneManagerInstance => _instance ??= new SceneManager();

		private void Reset() =>
			gameObject.name = SceneManager.SetSceneName(GetType());

		private void Awake()
		{
			SceneManagerInstance.AddSceneAsLoaded(GetType(), this);
			SceneInjector.InjectScene(this);
		}

		private void Start() { }

		public virtual void Go() { }
	}
}