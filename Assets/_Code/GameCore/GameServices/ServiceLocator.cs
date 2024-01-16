using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCore
{
	public sealed class ServiceLocator
	{
		private static ServiceLocator _instance;
		public static ServiceLocator Container => _instance ??= new ServiceLocator();

		private Dictionary<IService, Type> _services;

		public async Task<ServiceLocator> InitServices()
		{
			await Register(new ConfigService())
				.Init();

			await Register(new AssetService(GetService<ConfigService>()))
				.Init();

			await Register(new InputService())
				.Init();

			return this;
		}

		private TService Register<TService>(TService serviceInstance) where TService : IService =>
			ServiceInstance<TService>.Instance = serviceInstance;

		private TService GetService<TService>() where TService : IService =>
			ServiceInstance<TService>.Instance;

		private static class ServiceInstance<TService> where TService : IService
		{
			public static TService Instance;
		}
	}
}