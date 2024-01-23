using GameCore.GameServices;
using UnityEngine;

public sealed class RotationHandler
{
	private readonly Transform _transform;
	private readonly Camera _camera;
	private readonly float _rotationOffset = 0;

	public RotationHandler(Transform transform, float rotationOffset = 0)
	{
		_transform = transform;
		_camera = ServiceLocator.Container.GetService<AssetService>().Camera;
		_rotationOffset = rotationOffset;
	}

	public void Rotate(Vector2 target)
	{
		Vector2 targetPos = _camera.ScreenToWorldPoint(target);
		Vector2 objectPos = _transform.position;
		Vector2 direction = (targetPos - objectPos).normalized;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		_transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - _rotationOffset));
	}
}