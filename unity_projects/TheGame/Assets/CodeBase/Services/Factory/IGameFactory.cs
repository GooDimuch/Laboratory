using System.Collections.Generic;
using CodeBase.Services.PersistantProgress;
using UnityEngine;

namespace CodeBase.Services.Factory {
	public interface IGameFactory : IService {
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }
		GameObject CreateHero(GameObject at);
		void CreateHud();
		void Cleanup();
	}
}