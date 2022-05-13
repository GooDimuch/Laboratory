using CodeBase.Data;
using CodeBase.Services.Factory;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.SaveLoadService {
	public class SaveLoadService : ISaveLoadService {
		private const string PROGRESS_KEY = "Progress";

		private readonly IPersistentProgressService _progressService;
		private readonly IGameFactory _gameFactory;

		public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory) {
			_progressService = progressService;
			_gameFactory = gameFactory;
		}

		public void SaveProgress() {
			Debug.Log("SaveProgress");
			foreach (var progressWriter in _gameFactory.ProgressWriters)
				progressWriter.UpdateProgress(_progressService.Progress);

			PlayerPrefs.SetString(PROGRESS_KEY, _progressService.Progress.ToJson());
		}

		public PlayerProgress LoadProgress() {
			Debug.Log("LoadProgress");
			return PlayerPrefs.GetString(PROGRESS_KEY)?.ToDeserialize<PlayerProgress>();
		}
	}
}