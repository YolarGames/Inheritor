using System.Threading.Tasks;
using InheritorCode.GameCore.Firebase;
using InheritorCode.GameCore.GameServices.GameStateManagement;
using UnityEngine;

namespace InheritorCode.GameCore.GameServices
{
	public sealed class ServiceLocator
	{
		private bool _isInitialized;
		private static ServiceLocator _instance;
		public static ServiceLocator Container => _instance ??= new ServiceLocator();
		private static Task _initTask;

		public async Task InitServices()
		{
			if (_isInitialized)
				return;

			Debug.Log($"{GetType().Name} start service initialization");

			await StartServiceInitialization();

			Debug.Log($"{GetType().Name} finish service initialization");
		}

		public async Task StartServiceInitialization()
		{
			await Register(new ConfigService() as IConfigService).Init();
			await Register(new InputService() as IInputService).Init();
			await Register(new AssetService(GetService<IConfigService>().AssetServiceConfig) as IAssetService).Init();
			await Register(new AudioService(GetService<IConfigService>().AudioServiceConfig) as IAudioService).Init();
			await Register(new FactoryService(GetService<IAssetService>()) as IFactoryService).Init();
			await Register(new FirebaseService() as IFirebaseService).Init();
			await Register(new GameStateService(GetService<IFirebaseService>()) as IGameStateService).Init();

			_isInitialized = true;
		}

		private TService Register<TService>(TService serviceInstance) where TService : IService =>
			ServiceInstance<TService>.Instance = serviceInstance;

		public TService GetService<TService>() where TService : IService =>
			ServiceInstance<TService>.Instance;

		private static class ServiceInstance<TService> where TService : IService
		{
			public static TService Instance;
		}
	}
}