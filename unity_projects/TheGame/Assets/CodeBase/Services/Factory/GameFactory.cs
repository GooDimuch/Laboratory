using System;
using System.Collections.Generic;
using CodeBase.Hero;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.PersistantProgress;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Services.Factory {
	public class GameFactory : IGameFactory {
		private readonly IAsset _asset;

		public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
		public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
		public GameObject HeroGameObject { get; private set; }
		public event Action HeroCreated;

		public GameFactory(IAsset asset) {
			_asset = asset;
		}

		public GameObject CreateHero(GameObject at) {
			HeroGameObject = InstantiateRegistered(AssetPath.HERO_PATH, at.transform.position, at.transform.rotation);
			HeroCreated?.Invoke();
			return HeroGameObject;
		}

		public void CreateHud(GameObject hero) {
			var hud = InstantiateRegistered(AssetPath.HUD_PATH);
			hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
		}

		private GameObject InstantiateRegistered(string prefabPath) =>
			InstantiateRegistered(prefabPath, Vector3.zero, Quaternion.identity);

		private GameObject InstantiateRegistered(string prefabPath, Vector3 at, Quaternion with) {
			var gameObject = _asset.Instantiate(path: prefabPath, at: at, with: with);
			RegisterProgressWatchers(gameObject);
			return gameObject;
		}

		public void Cleanup() {
			ProgressReaders.Clear();
			ProgressWriters.Clear();
		}

		private void RegisterProgressWatchers(GameObject gameObject) {
			foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				Register(progressReader);
		}

		private void Register(ISavedProgressReader progressReader) {
			if (progressReader is ISavedProgress progressWriter)
				ProgressWriters.Add(progressWriter);
			ProgressReaders.Add(progressReader);
		}
	}
}