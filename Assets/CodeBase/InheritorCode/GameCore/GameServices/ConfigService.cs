using System.Threading.Tasks;
using Configs;
using UnityEngine;

namespace GameCore.GameServices
{
	public sealed class ConfigService : IConfigService
	{
		public AssetServiceConfig AssetServiceConfig { get; private set; }
		public AudioServiceConfig AudioServiceConfig { get; private set; }

		public async Task Init()
		{
			AssetServiceConfig = await LoadConfig<AssetServiceConfig>();
			AudioServiceConfig = await LoadConfig<AudioServiceConfig>();
		}

		private async Task<TConfig> LoadConfig<TConfig>() where TConfig : ScriptableObject
		{
			Debug.Log($"{GetType().Name} loading {typeof(TConfig).Name} config");

			ResourceRequest request = Resources.LoadAsync<TConfig>(GetConfigPath<TConfig>());

			while (!request.isDone)
				await Task.Yield();

			return request.asset as TConfig;
		}

		private static string GetConfigPath<TConfig>() where TConfig : ScriptableObject =>
			typeof(TConfig).ToString().Replace('.', '/');
	}

	public interface IConfigService : IService
	{
		AssetServiceConfig AssetServiceConfig { get; }
		AudioServiceConfig AudioServiceConfig { get; }
	}
}