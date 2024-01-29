using System;
using System.Collections;
using Characters;
using GameCore;
using GameCore.GameServices;
using UnityEngine;

public sealed class ProjectileShootHandler : IDisposable
{
	private readonly Transform _shootingObject;
	private readonly Transform _shootPoint;
	private readonly FactoryService _factoryService;
	private readonly MonoBehaviour _monoBehaviour;
	private WaitForSeconds _waitForSeconds;
	private Coroutine _shootingRoutine;

	public ProjectileShootHandler(Transform shootingObject, Transform shootPoint, FactoryService factoryService, MonoBehaviour monoBehaviour)
	{
		_shootingObject = shootingObject;
		_shootPoint = shootPoint;
		_factoryService = factoryService;
		_monoBehaviour = monoBehaviour;
		_waitForSeconds = new WaitForSeconds(0.2f);
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