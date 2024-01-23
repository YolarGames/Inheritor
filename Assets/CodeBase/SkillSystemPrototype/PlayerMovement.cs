using UnityEngine;

namespace SkillSystemPrototype
{
	public sealed class PlayerMovement : MonoBehaviour
	{
		private Rigidbody2D _rigidbody;
		private PlayerModel _playerModel;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
			_playerModel = new PlayerModel();
		}

		private void OnEnable() =>
			PlayerEvents.OnMove += Move;

		private void OnDisable() =>
			PlayerEvents.OnMove -= Move;

		private void Move(Vector2 moveVector) =>
			_rigidbody.velocity = moveVector * _playerModel.MovementSpeed.FinalValue;
	}
}