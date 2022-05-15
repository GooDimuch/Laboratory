using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArrowController), true)]
public class ArrowControllerEditor : Editor {
	private ArrowController script;

	void OnEnable() { script = (ArrowController) target; }

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		InspectorGUI();
	}

	private void InspectorGUI() {
		serializedObject.Update();

		CheckMultiMode();

		foreach (var constructor in script.constructors) {
			if (constructor.create) {
				if (constructor.minValue > constructor.maxValue ||
						constructor.StartAngle > constructor.EndAngle ||
						Math.Abs(constructor.minValue - constructor.maxValue) < float.Epsilon &&
						Math.Abs(constructor.StartAngle - constructor.EndAngle) < float.Epsilon) {
					if (constructor.correct) { constructor.correct = false; }
					Debug.LogError("Incorrect ");
				} else {
					constructor.correct = true;
					if (constructor.curve.keys.Length < 2) { CreateBaseKeys(constructor); }
					DrawCurveInEditor(constructor);
				}
			}
		}
		serializedObject.ApplyModifiedProperties();
	}

	private void CheckMultiMode() {
		var exist = script.gameObject.TryGetComponent(out IntState state);
		if (script.constructors.Count > 1 && !exist) {
			state = script.gameObject.AddComponent<IntState>();
			state.simObjectName = script.savedSimObjectNameForIntState;
			state.systemName = script.savedSystemNameForIntState;
		} else if (script.constructors.Count < 2 && exist) {
			script.savedSimObjectNameForIntState = state.simObjectName;
			script.savedSystemNameForIntState = state.systemName;
			DestroyImmediate(state);
		}
	}

	private void CreateBaseKeys(ArrowController.CurveConstructor constructor) {
		constructor.curve = AnimationCurve.Linear(constructor.minValue,
				constructor.StartAngle,
				constructor.maxValue,
				constructor.EndAngle);
	}

	private void DrawCurveInEditor(ArrowController.CurveConstructor constructor) {
		EditorGUILayout.LabelField($"Curve {script.constructors.IndexOf(constructor)}");
		EditorGUILayout.CurveField(constructor.curve,
				Color.green,
				new Rect(constructor.minValue,
						constructor.StartAngle,
						constructor.maxValue - constructor.minValue,
						constructor.EndAngle - constructor.StartAngle));
	}
}