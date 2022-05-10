using CodeBase.Data;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy {
	public class AgentMoveToPlayer : Follow {
		public NavMeshAgent Agent;
		public float MinimalDistance = 1;

		private Transform _heroTransform;

		public void Constract(Transform heroTransform) =>
			_heroTransform = heroTransform;

		private void Update() {
			if (IsInitialized() && IsHeroNotReached())
				Agent.destination = _heroTransform.position;
		}

		private bool IsInitialized() =>
			_heroTransform != null;

		private bool IsHeroNotReached() =>
			Agent.transform.position.SqrMagnitudeTo(_heroTransform.position) >= MinimalDistance;
	}
}