using SkillSystemPrototype;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputManager : MonoBehaviour
{
	private PlayerInput _playerInput;

	private void Awake()
	{
		_playerInput = new PlayerInput();
		_playerInput.Enable();
	}

	private void OnEnable()
	{
		_playerInput.Main.move.started += MovePLayer;
		_playerInput.Main.move.performed += MovePLayer;
		_playerInput.Main.move.canceled += MovePLayer;
	}

	private void OnDisable()
	{
		_playerInput.Main.move.started -= MovePLayer;
		_playerInput.Main.move.performed -= MovePLayer;
		_playerInput.Main.move.canceled -= MovePLayer;
	}

	private void MovePLayer(InputAction.CallbackContext obj) =>
		PlayerEvents.OnMove?.Invoke(obj.ReadValue<Vector2>());
}