using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
		private readonly IAssetProvider _assetProvider;
		private readonly IStaticDataService _staticData;
		private readonly IRandomService _randomizer;
		private readonly IPersistentProgressService _progressService;
		private readonly IWindowService _windowService;
		private readonly IGameStateMachine _stateMachine;

		public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
		public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
		private Transform HeroTransform { get; set; }

		public GameFactory(IAssetProvider assetProvider, IStaticDataService staticData, IRandomService randomizer,
			IPersistentProgressService progressService, IWindowService windowService, IGameStateMachine stateMachine) {
			_assetProvider = assetProvider;
			_staticData = staticData;
			_randomizer = randomizer;
			_progressService = progressService;
			_windowService = windowService;
			_stateMachine = stateMachine;
		}

		public async Task WarmUp() {
			await _assetProvider.Load<GameObject>(AssetAddress.LOOT_ADDRESS);
			await _assetProvider.Load<GameObject>(AssetAddress.SPAWNER_ADDRESS);
		}

		public async Task<GameObject> CreateHud(GameObject hero) {
			var hud = await InstantiateRegisteredAsync(AssetAddress.HUD_ADDRESS);
			hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
			hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);
			foreach (var windowButton in hud.GetComponentsInChildren<OpenWindowButton>())
				windowButton.Construct(_windowService);
			return hud;
		}

		public async Task<GameObject> CreateHero(TransformData at) {
			var heroGameObject =
				await InstantiateRegisteredAsync(AssetAddress.HERO_ADDRESS, at.position.AsUnityVector(), at.rotation);
			HeroTransform = heroGameObject.transform;
			return heroGameObject;
		}

		public async Task<SpawnPoint> CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId) {
			var prefab = await _assetProvider.Load<GameObject>(AssetAddress.SPAWNER_ADDRESS);
			var spawner = InstantiateRegistered(prefab, at, Quaternion.identity)
				.GetComponent<SpawnPoint>().Construct(spawnerId, monsterTypeId, this);
			return spawner;
		}

		public async Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent) {
			var monsterData = _staticData.ForMonster(monsterTypeId);

			var prefab = await _assetProvider.Load<GameObject>(monsterData.PrefabReference);
			var monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

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

		public async Task<LootPiece> CreateLoot() {
			var prefab = await _assetProvider.Load<GameObject>(AssetAddress.LOOT_ADDRESS);
			var lootPiece = InstantiateRegistered(prefab).GetComponent<LootPiece>();
			lootPiece.Construct(_progressService.Progress.WorldData);
			return lootPiece;
		}

		public async Task CreateSaveTrigger(TriggerData at, ISaveLoadService saveLoadService) {
			var triggerGameObject = await InstantiateRegisteredAsync(AssetAddress.SAVE_TRIGGER_ADDRESS,
				at.transform.position.AsUnityVector(), at.transform.rotation);
			var trigger = triggerGameObject.GetComponent<SaveTrigger>();
			var collider = trigger.GetComponent<BoxCollider>();
			collider.size = at.size.AsUnityVector();
			trigger.Construct(saveLoadService);
		}

		public async Task CreateLevelTransferTrigger(LevelTransferTriggerData at) {
			var triggerGameObject = await InstantiateRegisteredAsync(AssetAddress.LEVEL_TRANSFER_TRIGGER_ADDRESS,
				at.transform.position.AsUnityVector(), at.transform.rotation);
			var trigger = triggerGameObject.GetComponent<LevelTransferTrigger>();
			var collider = trigger.GetComponent<BoxCollider>();
			collider.size = at.size.AsUnityVector();
			trigger.Construct(_stateMachine, LoadLevelState.GetLevelByName(at.TransferTo));
		}

		public void Cleanup() {
			ProgressReaders.Clear();
			ProgressWriters.Clear();
			_assetProvider.Cleanup();
		}

		private Task<GameObject> InstantiateRegisteredAsync(string address) =>
			InstantiateRegisteredAsync(address, Vector3.zero, Quaternion.identity);

		private async Task<GameObject> InstantiateRegisteredAsync(string address, Vector3 at, Quaternion with) {
			var gameObject = await _assetProvider.InstantiateAsync(address: address, at: at, with: with);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		private GameObject InstantiateRegistered(GameObject prefab) =>
			InstantiateRegistered(prefab, Vector3.zero, Quaternion.identity);

		private GameObject InstantiateRegistered(GameObject prefab, Vector3 at, Quaternion with) {
			var gameObject = Object.Instantiate(prefab, at, with);
			RegisterProgressWatchers(gameObject);
			return gameObject;
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