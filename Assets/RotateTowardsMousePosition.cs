using SkillSystemPrototype;
using UnityEngine;

public sealed class RotateTowardsMousePosition : MonoBehaviour
{
	private const float ROTATION_OFFSET = 90f;
	private Transform _transform;
	private Camera _camera;
	
	private void Awake()
	{
		_camera = Camera.main;
		_transform = transform;
	}

	private void OnEnable() =>
		PlayerEvents.OnMouseHold0 += RotateTowards;

	private void OnDisable() =>
		PlayerEvents.OnMouseHold0 -= RotateTowards;

	private void RotateTowards(Vector2 target)
	{
		Vector2 targetPos = _camera.ScreenToWorldPoint(target);
		Vector2 objectPos = _transform.position;
		Vector2 direction = (targetPos - objectPos).normalized;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		_transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - ROTATION_OFFSET));
	}
}