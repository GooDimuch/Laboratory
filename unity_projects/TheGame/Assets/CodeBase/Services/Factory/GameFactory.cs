using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Data.Triggers;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.Triggers;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.GameStateMachine;
using CodeBase.Services.GameStateMachine.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoadService;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.WindowService;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Services.Factory {
	public class GameFactory : IGameFactory {
		private readonly IAsset _asset;
		private readonly IStaticDataService _staticData;
		private readonly IRandomService _randomizer;
		private readonly IPersistentProgressService _progressService;
		private readonly IWindowService _windowService;
		private readonly IGameStateMachine _stateMachine;

		public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
		public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
		private Transform HeroTransform { get; set; }

		public GameFactory(IAsset asset, IStaticDataService staticData, IRandomService randomizer,
			IPersistentProgressService progressService, IWindowService windowService, IGameStateMachine stateMachine) {
			_asset = asset;
			_staticData = staticData;
			_randomizer = randomizer;
			_progressService = progressService;
			_windowService = windowService;
			_stateMachine = stateMachine;
		}

		public GameObject CreateHud(GameObject hero) {
			var hud = InstantiateRegistered(AssetPath.HUD_PATH);
			hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
			hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);
			foreach (var windowButton in hud.GetComponentsInChildren<OpenWindowButton>())
				windowButton.Construct(_windowService);
			return hud;
		}

		public GameObject CreateHero(TransformData at) {
			var heroGameObject = InstantiateRegistered(AssetPath.HERO_PATH, at.position.AsUnityVector(), at.rotation);
			HeroTransform = heroGameObject.transform;
			return heroGameObject;
		}

		public SpawnPoint CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId) {
			var spawner = InstantiateRegistered(AssetPath.SPAWNER_PATH, at, Quaternion.identity)
				.GetComponent<SpawnPoint>().Construct(spawnerId, monsterTypeId, this);
			return spawner;
		}

		public GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent) {
			var monsterData = _staticData.ForMonster(monsterTypeId);
			var monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

			var health = monster.GetComponent<IHealth>();
			health.Current = monsterData.Hp;
			health.Max = monsterData.Hp;

			var attack = monster.GetComponent<Attack>();
			attack.Construct(HeroTransform);
			attack.Damage = monsterData.Damage;
			attack.Cleavage = monsterData.Cleavage;
			attack.EffectiveDistance = monsterData.EffectiveDistance;

			var lootSpawner = monster.GetComponentInChildren<LootSpawner>();
			lootSpawner.Construct(this, _randomizer);
			lootSpawner.SetLootValue(monsterData.LootMin, monsterData.LootMax);

			monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;
			monster.GetComponent<ActorUI>().Construct(health);
			monster.GetComponent<AgentMoveToPlayer>().Constract(HeroTransform);
			monster.GetComponent<RotateToHero>()?.Constract(HeroTransform);

			return monster;
		}

		public LootPiece CreateLoot() {
			var lootPiece = InstantiateRegistered(AssetPath.LOOT_PATH).GetComponent<LootPiece>();
			lootPiece.Construct(_progressService.Progress.WorldData);
			return lootPiece;
		}

		public void CreateSaveTrigger(TriggerData at, ISaveLoadService saveLoadService) {
			var trigger = InstantiateRegistered(AssetPath.SAVE_TRIGGER_PATH, at.transform.position.AsUnityVector(),
				at.transform.rotation).GetComponent<SaveTrigger>();
			var collider = trigger.GetComponent<BoxCollider>();
			collider.size = at.size.AsUnityVector();
			trigger.Construct(saveLoadService);
		}

		public void CreateLevelTransferTrigger(LevelTransferTriggerData at) {
			var trigger =
				InstantiateRegistered(AssetPath.LEVEL_TRANSFER_TRIGGER_PATH, at.transform.position.AsUnityVector(),
					at.transform.rotation).GetComponent<LevelTransferTrigger>();
			var collider = trigger.GetComponent<BoxCollider>();
			collider.size = at.size.AsUnityVector();
			trigger.Construct(_stateMachine, LoadLevelState.GetLevelByName(at.TransferTo));
		}

		private GameObject InstantiateRegistered(string prefabPath) =>
			InstantiateRegistered(prefabPath, Vector3.zero, Quaternion.identity);

		private GameObject InstantiateRegistered(string prefabPath, Vector3 at, Quaternion with) {
			var gameObject = _asset.Instantiate(path: prefabPath, at: at, with: with);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		public void Cleanup() {
			ProgressReaders.Clear();
			ProgressWriters.Clear();
		}

		private void RegisterProgressWatchers(GameObject gameObject) {
			foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				RegisterProgressWatcher(progressReader);
		}

		private void RegisterProgressWatcher(ISavedProgressReader progressReader) {
			if (progressReader is ISavedProgress progressWriter)
				ProgressWriters.Add(progressWriter);
			ProgressReaders.Add(progressReader);
		}
	}
}