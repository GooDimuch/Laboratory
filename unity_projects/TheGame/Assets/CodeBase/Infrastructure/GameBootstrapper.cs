﻿using CodeBase.Logic;
using CodeBase.Services.GameStateMachine.States;
using UnityEngine;

namespace CodeBase.Infrastructure {
	public class GameBootstrapper : MonoBehaviour, ICoroutineRunner {
		public LoadingCurtain curtainPrefab;
		private Game _game;

		private void Awake() {
			_game = new Game(this, Instantiate(curtainPrefab));
			_game.StateMachine.Enter<BootstrapState>();

			DontDestroyOnLoad(this);
		}
	}
}