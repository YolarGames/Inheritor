using UnityEngine;
using Utils;

namespace SceneRoot
{
	public class SceneRootBase : MonoBehaviour
	{
		private void Reset()
		{
			if (transform.parent != null)
			{
				Debug.LogError("SceneRoot must not have a parent object", this);
				return;
			}

			gameObject.name = GetType().Name.AsSceneRootName();
		}

		protected virtual void Go() { }
	}
}