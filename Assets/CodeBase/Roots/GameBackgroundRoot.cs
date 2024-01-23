using GameCore.GameServices;
using SceneInjection;
using UnityEngine;

namespace Roots
{
	public class GameBackgroundRoot : ASceneRoot
	{
		[SerializeField] private Transform _background;

		public override void Go() =>
			_background.localScale = CalculateScale();

		private static Vector3 CalculateScale()
		{
			Camera cam = ServiceLocator.Container.GetService<AssetService>().Camera;
			float orthographicSize = cam.orthographicSize;
			var scale = new Vector3(orthographicSize * 2 * cam.aspect, orthographicSize * 2, 1);
			return scale;
		}
	}
}