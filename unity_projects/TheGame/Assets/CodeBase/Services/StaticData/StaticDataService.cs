﻿using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Services.StaticData {
	public class StaticDataService : IStaticDataService {
		private const string MONSTERS_DATA_PATH = "StaticData/Monsters";
		private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;

		public void LoadMonsters() {
			_monsters = Resources
				.LoadAll<MonsterStaticData>(MONSTERS_DATA_PATH)
				.ToDictionary(x => x.MonsterTypeId, x => x);
		}

		public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
			_monsters.TryGetValue(typeId, out var staticData)
				? staticData
				: null;
	}
}