using GameCore.GameServices;
using SceneInjection;
using SkillSystemPrototype;
using UnityEngine;

namespace Roots
{
	public sealed class PlayerTowerRoot : ASceneRoot
	{
		[SerializeField] private Transform _tower;
		[SerializeField] private Transform _shootPosition;
		[SerializeField] private float _rotationOffset;

		private FactoryService _factoryService;
		private RotationHandler _rotationHandler;
		private ProjectileShootHandler _projectileShootHandler;

		public Transform Tower => _tower;

		public override void Go()
		{
			_rotationHandler = new RotationHandler(_tower.transform, _rotationOffset);
			_projectileShootHandler = new ProjectileShootHandler(_tower, _shootPosition, ServiceLocator.Container.GetService<FactoryService>());

			PlayerEvents.OnMouseHold0 += _rotationHandler.Rotate;
			PlayerEvents.OnMouseDown0 += _projectileShootHandler.StartShoot;
			PlayerEvents.OnMouseUp0 += _projectileShootHandler.StopShoot;

			StartCoroutine(_projectileShootHandler.ShootRoutine());
		}
	}
}