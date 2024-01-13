using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = "CanvasSetupConfig", menuName = "Configs/CanvasSetupConfig")]
	public sealed class CanvasSetupConfig : ScriptableObject
	{
		[SerializeField] private RenderMode _renderMode;
		[SerializeField] private Vector2Int _targetResolution;
	}
}