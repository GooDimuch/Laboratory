using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services.StaticData {
	public class StaticDataService : IStaticDataService {
		private const string MONSTERS_DATA_PATH = "StaticData/Monsters";
		private const string LEVELS_DATA_PATH = "StaticData/Levels";
		private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
		private Dictionary<string, LevelStaticData> _levels;

		public void LoadMonsters() {
			_monsters = Resources
				.LoadAll<MonsterStaticData>(MONSTERS_DATA_PATH)
				.ToDictionary(x => x.MonsterTypeId, x => x);
			_levels = Resources
				.LoadAll<LevelStaticData>(LEVELS_DATA_PATH)
				.ToDictionary(x => x.LevelKey, x => x);
		}

		public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
			_monsters.TryGetValue(typeId, out var staticData)
				? staticData
				: null;

		public LevelStaticData ForLevel(string sceneKey) =>
			_levels.TryGetValue(sceneKey, out var staticData)
				? staticData
				: null;
	}
}