using ECS.CustomClone.Component;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.CustomClone.Systems {
	public class PlayerInputSystem : IEcsRunSystem {
		private EcsFilter<InputEventComponent> _inputEventsFilter;

		public void Run() {
			foreach (var i in _inputEventsFilter) {
				ref var inputEvent = ref _inputEventsFilter.Get1(i);

				inputEvent.direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			}
		}
	}
}