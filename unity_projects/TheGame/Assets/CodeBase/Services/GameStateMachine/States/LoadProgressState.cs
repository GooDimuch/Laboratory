using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoadService;

namespace CodeBase.Services.GameStateMachine.States {
	public class LoadProgressState : IState {
		private readonly GameStateMachine _gameStateMachine;
		private readonly IPersistentProgressService _progressService;
		private readonly ISaveLoadService _saveLoadService;

		public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService,
			ISaveLoadService saveLoadService) {
			_gameStateMachine = gameStateMachine;
			_progressService = progressService;
			_saveLoadService = saveLoadService;
		}

		public void Enter() {
			LoadProgressOrNew();
			_gameStateMachine.Enter<LoadLevelState, LoadLevelState.Level>(
				LoadLevelState.GetLevelByName(_progressService.Progress.WorldData.PositionOnLevel.Level));
		}

		public void Exit() { }

		private void LoadProgressOrNew() {
			_progressService.Progress = _saveLoadService.LoadProgress() ?? CreateNewProgress();
		}

		private PlayerProgress CreateNewProgress() {
			var progress = new PlayerProgress(LoadLevelState.Level.Level_1.ToString());

			progress.HeroState.maxHp = 100;
			progress.HeroState.ResetHp();

			progress.HeroStats.Damage = 50;
			progress.HeroStats.DamageRadius = 2;

			return progress;
		}
	}
}