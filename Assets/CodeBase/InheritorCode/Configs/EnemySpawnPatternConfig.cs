using System;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "EnemySpawnPatternConfig", menuName = "Configs/Spawner/Spawn pattern")]
	public sealed class EnemySpawnPatternConfig : ScriptableObject
	{
		[SerializeField] private EnemySpawnUnit[] _spawnUnits;
		public EnemySpawnUnit[] SpawnUnits => _spawnUnits;
	}

	[Serializable]
	public class EnemySpawnUnit : SpawnerPart
	{
		public GameObject Enemy;
		public float DelayAfter;
	}

	public class SpawnerPart { }
}