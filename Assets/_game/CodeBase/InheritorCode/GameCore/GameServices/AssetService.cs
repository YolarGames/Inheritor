using System.Threading.Tasks;
using InheritorCode.Characters;
using InheritorCode.Configs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InheritorCode.GameCore.GameServices
{
	public sealed class AssetService : IAssetService
	{
		private readonly AssetServiceConfig _config;

		public Arrow ArrowPrefab => _config.ArrowPrefab;
		public Camera Camera { get; private set; }

		public AssetService(AssetServiceConfig config) =>
			_config = config;

		public async Task Init()
		{
			Camera = Object.Instantiate(_config.Camera);
			Object.Instantiate(_config.EventSystem);
			Object.Instantiate(_config.DebugConsole);

			await Task.CompletedTask;
		}
	}

	public interface IAssetService : IService
	{
		Arrow ArrowPrefab { get; }
		Camera Camera { get; }
	}
}