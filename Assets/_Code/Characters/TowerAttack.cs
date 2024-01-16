using GameCore;
using SkillSystemPrototype;
using UnityEngine;

namespace Characters
{
	public sealed class TowerAttack : MonoBehaviour
	{
		[SerializeField] private Transform _shootPoint;
		[SerializeField] private float _attackCooldown;
		[SerializeField] private float _arrowSpeed;
		[SerializeField] private int _arrowDamage;

		private IFactoryService _factoryService;
		private Timer _timer;
		private Transform _transform;
		private bool _mousePressed;
		private Vector2 _shootDirection;

		private void Awake()
		{
			_factoryService = ServiceLocator.Container.GetService<FactoryService>();
			_transform = transform;
			_timer = new Timer(_attackCooldown);
			_timer.Reset();
		}

		private void OnEnable()
		{
			PlayerEvents.OnMouseDown0 += SetMousePressed;
			PlayerEvents.OnMouseUp0 += SetMouseReleased;
		}

		private void OnDisable()
		{
			PlayerEvents.OnMouseDown0 -= SetMousePressed;
			PlayerEvents.OnMouseUp0 -= SetMouseReleased;
		}

		private void Update()
		{
			if (!CanShoot())
				return;

			_shootDirection = CalculateDirection();

			Arrow arrow = _factoryService.CreateArrow(_transform.position, _transform.rotation);
			arrow.InitData(_shootDirection, _arrowSpeed, _arrowDamage);
			arrow.Launch();

			_timer.Reset();
		}

		private Vector3 CalculateDirection() =>
			(_shootPoint.position - _transform.position).normalized;

		private bool CanShoot() =>
			_mousePressed && _timer.IsReady;

		private void SetMousePressed() =>
			_mousePressed = true;

		private void SetMouseReleased() =>
			_mousePressed = false;
	}
}