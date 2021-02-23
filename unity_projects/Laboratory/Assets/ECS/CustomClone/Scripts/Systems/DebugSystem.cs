using ECS.CustomClone.Component;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.CustomClone.Systems {
	public class DebugSystem : IEcsRunSystem {
		private EcsFilter<MovableComponent> _movablesFilter;
		private EcsFilter<InputEventComponent> _inputEventsFilter;

		public void Run() {
			foreach (var i in _movablesFilter) {
				ref var inputEvent = ref _movablesFilter.Get1(i);
				Debug.Log(inputEvent);
			}
			foreach (var i in _inputEventsFilter) {
				ref var inputEvent = ref _inputEventsFilter.Get1(i);
				Debug.Log(inputEvent);
			}
		}
	}
}