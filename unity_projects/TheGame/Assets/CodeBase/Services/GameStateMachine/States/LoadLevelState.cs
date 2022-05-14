using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Data.Loot;
using CodeBase.Enemy;
using CodeBase.Infrastructure;
using CodeBase.Logic;
using CodeBase.Services.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoadService;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Services.UIFactory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Services.GameStateMachine.States {
	public class LoadLevelState : IPayloadedState<LoadLevelState.Level> {
		public enum Level {
			Level_1,
			Level_2,
		}

		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IGameFactory _gameFactory;
		private readonly IPersistentProgressService _progressService;
		private readonly IStaticDataService _staticDataService;
		private readonly IUIFactory _uiFactory;
		private readonly ISaveLoadService _saveLoadService;

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
			IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticDataService,
			IUIFactory uiFactory, ISaveLoadService saveLoadService) {
			_stateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_gameFactory = gameFactory;
			_progressService = progressService;
			_staticDataService = staticDataService;
			_uiFactory = uiFactory;
			_saveLoadService = saveLoadService;
		}

		public void Enter(Level level) {
			_loadingCurtain.Show();
			_gameFactory.Cleanup();
			_gameFactory.WarmUp();
			_sceneLoader.Load(level.ToString(), OnLoaded);
		}

		public void Exit() =>
			_loadingCurtain.Hide();

		public static Level GetLevelByName(string name) => Enum.TryParse(name, out Level level)
			? level
			: throw new ArgumentException($"Level with name '{name}' not found");

		private async void OnLoaded() {
			InitUI();
			await InitGameWorld();
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

		private async Task InitGameWorld() {
			var levelData = LevelStaticData();

			InitSaveTriggers(levelData);
			InitLevelTransferTrigger(levelData);
			await InitEnemySpawners(levelData);
			await InitDroppedLoot();
			var hero = InitHero(levelData);
			_gameFactory.CreateHud(hero);
			CameraFollow(hero);
		}

		private void InitSaveTriggers(LevelStaticData levelData) {
			foreach (var triggerTransform in levelData.SaveTriggerTransforms)
				_gameFactory.CreateSaveTrigger(triggerTransform, _saveLoadService);
		}

		private void InitLevelTransferTrigger(LevelStaticData levelData) =>
			_gameFactory.CreateLevelTransferTrigger(levelData.NextLevelTriggerTransform);

		private async Task InitEnemySpawners(LevelStaticData levelData) {
			foreach (var spawnerData in levelData.EnemySpawners)
				await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
		}

		private async Task InitDroppedLoot() {
			foreach (var item in _progressService.Progress.WorldData.LootData.LootPiecesOnScene.Dictionary) {
				var lootPiece = await _gameFactory.CreateLoot();
				lootPiece.GetComponent<UniqueId>().Id = item.Key;
				lootPiece.Initialize(item.Value.Loot);
				lootPiece.transform.position = item.Value.Position.AsUnityVector();
			}
		}

		private GameObject InitHero(LevelStaticData levelData) =>
			_gameFactory.CreateHero(levelData.InitialHeroTransform);

		private LevelStaticData LevelStaticData() =>
			_staticDataService.ForLevel(SceneManager.GetActiveScene().name);

		private void CameraFollow(GameObject hero) =>
			Camera.main.GetComponent<CameraFollow>().Follow(hero);
	}
}