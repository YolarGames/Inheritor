using InheritorCode.Configs;
using InheritorCode.GameCore.GameServices;
using InheritorCode.SceneInjection;
using UnityEngine;

namespace InheritorCode.Roots
{
	public class EnemySpawnerRoot : ASceneRoot
	{
		[SerializeField] private float _spawnOffsetY = 1;
		[SerializeField] private EnemySpawnPatternConfig _enemySpawnPatternConfig;

		private FactoryService _factoryService;
		private Vector2 _spawnHeightWidth;
		private Camera _camera;

		public override void Go()
		{
			base.Go();
			_factoryService = ServiceLocator.Container.GetService<FactoryService>();
			_camera = ServiceLocator.Container.GetService<AssetService>().Camera;
			_spawnHeightWidth = CalculateSpawnPoint();
			Debug.Log(_spawnHeightWidth);
			var spawner = new EnemyWaveSpawner(_enemySpawnPatternConfig, _spawnHeightWidth);
			StartCoroutine(spawner.StartSpawn());
		}

		private Vector2 CalculateSpawnPoint()
		{
			return new Vector2(
				_camera.orthographicSize * _camera.aspect,
				_camera.orthographicSize + _spawnOffsetY);
		}
	}
}