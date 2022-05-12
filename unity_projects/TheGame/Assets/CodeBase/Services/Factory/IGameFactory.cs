using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services.Factory {
	public interface IGameFactory : IService {
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }
		GameObject CreateHud(GameObject hero);
		GameObject CreateHero(GameObject at);
		SpawnPoint CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
		GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
		LootPiece CreateLoot();
		void Cleanup();
	}
}