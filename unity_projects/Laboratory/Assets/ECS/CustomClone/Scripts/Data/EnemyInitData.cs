using UnityEngine;

namespace ECS.CustomClone {
	[CreateAssetMenu]
	public class EnemyInitData : ScriptableObject {
		public GameObject enemyPrefab;
		public float defaultSpeed = 1f;

		public static EnemyInitData _enemyData;

		public static EnemyInitData EnemyData {
			get {
				if (_enemyData == null) { _enemyData = Resources.Load("Data/EnemyData") as EnemyInitData; }
				return _enemyData;
			}
		}
	}
}