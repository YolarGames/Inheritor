using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace InheritorCode.UI
{
	public class ShowHideHandler
	{
		private const string k_opacity0 = "opacity-0";

		private readonly VisualElement _visualElement;
		private readonly Action<bool> _onChanged;
		private bool _isShown = false;

		public ShowHideHandler(VisualElement visualElement, MonoBehaviour monoBehaviour, Action<bool> onChanged = null)
		{
			if (visualElement == null || monoBehaviour == null)
				Debug.LogError("Can't init members with Null");

			_visualElement = visualElement;
			_onChanged = onChanged;

			monoBehaviour.StartCoroutine(SetDisplayNone());
		}

		public async Task Show()
		{
			DisplayImmediate(true);
			_visualElement.RemoveFromClassList(k_opacity0);
			_visualElement.RegisterCallback<TransitionEndEvent>(OnTransitionEnded);

			while (!_isShown)
				await Task.Yield();
		}

		public async Task Hide()
		{
			_visualElement.AddToClassList(k_opacity0);
			_visualElement.RegisterCallback<TransitionEndEvent>(OnTransitionEnded);

			while (_isShown)
				await Task.Yield();

			DisplayImmediate(false);
		}

		public async void Toggle()
		{
			if (_isShown)
				await Hide();
			else
				await Show();
		}

		public async Task ToggleAwaitable()
		{
			if (_isShown)
				await Hide();
			else
				await Show();
		}

		private void OnTransitionEnded(TransitionEndEvent evt)
		{
			_visualElement.UnregisterCallback<TransitionEndEvent>(OnTransitionEnded);
			_isShown = !_visualElement.ClassListContains(k_opacity0);
			_onChanged?.Invoke(_isShown);
		}

		private IEnumerator SetDisplayNone()
		{
			yield return null;
			_visualElement.style.display = DisplayStyle.None;
		}

		private void DisplayImmediate(bool value) =>
			_visualElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
	}
}