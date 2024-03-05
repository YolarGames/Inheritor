using UnityEngine;

namespace InheritorCode.Characters
{
	public class Health : MonoBehaviour
	{
		[SerializeField] private int _maxHealth;
		private int _currentHealth;

		private void Awake() =>
			CurrentHealth = _maxHealth;

		public int CurrentHealth
		{
			get => _currentHealth;
			set
			{
				int health = Mathf.Clamp(value, 0, _maxHealth);

				if (health == 0)
					Die();
			}
		}

		public virtual void Damage(int damage) { }

		protected virtual void Die() =>
			Destroy(gameObject);
	}
}