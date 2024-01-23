using System.Collections;
using Characters;
using GameCore.GameServices;
using UnityEngine;

public sealed class ProjectileShootHandler
{
	private readonly Transform _shootingObject;
	private readonly Transform _shootPoint;
	private readonly FactoryService _factoryService;
	private bool _isShooting = false;
	private WaitForSeconds _waitForSeconds;

	public ProjectileShootHandler(Transform shootingObject, Transform shootPoint, FactoryService factoryService)
	{
		_shootingObject = shootingObject;
		_shootPoint = shootPoint;
		_factoryService = factoryService;
		_waitForSeconds = new WaitForSeconds(0.2f);
	}

	public IEnumerator ShootRoutine()
	{
		while (true)
		{
			if (!_isShooting)
			{
				yield return null;
				continue;
			}

			LaunchProjectile();
			yield return _waitForSeconds;
		}
	}

	private void LaunchProjectile()
	{
		Vector3 shootDirection = CalculateDirection();

		Arrow arrow = _factoryService.CreateArrow(_shootingObject.position, _shootingObject.rotation);
		arrow.InitData(shootDirection, 5, 5);
		arrow.Launch();
	}

	private Vector3 CalculateDirection() =>
		(_shootPoint.position - _shootingObject.position).normalized;

	public void StartShoot() =>
		_isShooting = true;

	public void StopShoot() =>
		_isShooting = false;
}