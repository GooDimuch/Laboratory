using CodeBase.CameraLogic;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Factory;
using CodeBase.Services.PersistantProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.States {
	public class LoadLevelState : IPayloadedState<string> {
		private const string INITIAL_POINT_TAG = "InitialPoint";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		private readonly IGameFactory _gameFactory;
		private readonly IPersistantProgressService _progressService;

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
			IGameFactory gameFactory, IPersistantProgressService progressService) {
			_stateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_gameFactory = gameFactory;
			_progressService = progressService;
		}

		public void Enter(string sceneName) {
			_loadingCurtain.Show();
			_gameFactory.Cleanup();
			_sceneLoader.Load(sceneName, OnLoaded);
		}

		public void Exit() =>
			_loadingCurtain.Hide();

		private void OnLoaded() {
			InitGameWorld();
			LoadProgress();

			_stateMachine.Enter<GameLoopState>();
		}

		private void LoadProgress() {
			foreach (var progressReader in _gameFactory.ProgressReaders)
				progressReader.LoadProgress(_progressService.Progress);
		}

		private void InitGameWorld() {
			_gameFactory.CreateHud();
			var hero = _gameFactory.CreateHero(GameObject.FindWithTag(INITIAL_POINT_TAG));
			CameraFollow(hero);
		}

		private void CameraFollow(GameObject hero) =>
			Camera.main.GetComponent<CameraFollow>().Follow(hero);
	}
}