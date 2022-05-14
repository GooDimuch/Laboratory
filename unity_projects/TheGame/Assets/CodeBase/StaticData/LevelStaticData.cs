using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Data.Triggers;
using UnityEngine;

namespace CodeBase.StaticData {
	[CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
	public class LevelStaticData : ScriptableObject {
		public string LevelKey;
		public TransformData InitialHeroTransform;
		public LevelTransferTriggerData NextLevelTriggerTransform;
		public List<TriggerData> SaveTriggerTransforms;
		public List<EnemySpawnerStaticData> EnemySpawners;
	}
}