using System.Threading.Tasks;
using InheritorCode.Configs;
using UnityEngine;

namespace InheritorCode.GameCore.GameServices
{
	public sealed class ConfigService : IConfigService
	{
		private const string CONFIGS_PATH = "Configs/";
		public AssetServiceConfig AssetServiceConfig { get; private set; }
		public AudioServiceConfig AudioServiceConfig { get; private set; }

		public async Task Init()
		{
			AssetServiceConfig = await LoadConfig<AssetServiceConfig>();
			AudioServiceConfig = await LoadConfig<AudioServiceConfig>();
		}

		private async Task<TConfig> LoadConfig<TConfig>() where TConfig : ScriptableObject
		{
			ResourceRequest request = Resources.LoadAsync<TConfig>(GetConfigPath<TConfig>());

			while (!request.isDone)
				await Task.Yield();

			if (request.asset != null)
				return request.asset as TConfig;

			Debug.LogError("ConfigService: Failed to load config: " + typeof(TConfig).Name);
			return null;
		}

		private static string GetConfigPath<TConfig>() where TConfig : ScriptableObject =>
			CONFIGS_PATH + typeof(TConfig).Name;
	}

	public interface IConfigService : IService
	{
		AssetServiceConfig AssetServiceConfig { get; }
		AudioServiceConfig AudioServiceConfig { get; }
	}
}