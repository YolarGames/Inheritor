using System.Threading.Tasks;
using SkillSystemPrototype;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore.GameServices
{
	public sealed class InputService : IInputService
	{
		private PlayerInput _playerInput;

		public async Task Init()
		{
			_playerInput = new PlayerInput();
			_playerInput.Enable();

			SubscribeToActions();

			await Task.CompletedTask;
		}

		private void SubscribeToActions()
		{
			_playerInput.Main.move.started += MovePLayer;
			_playerInput.Main.move.performed += MovePLayer;
			_playerInput.Main.move.canceled += MovePLayer;

			_playerInput.Main.mousePosition.started += TrackMousePosition;
			_playerInput.Main.mousePosition.performed += TrackMousePosition;
			_playerInput.Main.mousePosition.canceled += TrackMousePosition;
		}

		private void UnsubscribeFromActions()
		{
			_playerInput.Main.move.started -= MovePLayer;
			_playerInput.Main.move.performed -= MovePLayer;
			_playerInput.Main.move.canceled -= MovePLayer;

			_playerInput.Main.mousePosition.started -= TrackMousePosition;
			_playerInput.Main.mousePosition.performed -= TrackMousePosition;
			_playerInput.Main.mousePosition.canceled -= TrackMousePosition;
		}

		private void MovePLayer(InputAction.CallbackContext obj) =>
			PlayerEvents.OnMove?.Invoke(obj.ReadValue<Vector2>());

		private void TrackMousePosition(InputAction.CallbackContext obj)
		{
			var value = obj.ReadValue<Vector2>();
			
			if (obj.started)
				PlayerEvents.OnMouseDown0?.Invoke();

			else if (obj.performed)
				PlayerEvents.OnMouseHold0?.Invoke(value);

			else if (obj.canceled)
				PlayerEvents.OnMouseUp0?.Invoke();
		}
	}
}