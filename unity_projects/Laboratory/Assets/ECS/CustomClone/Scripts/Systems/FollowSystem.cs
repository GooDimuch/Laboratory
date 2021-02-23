using ECS.CustomClone.Component;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.CustomClone.Systems {
	public class FollowSystem : IEcsRunSystem {
		private EcsFilter<FollowComponent, MovableComponent> _enemyFollowFilter;

		public void Run() {
			foreach (var i in _enemyFollowFilter) {
				ref var followComponent = ref _enemyFollowFilter.Get1(i);
				ref var movableComponent = ref _enemyFollowFilter.Get2(i);

				var direction = followComponent.target.position - movableComponent.transform.position;
				movableComponent.transform.position +=
						direction.normalized * Time.deltaTime * movableComponent.moveSpeed;
				movableComponent.transform.localRotation = Quaternion.Euler(Vector3.left * 90);
				movableComponent.isMoving = direction.sqrMagnitude > 0;
			}
		}
	}
}