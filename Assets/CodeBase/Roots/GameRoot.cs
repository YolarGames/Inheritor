using SceneInjection;
using UnityEngine;

namespace Roots
{
	public sealed class GameRoot : ASceneRoot
	{
		[InjectScene] private PlayerTowerRoot _playerTower;
		[InjectScene] private GameBackgroundRoot _gameBackground;

		[SerializeField] private Transform _towerPosition;

		public override void Go()
		{
			_playerTower.Tower.position = _towerPosition.position;
		}
	}
}