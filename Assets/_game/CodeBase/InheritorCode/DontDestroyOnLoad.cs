using UnityEngine;

namespace InheritorCode
{
	public sealed class DontDestroyOnLoad : MonoBehaviour
	{
		private void Awake() =>
			DontDestroyOnLoad(gameObject);
	}
}