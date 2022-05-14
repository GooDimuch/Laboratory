using System.Collections.Generic;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.StaticData {
	[CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
	public class LevelStaticData : ScriptableObject {
		public string LevelKey;
		public TransformData InitialHeroTransform;
		public List<EnemySpawnerStaticData> EnemySpawners;
	}
}