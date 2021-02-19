using ECS.CustomClone.Component;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.CustomClone.Systems {
	public class PlayerMoveSystem : IEcsRunSystem {
		private EcsFilter<MovableComponent, InputEventComponent> _playerMoveFilter;
		private bool is3D = false;

		public void Run() {
			foreach (var i in _playerMoveFilter) {
				ref var movableComponent = ref _playerMoveFilter.Get1(i);
				ref var inputComponent = ref _playerMoveFilter.Get2(i);

				var direction = is3D
						? new Vector3(inputComponent.direction.x, 0, inputComponent.direction.y)
						: (Vector3) inputComponent.direction;

				movableComponent.transform.position += direction.normalized * Time.deltaTime * movableComponent.moveSpeed;
				movableComponent.transform.localRotation = Quaternion.Euler(Vector3.left * 90);
				movableComponent.isMoving = direction.sqrMagnitude > 0;
			}
		}
	}
}