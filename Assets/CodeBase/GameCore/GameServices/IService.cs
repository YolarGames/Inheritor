using System.Threading.Tasks;
using Characters;
using Configs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore.GameServices
{
	public interface IService
	{
		Task Init();
	}

	public interface IInputService : IService { }

	public interface IConfigService : IService
	{
		AssetServiceConfig AssetServiceConfig { get; }
	}

	public interface IAssetService : IService
	{
		Arrow ArrowPrefab { get; }
		Camera Camera { get; }
		EventSystem EventSystem { get; }
	}

	public interface IFactoryService : IService
	{
		Arrow CreateArrow(Vector3 position, Quaternion transformRotation);
	}
}