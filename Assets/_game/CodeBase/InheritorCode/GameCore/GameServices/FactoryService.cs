using System.Threading.Tasks;
using InheritorCode.Characters;
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

		public Arrow CreateArrow(Vector3 position, Quaternion rotation)
		{
			if (_assetService.ArrowPrefab == null)
				Debug.LogError("FactoryService: Arrow prefab is not set in AssetService");

			Arrow arrow = Object.Instantiate(_assetService.ArrowPrefab, position, rotation);
			return arrow;
		}
	}

	public interface IFactoryService : IService
	{
		Arrow CreateArrow(Vector3 position, Quaternion transformRotation);
	}
}