using System.Reflection;
using CodeBase.Infrastructure.States;
using UnityEditor;

namespace CodeBase.Infrastructure.Editor {
	[CustomEditor(typeof(GameBootstrapper))]
	public class GameBootstrapperEditor : UnityEditor.Editor {
		private GameBootstrapper Script => target as GameBootstrapper;
		private GameStateMachine _stateMachine;

		private void OnEnable() {
			_stateMachine = (Script.GetType().GetField("_game", BindingFlags.NonPublic | BindingFlags.Instance)
				?.GetValue(Script) as Game)?.StateMachine;
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			var stateName = _stateMachine?.GetType()
				.GetField("_activeState", BindingFlags.NonPublic | BindingFlags.Instance)
				?.GetValue(_stateMachine)?.GetType().Name ?? "Null";

			EditorGUILayout.LabelField("StateMachine", stateName, EditorStyles.label);

			serializedObject.ApplyModifiedProperties();
		}
	}
}