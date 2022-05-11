using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Services;
using CodeBase.Services.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic {
	public class EnemySpawner : MonoBehaviour, ISavedProgress {
		public MonsterTypeId MonsterTypeId;
		public bool Slain;
		private string _id;
		private IGameFactory _factory;
		private EnemyDeath _enemyDeath;

		private void Awake() {
			_id = GetComponent<UniqueId>().Id;
			_factory = AllServices.Container.Single<IGameFactory>();
		}


		public void LoadProgress(PlayerProgress progress) {
			if (progress.KillData.ClearedSpawners.Contains(_id))
				Slain = true;
			else
				Spawn();
		}

		public void UpdateProgress(PlayerProgress progress) {
			if (Slain)
				progress.KillData.ClearedSpawners.Add(_id);
		}

		private void Spawn() {
			var monster = _factory.CreateMonster(MonsterTypeId, transform);
			_enemyDeath = monster.GetComponent<EnemyDeath>();
			_enemyDeath.Happened += Slay;
		}

		private void Slay() {
			if (_enemyDeath)
				_enemyDeath.Happened -= Slay;
			Slain = true;
		}
	}
}