using System.Collections.Generic;
using CodeBase.Services.PersistantProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services.Factory {
	public interface IGameFactory : IService {
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }
		GameObject CreateHero(GameObject at);
		void CreateHud(GameObject hero);
		void Cleanup();
		void Register(ISavedProgressReader progressReader);
		GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
	}
}