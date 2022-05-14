using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Data.Triggers;
using CodeBase.Enemy;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoadService;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services.Factory {
	public interface IGameFactory : IService {
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }
		GameObject CreateHud(GameObject hero);
		GameObject CreateHero(TransformData at);
		SpawnPoint CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
		GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
		LootPiece CreateLoot();
		void CreateSaveTrigger(TriggerData at, ISaveLoadService saveLoadService);
		void CreateLevelTransferTrigger(LevelTransferTriggerData at);
		void Cleanup();
	}
}