using UnityEngine;

[CreateAssetMenu(fileName = "CanvasSetupConfig", menuName = "Configs/CanvasSetupConfig")]
public sealed class CanvasSetupConfig : ScriptableObject
{
	[SerializeField] private Vector2Int _targetResolution;
		
}