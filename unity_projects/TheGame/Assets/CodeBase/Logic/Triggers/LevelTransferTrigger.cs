using CodeBase.Services.GameStateMachine;
using CodeBase.Services.GameStateMachine.States;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Logic.Triggers {
	[RequireComponent(typeof(BoxCollider))]
	public class LevelTransferTrigger : MonoBehaviour {
		private const string PLAYER_TAG = "Player";

		[SerializeField, ReadOnly] private LoadLevelState.Level transferTo;

		private IGameStateMachine _stateMachine;
		private bool _triggered;

		public void Construct(IGameStateMachine stateMachine, LoadLevelState.Level level) {
			_stateMachine = stateMachine;
			transferTo = level;
		}

		private void OnTriggerEnter(Collider other) {
			if (_triggered || !other.CompareTag(PLAYER_TAG))
				return;

			_stateMachine.Enter<LoadLevelState, LoadLevelState.Level>(transferTo);
			_triggered = true;
		}
	}
}