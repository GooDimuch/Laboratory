using Services.Input;
using UnityEngine;

namespace Infrastructure {
	public class BootstrapState : IState {
		private const string INITIAL = "BootScene";
		private const string GAME_SCENE = "GameScene";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;

		public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader) {
			_stateMachine = stateMachine;
			_sceneLoader = sceneLoader;
		}

		public void Enter() {
			RegisterServices();
			_sceneLoader.Load(INITIAL, onLoaded: EnterLoadLevel);
		}

		public void Exit() { }

		private void EnterLoadLevel() =>
			_stateMachine.Enter<LoadLevelState, string>(GAME_SCENE);

		private void RegisterServices() {
			Game.InputService = RegisterInputService();
		}

		private static IInputService RegisterInputService() {
			if (Application.isEditor)
				return new StandaloneInputService();
			else
				return new MobileInputService();
		}
	}
}