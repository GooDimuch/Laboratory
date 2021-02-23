using ECS.CustomClone.Systems;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.CustomClone {
	public class Loader : MonoBehaviour {
		public static Loader Instance { get; private set; }
		private EcsWorld _world;
		private EcsSystems _systems;
		[Range(0, 1000000)] public int _enemyOfNumber = 1;
		public int EnemyOfNumber => _enemyOfNumber;

		private void Awake() { Instance = this; }

		private void Start() {
			_world = new EcsWorld();
			_systems = new EcsSystems(_world);

#if UNITY_EDITOR
			Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
			Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif

			_systems.Add(new GameInitSystem());
			_systems.Add(new PlayerInputSystem());
			_systems.Add(new PlayerMoveSystem());
			_systems.Add(new FollowSystem());
			// _systems.Add(new DebugSystem());

			_systems.Init();
		}

		private void Update() {
			_systems.Run();

			if (Input.GetKeyDown(KeyCode.Equals)) { _enemyOfNumber += 10; }
			if (Input.GetKeyDown(KeyCode.Minus)) { _enemyOfNumber -= 10; }

			_enemyOfNumber = Mathf.Clamp(_enemyOfNumber, 0, _enemyOfNumber);
		}

		private void OnDestroy() {
			_systems.Destroy();
			_world.Destroy();
		}
	}
}