using System;
using CodeBase.Data;
using CodeBase.Services;
using CodeBase.Services.Factory;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy {
	public class AgentMoveToPlayer : Follow {
		public NavMeshAgent Agent;
		public float MinimalDistance = 1;

		private Transform _heroTransform;
		private IGameFactory _gameFactory;

		private void Start() {
			_gameFactory = AllServices.Container.Single<IGameFactory>();

			if (_gameFactory.HeroGameObject != null)
				InitializeHeroTransform();
			else
				_gameFactory.HeroCreated += InitializeHeroTransform;
		}

		private void Update() {
			if (IsInitialized() && IsHeroNotReached())
				Agent.destination = _heroTransform.position;
		}

		private void OnDestroy() {
			if (_gameFactory != null)
				_gameFactory.HeroCreated -= InitializeHeroTransform;
		}

		private bool IsInitialized() =>
			_heroTransform != null;

		private void InitializeHeroTransform() =>
			_heroTransform = _gameFactory.HeroGameObject.transform;

		private bool IsHeroNotReached() =>
			Agent.transform.position.SqrMagnitudeTo(_heroTransform.position) >= MinimalDistance;
	}
}