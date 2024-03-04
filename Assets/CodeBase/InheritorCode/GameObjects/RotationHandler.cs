using GameCore;
using GameCore.GameServices;
using UnityEngine;
using Utils;

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
		if (Game.IsPaused)
			return;

		_transform.rotation = YolarUtils.Transform.Rotate(
			_transform.position,
			_camera.ScreenToWorldPoint(target),
			_rotationOffset);
	}
}