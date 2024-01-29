using SceneInjection;
using UnityEngine;

namespace Roots
{
	public sealed class GameRoot : ASceneRoot
	{
		[InjectScene] private PlayerTowerRoot _playerTower;
		[InjectScene] private GameBackgroundRoot _gameBackground;
		[InjectScene] private UiGameMenuRoot _gameMenu;

		// [InjectScene] private EnemySpawnerRoot _enemySpawner;

		[SerializeField] private Transform _towerPosition;

		public override void Go()
		{
			base.Go();
			
			_playerTower.Tower.position = _towerPosition.position;
		}
	}
}