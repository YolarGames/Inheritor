using UnityEngine;
using UnityEngine.UI;

namespace InheritorCode.Configs
{
	[CreateAssetMenu(fileName = "CanvasScalerConfig", menuName = "Configs/CanvasScalerConfig")]
	public sealed class CanvasScalerConfig : ScriptableObject
	{
		[SerializeField] private CanvasScaler.ScaleMode _scaleMode;
		[SerializeField] private Vector2Int _referenceResolution;
		[SerializeField] private CanvasScaler.ScreenMatchMode _matchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
		[SerializeField, Range(0f, 1f)] private float _matchWidthHeight;
		[SerializeField] private float _referencePixelsPerUnit;

		public CanvasScaler.ScaleMode ScaleMode => _scaleMode;
		public Vector2Int ReferenceResolution => _referenceResolution;
		public CanvasScaler.ScreenMatchMode MatchMode => _matchMode;
		public float MatchWidthHeight => _matchWidthHeight;
		public float ReferencePixelsPerUnit => _referencePixelsPerUnit;
	}
}