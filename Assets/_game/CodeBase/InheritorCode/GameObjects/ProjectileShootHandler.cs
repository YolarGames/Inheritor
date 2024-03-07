using System;
using System.Collections;
using InheritorCode.Characters;
using InheritorCode.GameCore;
using InheritorCode.GameCore.GameServices;
using UnityEngine;

namespace InheritorCode.GameObjects
{
	public sealed class ProjectileShootHandler : IDisposable
	{
		private readonly Transform _shootingObject;
		private readonly Transform _shootPoint;
		private readonly IFactoryService _factoryService;
		private readonly MonoBehaviour _monoBehaviour;
		private readonly Action _onShoot;
		private WaitForSeconds _waitForSeconds;
		private Coroutine _shootingRoutine;

		public ProjectileShootHandler(MonoBehaviour monoBehaviour, Transform shootingObject, Transform shootPoint,
			IFactoryService factoryService, Action onShoot
		)
		{
			_shootingObject = shootingObject;
			_shootPoint = shootPoint;
			_factoryService = factoryService;
			_monoBehaviour = monoBehaviour;
			_waitForSeconds = new WaitForSeconds(0.5f);
			_onShoot = onShoot;
		}

		public void SetNewShootSpeed(float value) =>
			_waitForSeconds = new WaitForSeconds(value);

		private IEnumerator ShootRoutine()
		{
			yield return null; // wait one frame for rotation to be applied

			while (true)
			{
				if (!Game.IsPaused)
				{
					LaunchProjectile();
					yield return _waitForSeconds;
				}

				yield return null;
			}
		}

		private void LaunchProjectile()
		{
			Vector3 shootDirection = CalculateDirection();

			Arrow arrow = _factoryService.CreateArrow(_shootingObject.position, _shootingObject.rotation);
			arrow.InitData(shootDirection, 5, 5);
			arrow.Launch();

			_onShoot?.Invoke();

			return;

			Vector3 CalculateDirection() =>
				(_shootPoint.position - _shootingObject.position).normalized;
		}

		public void StartShoot() =>
			_shootingRoutine = _monoBehaviour.StartCoroutine(ShootRoutine());

		public void StopShoot()
		{
			if (_shootingRoutine != null)
				_monoBehaviour.StopCoroutine(_shootingRoutine);
		}

		public void Dispose() =>
			StopShoot();
	}
}