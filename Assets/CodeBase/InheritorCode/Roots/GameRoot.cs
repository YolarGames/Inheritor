using GameStateManagement;
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

		private GameStateService _gameStateService = GameStateService.Instance;

		public override void Go()
		{
			base.Go();

			using (Transaction<GameState> transaction = _gameStateService.StartTransaction())
			{
				transaction.State.Coins = 0;
				transaction.State.Exp = 0;
			}

			_playerTower.Tower.position = _towerPosition.position;
		}
	}
}