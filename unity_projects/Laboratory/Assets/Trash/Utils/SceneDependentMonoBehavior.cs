using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDependentMonoBehavior : MonoBehaviour {
	public List<string> sceneNames = new List<string>{"Simulations"};
	protected bool IsAllSceneLoaded { get; private set; }
	private int _loadedSceneCounter;

	protected void Start() {
    foreach (var name in sceneNames.Where(IsSceneLoaded)) {
			InitializeOnSceneLoading(name);
			_loadedSceneCounter++;
		}
		if (_loadedSceneCounter.Equals(sceneNames.Count)) {
			IsAllSceneLoaded = true;
			AllSceneLoaded();
		}
    SceneManager.sceneLoaded += OnSceneLoaded;
    SceneManager.sceneUnloaded += OnSceneUnloaded;
  }

	private void OnEnable() {
		
	}

	private void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}

	protected void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (!HasScene(scene.name)) return;
		try {
			InitializeOnSceneLoading(scene.name);
			_loadedSceneCounter++;
			if (_loadedSceneCounter.Equals(sceneNames.Count)) { AllSceneLoaded(); }
		} catch (Exception e) {
			Debug.LogError("gameObject = " + name + "  " + scene.name + " " + mode + " error detail :" + e);
		}
	}

	protected void OnSceneUnloaded(Scene scene) {
		if (HasScene(scene.name)) { ClearOnSceneUnloading(scene.name); }
	}

	private bool HasScene(string sceneName) { return sceneNames != null && sceneNames.Any(sceneName.Contains); }

	private bool IsSceneLoaded(string sceneName) {
		if (gameObject.scene.name.Contains(sceneName)) { return true; }
		for (var i = 0; i < SceneManager.sceneCount; i++) {
			if (SceneManager.GetSceneAt(i).isLoaded && SceneManager.GetSceneAt(i).name.Contains(sceneName)) { return true; }
		}
		return false;
	}

	protected virtual void AllSceneLoaded() { }
	protected virtual void InitializeOnSceneLoading(string sceneName) { }
	protected virtual void ClearOnSceneUnloading(string sceneName) { }
}