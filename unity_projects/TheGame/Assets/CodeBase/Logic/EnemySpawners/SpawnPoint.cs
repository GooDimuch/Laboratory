using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Services.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners {
	public class SpawnPoint : MonoBehaviour, ISavedProgress {
		public string Id { get; private set; }
		public MonsterTypeId MonsterTypeId { get; private set; }

		private IGameFactory _factory;
		private EnemyDeath _enemyDeath;
		private bool _slain;

		public SpawnPoint Construct(string id, MonsterTypeId monsterTypeId, IGameFactory gameFactory) {
			Id = id;
			MonsterTypeId = monsterTypeId;
			_factory = gameFactory;
			return this;
		}

		private void OnDestroy() {
			if (_enemyDeath != null)
				_enemyDeath.Happened -= Slay;
		}

		public void LoadProgress(PlayerProgress progress) {
			if (progress.KillData.ClearedSpawners.Contains(Id))
				_slain = true;
			else
				Spawn();
		}

		public void UpdateProgress(PlayerProgress progress) {
			var slainSpawnersList = progress.KillData.ClearedSpawners;

			if (_slain && !slainSpawnersList.Contains(Id))
				slainSpawnersList.Add(Id);
		}

		private async void Spawn() {
			var monster = await _factory.CreateMonster(MonsterTypeId, transform);
			_enemyDeath = monster.GetComponent<EnemyDeath>();
			_enemyDeath.Happened += Slay;
		}

		private void Slay() {
			if (_enemyDeath != null)
				_enemyDeath.Happened -= Slay;

			_slain = true;
		}
	}
}