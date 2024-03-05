using System.Collections;
using UnityEngine;

namespace InheritorCode.Configs
{
	public sealed class EnemyWaveSpawner
	{
		private readonly EnemySpawnPatternConfig _spawnPatternConfig;
		private readonly Vector2 _spawnHeightWidth;

		public EnemyWaveSpawner(EnemySpawnPatternConfig spawnPatternConfig, Vector2 spawnHeightWidth)
		{
			_spawnPatternConfig = spawnPatternConfig;
			_spawnHeightWidth = spawnHeightWidth;
		}

		public IEnumerator StartSpawn()
		{
			foreach (EnemySpawnUnit unit in _spawnPatternConfig.SpawnUnits)
			{
				float xPos = Random.Range(-_spawnHeightWidth.x, _spawnHeightWidth.x);
				yield return new WaitForSeconds(unit.DelayAfter);
			}
		}
	}
}