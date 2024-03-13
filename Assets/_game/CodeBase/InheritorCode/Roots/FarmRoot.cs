using InheritorCode.Farming;
using InheritorCode.GameCore.GameServices;
using InheritorCode.SceneInjection;
using UnityEngine;

namespace InheritorCode.Roots
{
	public class FarmRoot : ASceneRoot
	{
		[SerializeField] private Transform[] _farmSpots;
		[SerializeField] private FarmTile[] _farmTiles;
		private IFactoryService _factoryService;

		public override void Go()
		{
			base.Go();

			_factoryService = ServiceLocator.Container.GetService<IFactoryService>();
			foreach (Transform farmSpot in _farmSpots)
				_factoryService.CreateFarmTile(farmSpot);
		}
	}
}