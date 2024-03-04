using System;
using System.Threading.Tasks;
using SkillSystemPrototype;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore.GameServices
{
	public sealed class InputService : IInputService
	{
		private PlayerInput _playerInput;
		private BackPressedHandler _backPressedHandler;

		public async Task Init()
		{
			_playerInput = new PlayerInput();
			_playerInput.Enable();

			var obj = new GameObject("InputServiceBackPressedHandler");
			obj.AddComponent<DontDestroyOnLoad>();
			_backPressedHandler = obj.AddComponent<BackPressedHandler>();

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

			_backPressedHandler.OnBackPressed += InvokeBackPressed;
		}

		private void UnsubscribeFromActions()
		{
			_playerInput.Main.move.started -= MovePLayer;
			_playerInput.Main.move.performed -= MovePLayer;
			_playerInput.Main.move.canceled -= MovePLayer;

			_playerInput.Main.mousePosition.started -= TrackMousePosition;
			_playerInput.Main.mousePosition.performed -= TrackMousePosition;
			_playerInput.Main.mousePosition.canceled -= TrackMousePosition;

			_backPressedHandler.OnBackPressed -= InvokeBackPressed;
		}

		private void InvokeBackPressed() =>
			PlayerInputEvents.OnBackPressed?.Invoke();

		private void MovePLayer(InputAction.CallbackContext obj) =>
			PlayerInputEvents.OnMove?.Invoke(obj.ReadValue<Vector2>());

		private void TrackMousePosition(InputAction.CallbackContext obj)
		{
			var value = obj.ReadValue<Vector2>();

			if (obj.started)
				PlayerInputEvents.OnMouseDown0?.Invoke();

			else if (obj.performed)
				PlayerInputEvents.OnMouseHold0?.Invoke(value);

			else if (obj.canceled)
				PlayerInputEvents.OnMouseUp0?.Invoke();
		}
	}

	public class BackPressedHandler : MonoBehaviour
	{
		public event Action OnBackPressed;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				OnBackPressed?.Invoke();
		}
	}

	public interface IInputService : IService { }
}