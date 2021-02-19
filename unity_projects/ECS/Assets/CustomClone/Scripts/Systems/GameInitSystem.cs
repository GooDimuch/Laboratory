using System;
using System.Collections.Generic;
using ECS.CustomClone.Component;
using Leopotam.Ecs;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ECS.CustomClone.Systems {
	public class GameInitSystem : IEcsInitSystem, IEcsRunSystem {
		private EcsWorld _world;
		private GameObject playerObject;
		private readonly List<EcsEntity> enemyList = new List<EcsEntity>();
		private int enemyIdCounter;

		private int EnemyOfNumber => Loader.Instance.EnemyOfNumber;

		public void Init() {
			var player = _world.NewEntity();
			ref var playerComponent = ref player.Get<PlayerComponent>();
			ref var movableComponent = ref player.Get<MovableComponent>();
			ref var inputComponent = ref player.Get<InputEventComponent>();
			ref var animationsComponent = ref player.Get<AnimatedCharacterComponent>();

			playerComponent.Id = 0;
			playerObject = Object.Instantiate(PlayerInitData.PlayerData.playerPrefab,
					Vector3.zero,
					Quaternion.identity);
			// animationsComponent.animator = spawnedPlayerPrefab.transform.Find("Sprite").GetComponent<Animator>();
			movableComponent.moveSpeed = PlayerInitData.PlayerData.defaultSpeed;
			movableComponent.transform = playerObject.transform;
		}

		public void Run() {
			var diff = EnemyOfNumber - enemyList.Count;
			if (diff > 0) {
				for (var i = 0; i < Math.Abs(diff); i++) { CreateEnemy(GetRandomPosition(), playerObject.transform); }
			}
			else if (diff < 0) {
				for (var i = 0; i < Math.Abs(diff); i++) { RemoveEnemy(); }
			}
		}

		private static Vector3 GetRandomPosition() { return Vector2.one * Random.Range(-2f, 2f); }

		private void CreateEnemy(Vector3 atPosition, Transform target) {
			var enemy = _world.NewEntity();
			enemyList.Add(enemy);
			ref var enemyComponent = ref enemy.Get<EnemyComponent>();
			ref var movableComponent = ref enemy.Get<MovableComponent>();
			ref var animationsComponent = ref enemy.Get<AnimatedCharacterComponent>();
			ref var followComponent = ref enemy.Get<FollowComponent>();

			enemyComponent.Id = enemyIdCounter++;
			var spawnedEnemyPrefab = Object.Instantiate(EnemyInitData.EnemyData.enemyPrefab,
					atPosition,
					Quaternion.identity);

			// animationsComponent.animator = spawnedPlayerPrefab.transform.Find("Sprite").GetComponent<Animator>();
			movableComponent.moveSpeed = EnemyInitData.EnemyData.defaultSpeed;
			movableComponent.transform = spawnedEnemyPrefab.transform;

			followComponent.target = target;
		}

		private void RemoveEnemy() {
			if (enemyList.Count == 0) { return; }
			enemyList[0].Destroy();
			enemyList.RemoveAt(0);
		}
	}
}