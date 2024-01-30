using Characters;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Configs
{
	[CreateAssetMenu(fileName = "AssetServiceConfig", menuName = "Configs/Services/AssetService config", order = 0)]
	public sealed class AssetServiceConfig : ScriptableObject
	{
		[SerializeField] private Arrow _arrowPrefab;
		[SerializeField] private Camera _camera;
		[SerializeField] private EventSystem _eventSystem;

		public Arrow ArrowPrefab => _arrowPrefab;
		public Camera Camera => _camera;
		public EventSystem EventSystem => _eventSystem;
	}
}