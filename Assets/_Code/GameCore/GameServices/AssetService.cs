using System.Threading.Tasks;

namespace GameCore
{
	public sealed class AssetService : IAssetService
	{
		private IConfigService _configService;

		public AssetService(IConfigService configService) =>
			_configService = configService;

		public async Task Init() { }
	}
}