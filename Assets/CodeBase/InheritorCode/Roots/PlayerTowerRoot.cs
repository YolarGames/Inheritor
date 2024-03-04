using Audio;
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
		[SerializeField] private AudioClip _bowShotSfx;

		private FactoryService _factoryService;
		private RotationHandler _rotationHandler;
		private ProjectileShootHandler _projectileShootHandler;
		private Coroutine _rotationRoutine;
		private AudioSfxPlayer _sfxPlayer;
		public Transform Tower => _tower;

		public override void Go()
		{
			base.Go();

			_rotationHandler = new RotationHandler(_tower.transform, _rotationOffset);
			_sfxPlayer = new AudioSfxPlayer(_bowShotSfx);
			_projectileShootHandler = new ProjectileShootHandler(
				this,
				_tower,
				_shootPosition,
				factoryService: ServiceLocator.Container.GetService<FactoryService>(),
				onShoot: _sfxPlayer.PlayOneShot);

			PlayerInputEvents.OnMouseHold0 += _rotationHandler.Rotate;
			PlayerInputEvents.OnMouseDown0 += _projectileShootHandler.StartShoot;
			PlayerInputEvents.OnMouseUp0 += _projectileShootHandler.StopShoot;
		}

		private void OnDisable()
		{
			PlayerInputEvents.OnMouseHold0 -= _rotationHandler.Rotate;
			PlayerInputEvents.OnMouseDown0 -= _projectileShootHandler.StartShoot;
			PlayerInputEvents.OnMouseUp0 -= _projectileShootHandler.StopShoot;

			_projectileShootHandler.Dispose();
		}
	}
}