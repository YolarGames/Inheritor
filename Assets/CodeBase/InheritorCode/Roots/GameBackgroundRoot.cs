using GameCore.GameServices;
using SceneInjection;
using UnityEngine;

namespace Roots
{
	public class GameBackgroundRoot : ASceneRoot
	{
		[SerializeField] private Transform _background;

		public override void Go()
		{
			base.Go();
			_background.localScale = CalculateScale();
		}

		private static Vector3 CalculateScale()
		{
			Camera cam = ServiceLocator.Container.GetService<AssetService>().Camera;
			float screenHeight = cam.orthographicSize * 2;
			var scale = new Vector3(screenHeight * cam.aspect, screenHeight, 1);
			return scale;
		}
	}
}