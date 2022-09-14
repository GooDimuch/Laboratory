﻿using CodeBase.CameraLogic;
using CodeBase.Logic;
using CodeBase.Services.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.UIFactory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States {
	public class LoadLevelState : IPayloadedState<string> {
		private const string INITIAL_POINT_TAG = "InitialPoint";
		private const string ENEMY_SPAWNER_TAG = "EnemySpawner";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IGameFactory _gameFactory;
		private readonly IPersistentProgressService _progressService;
		private readonly IStaticDataService _staticDataService;
		private readonly IUIFactory _uiFactory;

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
			IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticDataService,
			IUIFactory uiFactory) {
			_stateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_gameFactory = gameFactory;
			_progressService = progressService;
			_staticDataService = staticDataService;
			_uiFactory = uiFactory;
		}

		public void Enter(string sceneName) {
			_loadingCurtain.Show();
			_gameFactory.Cleanup();
			_sceneLoader.Load(sceneName, OnLoaded);
		}

		public void Exit() =>
			_loadingCurtain.Hide();

		private void OnLoaded() {
			InitUI();
			InitGameWorld();
			LoadProgress();

			_stateMachine.Enter<GameLoopState>();
		}

		private void LoadProgress() {
			foreach (var progressReader in _gameFactory.ProgressReaders)
				progressReader.LoadProgress(_progressService.Progress);
		}

		private void InitUI() {
			_uiFactory.CreateUIRoot();
		}

		private void InitGameWorld() {
			InitEnemySpawners();
			var hero = InitHero();
			_gameFactory.CreateHud(hero);
			CameraFollow(hero);
		}

		private void InitEnemySpawners() {
			var sceneKey = SceneManager.GetActiveScene().name;
			var levelData = _staticDataService.ForLevel(sceneKey);
			foreach (var spawnerData in levelData.EnemySpawners) {
				_gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
			}
		}

		private GameObject InitHero() => _gameFactory.CreateHero(GameObject.FindWithTag(INITIAL_POINT_TAG));

		private void CameraFollow(GameObject hero) =>
			Camera.main.GetComponent<CameraFollow>().Follow(hero);
	}
}