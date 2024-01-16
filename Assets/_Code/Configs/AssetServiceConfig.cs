using Characters;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "AssetServiceConfig", menuName = "Configs/AssetService config", order = 0)]
	public sealed class AssetServiceConfig : ScriptableObject
	{
		[SerializeField] private Arrow _arrowPrefab;
		public Arrow ArrowPrefab => _arrowPrefab;
	}
}