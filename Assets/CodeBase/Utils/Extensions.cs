﻿using System;
using UnityEngine.UIElements;

namespace Utils
{
	public static class CustomExtensions
	{
		#region UI Toolkit

		public static Button GetButton(this UIDocument document, string name = "") =>
			document != null ? document.rootVisualElement.Q<Button>(name) : null;

		public static Button GetButton(this VisualElement visualElement, string name = "") =>
			visualElement?.Q<Button>(name);

		public static Label GetLabel(this UIDocument document, string name) =>
			document != null ? document.rootVisualElement.Q<Label>(name) : null;

		public static Label GetLabel(this VisualElement visualElement, string name) =>
			visualElement?.Q<Label>(name);

		public static VisualElement GetVisualElement(this UIDocument document, string name) =>
			document != null ? document.rootVisualElement.Q<VisualElement>(name) : null;

		public static VisualElement GetVisualElement(this VisualElement visualElement, string name) =>
			visualElement?.Q<VisualElement>(name);

		public static void RegisterClickEvent(this VisualElement visualElement, EventCallback<ClickEvent> action) =>
			visualElement?.RegisterCallback<ClickEvent>(action);

		public static void UnregisterClickEvent(this VisualElement visualElement, EventCallback<ClickEvent> action) =>
			visualElement?.UnregisterCallback<ClickEvent>(action);

		#endregion
	}
}