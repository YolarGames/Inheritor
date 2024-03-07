using InheritorCode.GameCore.GameServices;
using InheritorCode.SceneInjection;
using UnityEngine;

namespace InheritorCode.Roots
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
			Camera cam = ServiceLocator.Container.GetService<IAssetService>().Camera;
			float screenHeight = cam.orthographicSize * 2;
			var scale = new Vector3(screenHeight * cam.aspect, screenHeight, 1);
			return scale;
		}
	}
}