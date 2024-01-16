using System.Threading.Tasks;
using Configs;
using UnityEngine;

namespace GameCore
{
	public sealed class ConfigService : IConfigService
	{
		public FactoryServiceConfig FactoryServiceConfig { get; private set; }
		public AssetServiceConfig AssetServiceConfig { get; private set; }

		public async Task Init()
		{
			FactoryServiceConfig = await LoadConfig<FactoryServiceConfig>();
			AssetServiceConfig = await LoadConfig<AssetServiceConfig>();
		}

		private async Task<TConfig> LoadConfig<TConfig>() where TConfig : ScriptableObject
		{
			ResourceRequest request = Resources.LoadAsync<TConfig>(GetConfigPath<TConfig>());

			while (!request.isDone)
				await Task.Yield();

			return request.asset as TConfig;
		}

		private static string GetConfigPath<TConfig>() where TConfig : ScriptableObject =>
			typeof(TConfig).ToString().Replace('.', '/');
	}
}