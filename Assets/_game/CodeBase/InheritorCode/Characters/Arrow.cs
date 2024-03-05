using UnityEngine;

namespace InheritorCode.Characters
{
	public class Arrow : MonoBehaviour
	{
		private Rigidbody2D _rigidbody;
		private Vector2 _direction;
		private float _speed;
		private int _damage;

		private void Awake() =>
			_rigidbody = GetComponent<Rigidbody2D>();

		public void InitData(Vector2 direction, float speed, int damage)
		{
			_direction = direction;
			_speed = speed;
			_damage = damage;
		}

		public void Launch() =>
			_rigidbody.velocity = _direction * _speed;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Enemy") && other.TryGetComponent(out EnemyHealth enemyHealth))
				enemyHealth.Damage(_damage);
		}
	}
}