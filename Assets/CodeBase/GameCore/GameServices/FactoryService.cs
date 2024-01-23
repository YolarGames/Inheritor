using System.Threading.Tasks;
using Characters;
using UnityEngine;

namespace GameCore.GameServices
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
			Arrow arrow = Object.Instantiate(_assetService.ArrowPrefab, position, rotation);
			return arrow;
		}
	}
}