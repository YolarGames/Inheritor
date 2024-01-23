using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.GameServices
{
	public sealed class ServiceLocator
	{
		private bool _isInitialized;
		private static ServiceLocator _instance;
		public static ServiceLocator Container => _instance ??= new ServiceLocator();
		private static Task _initTask;

		private Dictionary<IService, Type> _services;

		public async Task InitServices()
		{
			if (_isInitialized)
				return;

			Debug.Log($"{GetType().Name} start service initialization");

			_initTask = StartServiceInitialization();

			while (!_initTask.IsCompleted)
				await _initTask;

			Debug.Log($"{GetType().Name} finish service initialization");
		}

		public async Task StartServiceInitialization()
		{
			await Register(new ConfigService())
				.Init();
			Debug.Log("ConfigService initialized");

			await Register(new InputService())
				.Init();
			Debug.Log("InputService initialized");

			await Register(new AssetService(GetService<ConfigService>().AssetServiceConfig))
				.Init();
			Debug.Log("AssetService initialized");

			await Register(new FactoryService(GetService<AssetService>()))
				.Init();
			Debug.Log("FactoryService initialized");

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