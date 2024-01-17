using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI;

namespace GameCore
{
	public sealed class ServiceLocator
	{
		private static ServiceLocator _instance;
		public static ServiceLocator Container => _instance ??= new ServiceLocator();

		private Dictionary<IService, Type> _services;

		public async Task InitServices()
		{
			await Register(new ConfigService())
				.Init();

			await Register(new InputService())
				.Init();

			await Register(new AssetService(GetService<ConfigService>().AssetServiceConfig))
				.Init();

			await Register(new FactoryService(GetService<AssetService>()))
				.Init();
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