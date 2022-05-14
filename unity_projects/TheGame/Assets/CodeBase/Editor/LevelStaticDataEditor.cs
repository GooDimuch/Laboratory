using System;
using System.Linq;
using CodeBase.Data;
using CodeBase.Data.Triggers;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.Triggers;
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

			levelData.LevelKey = SceneManager.GetActiveScene().name;
			levelData.InitialHeroTransform = GameObject.FindWithTag(INITIAL_POINT_TAG).transform.AsTransformData();
			levelData.NextLevelTriggerTransform = GetLevelTransferTriggerData(FindObjectOfType<LevelTransferTriggerMarker>());
			levelData.SaveTriggerTransforms = FindObjectsOfType<SaveTriggerMarker>().Select(GetSaveTriggerData).ToList();

			EditorUtility.SetDirty(levelData);
		}

		private static TriggerData GetSaveTriggerData(SaveTriggerMarker triggerMarker) =>
			new TriggerData(triggerMarker.transform.AsTransformData(), triggerMarker.size);

		private static LevelTransferTriggerData GetLevelTransferTriggerData(LevelTransferTriggerMarker triggerMarker) =>
			new LevelTransferTriggerData(triggerMarker.transform.AsTransformData(), triggerMarker.size,
				triggerMarker.TransferTo);
	}
}