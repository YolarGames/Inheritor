using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using RectTransform = UnityEngine.RectTransform;

public sealed class JoystickRangeFitter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	[SerializeField] private OnScreenStick _screenStick;
	private RectTransform _rectTransform;

	private void Start()
	{
		_rectTransform = GetComponent<RectTransform>();
		_rectTransform.sizeDelta =
			new Vector2(
				(_screenStick.dynamicOriginRange + _rectTransform.rect.width) * 2,
				(_screenStick.dynamicOriginRange + _rectTransform.rect.height) * 2);

		_screenStick.behaviour = OnScreenStick.Behaviour.ExactPositionWithStaticOrigin;
	}

	public void OnPointerDown(PointerEventData eventData) =>
		_screenStick.OnPointerDown(eventData);

	public void OnDrag(PointerEventData eventData) =>
		_screenStick.OnDrag(eventData);

	public void OnPointerUp(PointerEventData eventData) =>
		_screenStick.OnPointerUp(eventData);
}