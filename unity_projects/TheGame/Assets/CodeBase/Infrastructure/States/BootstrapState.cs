﻿using CodeBase.Services;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.Factory;
using CodeBase.Services.Input;
using CodeBase.Services.PersistantProgress;
using CodeBase.Services.SaveLoadService;
using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.States {
	public class BootstrapState : IState {
		private const string INITIAL = "BootScene";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly AllServices _services;

		public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services) {
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
			_services = services;

			RegisterServices();
		}

		public void Enter() {
			_sceneLoader.Load(INITIAL, onLoaded: EnterLoadLevel);
		}

		public void Exit() { }

		private void EnterLoadLevel() =>
			_stateMachine.Enter<LoadProgressState>();

		private void RegisterServices() {
			RegisterStaticData();
			_services.RegisterSingle<IInputService>(InputService());
			_services.RegisterSingle<IAsset>(new Asset());
			_services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAsset>(),
				_services.Single<IStaticDataService>()));
			_services.RegisterSingle<IPersistantProgressService>(new PersistantProgressService());
			_services.RegisterSingle<ISaveLoadService>(new SaveLoadService(
				_services.Single<IPersistantProgressService>(), _services.Single<IGameFactory>()));
		}

		private void RegisterStaticData() {
			IStaticDataService staticData = new StaticDataService();
			staticData.LoadMonsters();
			_services.RegisterSingle(staticData);
		}

		private static IInputService InputService() {
			if (Application.installMode == ApplicationInstallMode.Editor)
				return new StandaloneInputService();
			else
				return new MobileInputService();
		}
	}
}