using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InheritorCode.Farming
{
	public class FarmTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] private SpriteRenderer _buildingView;

		private Tween _tileAnimation;

		public void SetBuildingView(Sprite sprite) =>
			_buildingView.sprite = sprite;

		private void OnMouseDown()
		{
			ScaleTo(0.95f);
		}

		private void OnMouseUp()
		{
			ScaleTo(1f);
		}

		private void ScaleTo(float value)
		{
			_tileAnimation.Kill();
			_tileAnimation = transform.DOScale(value, 0.1f);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Debug.Log("Pointer enter");
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Debug.Log("Pointer exit");
		}
	}
}