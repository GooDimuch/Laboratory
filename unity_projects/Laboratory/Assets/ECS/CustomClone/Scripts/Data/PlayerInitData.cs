using UnityEngine;

namespace ECS.CustomClone {
	[CreateAssetMenu]
	public class PlayerInitData : ScriptableObject {
		public GameObject playerPrefab;
		public float defaultSpeed = 2f;

		public static PlayerInitData _playerData;

		public static PlayerInitData PlayerData {
			get {
				if (_playerData == null) { _playerData = Resources.Load("Data/PlayerData") as PlayerInitData; }
				return _playerData;
			}
		}
	}
}