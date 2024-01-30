using System.Threading.Tasks;
using Characters;
using Configs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore.GameServices
{
	public sealed class AssetService : IAssetService
	{
		private readonly AssetServiceConfig _config;

		public Arrow ArrowPrefab => _config.ArrowPrefab;
		public Camera Camera => _config.Camera;
		public EventSystem EventSystem => _config.EventSystem;

		public AssetService(AssetServiceConfig config) =>
			_config = config;

		public async Task Init()
		{
			Object.Instantiate(Camera);
			Object.Instantiate(EventSystem);

			await Task.CompletedTask;
		}
	}

	public interface IAssetService : IService
	{
		Arrow ArrowPrefab { get; }
		Camera Camera { get; }
		EventSystem EventSystem { get; }
	}
}