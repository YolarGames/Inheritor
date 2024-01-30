using System;
using UnityEngine.UIElements;

namespace Utils
{
	public static class CustomExtensions
	{
		#region UI Toolkit

		public static Button GetButton(this VisualElement visualElement, string name = "") =>
			visualElement?.Q<Button>(name);

		public static VisualElement GetVisualElement(this UIDocument document, string name) =>
			document != null ? document.rootVisualElement.Q<VisualElement>(name) : null;

		public static VisualElement GetVisualElement(this VisualElement visualElement, string name) =>
			visualElement?.Q<VisualElement>(name);

		public static Slider GetSlider(this VisualElement visualElement, string name = null) =>
			visualElement?.Q<Slider>(name);

		public static void RegisterClickEvent(this VisualElement visualElement, EventCallback<ClickEvent> action) =>
			visualElement?.RegisterCallback<ClickEvent>(action);

		public static void UnregisterClickEvent(this VisualElement visualElement, EventCallback<ClickEvent> action) =>
			visualElement?.UnregisterCallback<ClickEvent>(action);

		public static void RegisterMouseEnterEvent(this VisualElement visualElement, EventCallback<MouseEnterEvent> action) =>
			visualElement?.RegisterCallback<MouseEnterEvent>(action);

		public static void UnregisterMouseEnterEvent(this VisualElement visualElement, EventCallback<MouseEnterEvent> action) =>
			visualElement?.UnregisterCallback<MouseEnterEvent>(action);

		#endregion
	}
}