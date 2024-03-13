using InheritorCode.Farming;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InheritorCode.Configs
{
	[CreateAssetMenu(fileName = "AssetServiceConfig", menuName = "Configs/Services/AssetService config", order = 0)]
	public sealed class AssetServiceConfig : ScriptableObject
	{
		[SerializeField] private FarmTile _farmTilePrefab;
		[SerializeField] private Camera _camera;
		[SerializeField] private EventSystem _eventSystem;
		[SerializeField] private GameObject _debugConsole;

		public FarmTile FarmTilePrefab => _farmTilePrefab;
		public Camera Camera => _camera;
		public EventSystem EventSystem => _eventSystem;
		public GameObject DebugConsole => _debugConsole;
	}
}