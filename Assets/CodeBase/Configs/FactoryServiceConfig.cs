using Characters;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "FactoryServiceConfig", menuName = "Configs/FactoryService config")]
	public sealed class FactoryServiceConfig : ScriptableObject
	{
		[SerializeField] private Arrow _arrowPrefab;
		public Arrow ArrowPrefab => _arrowPrefab;
	}
}