using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.UI.Services.WindowService;
using UnityEngine;

namespace CodeBase.Services.StaticData {
	public class StaticDataService : IStaticDataService {
		private const string MONSTERS_DATA_PATH = "StaticData/Monsters";
		private const string LEVELS_DATA_PATH = "StaticData/Levels";
		private const string WINDOW_CONFIGS_PATH = "StaticData/UI/WindowsData";

		private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
		private Dictionary<string, LevelStaticData> _levels;
		private Dictionary<WindowId, WindowConfig> _windowConfigs;

		public void LoadMonsters() {
			_monsters = Resources
				.LoadAll<MonsterStaticData>(MONSTERS_DATA_PATH)
				.ToDictionary(x => x.MonsterTypeId, x => x);
			_levels = Resources
				.LoadAll<LevelStaticData>(LEVELS_DATA_PATH)
				.ToDictionary(x => x.LevelKey, x => x);
			_windowConfigs = Resources
				.Load<WindowStaticData>(WINDOW_CONFIGS_PATH).WindowConfigs
				.ToDictionary(x => x.WindowId, x => x);
		}

		public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
			GetDictionaryValue(_monsters, typeId);

		public LevelStaticData ForLevel(string sceneKey) =>
			GetDictionaryValue(_levels, sceneKey);

		public WindowConfig ForWindow(WindowId windowId) =>
			GetDictionaryValue(_windowConfigs, windowId);

		private static TValue GetDictionaryValue<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
			where TValue : class =>
			dictionary.TryGetValue(key, out var staticData)
				? staticData
				: null;
	}
}