using CameraLogic;
using Logic;
using UnityEngine;

namespace Infrastructure {
	public class LoadLevelState : IPayloadedState<string> {
		private const string HERO_PATH = "Hero/Raptor";
		private const string INITIAL_POINT_TAG = "InitialPoint";
		private const string HUD_PATH = "Hud/Hud";
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain) {
			_stateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
		}

		public void Enter(string sceneName) {
			_loadingCurtain.Show();
			_sceneLoader.Load(sceneName, OnLoaded);
		}

		public void Exit() =>
			_loadingCurtain.Hide();

		private void OnLoaded() {
			var initialPoint = GameObject.FindWithTag(INITIAL_POINT_TAG);
			if (initialPoint != null) {
				var hero = Instantiate(path: HERO_PATH, at: initialPoint.transform.position,
					with: initialPoint.transform.rotation);
				Instantiate(HUD_PATH);

				CameraFollow(hero);
			}

			_stateMachine.Enter<GameLoopState>();
		}

		private void CameraFollow(GameObject hero) =>
			Camera.main.GetComponent<CameraFollow>().Follow(hero);

		private static GameObject Instantiate(string path) =>
			Instantiate(path, Vector3.zero, Quaternion.identity);

		private static GameObject Instantiate(string path, Vector3 at) =>
			Instantiate(path, at, Quaternion.identity);

		private static GameObject Instantiate(string path, Vector3 at, Quaternion with) {
			var prefab = Resources.Load<GameObject>(path);
			return Object.Instantiate(prefab, at, with);
		}
	}
}