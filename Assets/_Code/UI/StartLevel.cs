using GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public sealed class StartLevel : MonoBehaviour
	{
		private Button _button;

		private void Awake() =>
			_button = GetComponent<Button>();

		private void OnEnable() =>
			_button.onClick.AddListener(LoadLevel);

		private void OnDisable() =>
			_button.onClick.RemoveListener(LoadLevel);

		private void LoadLevel() =>
			GameStateMachine.Instance.Enter<StateLoadLevel, string>("2_game");
	}
}