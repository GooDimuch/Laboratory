using System;
using System.Linq;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor {
	[CustomEditor(typeof(LevelStaticData))]
	public class LevelStaticDataEditor : UnityEditor.Editor {
		private const string INITIAL_POINT_TAG = "InitialPoint";

		private LevelStaticData _levelData;

		private void OnEnable() {
			try {
				_levelData = (LevelStaticData)target ?? throw new NullReferenceException($"script is null");
			}
			catch (Exception e) {
				Debug.LogError(e);
			}
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			PressCollect(_levelData);
		}

		private static void PressCollect(LevelStaticData levelData) {
			if (!GUILayout.Button("Collect")) return;
			levelData.EnemySpawners = FindObjectsOfType<SpawnMarker>()
				.Select(x => new EnemySpawnerStaticData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position))
				.ToList();

			levelData.InitialHeroTransform = GameObject.FindWithTag(INITIAL_POINT_TAG).transform.AsTransformData();
			levelData.LevelKey = SceneManager.GetActiveScene().name;

			EditorUtility.SetDirty(levelData);
		}
	}
}