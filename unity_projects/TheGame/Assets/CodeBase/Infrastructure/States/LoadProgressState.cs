using CodeBase.Data;
using CodeBase.Services.PersistantProgress;
using CodeBase.Services.SaveLoadService;

namespace CodeBase.Infrastructure.States {
	public class LoadProgressState : IState {
		private const string GAME_SCENE = "GameScene";

		private readonly GameStateMachine _gameStateMachine;
		private readonly IPersistantProgressService _progressService;
		private readonly ISaveLoadService _saveLoadService;

		public LoadProgressState(GameStateMachine gameStateMachine, IPersistantProgressService progressService,
			ISaveLoadService saveLoadService) {
			_gameStateMachine = gameStateMachine;
			_progressService = progressService;
			_saveLoadService = saveLoadService;
		}

		public void Enter() {
			LoadProgressOrNew();
			_gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
		}

		public void Exit() { }

		private void LoadProgressOrNew() {
			_progressService.Progress = _saveLoadService.LoadProgress() ?? CreateNewProgress();
		}

		private PlayerProgress CreateNewProgress() {
			var progress = new PlayerProgress(GAME_SCENE);
			
			progress.HeroState.maxHp = 100;
			progress.HeroState.ResetHp();

			progress.HeroStats.Damage = 50;
			progress.HeroStats.DamageRadius = 2;
			
			return progress;
		}
	}
}