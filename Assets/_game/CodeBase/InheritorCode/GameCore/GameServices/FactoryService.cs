using System.Threading.Tasks;
using InheritorCode.Farming;
using UnityEngine;

namespace InheritorCode.GameCore.GameServices
{
	public sealed class FactoryService : IFactoryService
	{
		private readonly IAssetService _assetService;

		public FactoryService(IAssetService assetService) =>
			_assetService = assetService;

		public async Task Init()
		{
			// init pools
			await Task.CompletedTask;
		}

		public FarmTile CreateFarmTile(Transform farmSpot)
		{
			if (_assetService.FarmTilePrefab == null)
				Debug.LogError("FactoryService: FarmTilePrefab prefab is not set in AssetService");

			FarmTile farmTile = Object.Instantiate(_assetService.FarmTilePrefab, farmSpot.position, Quaternion.identity);
			farmTile.transform.parent = farmSpot;
			return farmTile;
		}
	}

	public interface IFactoryService : IService
	{
		FarmTile CreateFarmTile(Transform farmSpot);
	}
}