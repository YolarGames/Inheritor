using InheritorCode.Characters;
using UnityEngine;

namespace InheritorCode.Configs
{
	[CreateAssetMenu(fileName = "FactoryServiceConfig", menuName = "Configs/Services/FactoryService config")]
	public sealed class FactoryServiceConfig : ScriptableObject
	{
		[SerializeField] private Arrow _arrowPrefab;
		public Arrow ArrowPrefab => _arrowPrefab;
	}
}