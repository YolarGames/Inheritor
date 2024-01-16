using System.Threading.Tasks;
using Characters;
using Configs;

namespace GameCore
{
	public sealed class AssetService : IAssetService
	{
		private AssetServiceConfig _config;

		public Arrow ArrowPrefab { get; private set; }

		public AssetService(AssetServiceConfig config) =>
			_config = config;

		public async Task Init()
		{
			ArrowPrefab = _config.ArrowPrefab;

			await Task.CompletedTask;
		}
	}
}