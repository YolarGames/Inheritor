using InheritorCode.Characters;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InheritorCode.Configs
{
	[CreateAssetMenu(fileName = "AssetServiceConfig", menuName = "Configs/Services/AssetService config", order = 0)]
	public sealed class AssetServiceConfig : ScriptableObject
	{
		[SerializeField] private Arrow _arrowPrefab;
		[SerializeField] private Camera _camera;
		[SerializeField] private EventSystem _eventSystem;
		[SerializeField] private GameObject _debugConsole;
		

		public Arrow ArrowPrefab => _arrowPrefab;
		public Camera Camera => _camera;
		public EventSystem EventSystem => _eventSystem;
		public GameObject DebugConsole => _debugConsole;
	}
}