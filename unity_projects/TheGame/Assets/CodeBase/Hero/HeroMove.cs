using CodeBase.Data;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistantProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero {
	[RequireComponent(typeof(CharacterController))]
	public class HeroMove : MonoBehaviour, ISavedProgress {
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private float _movementSpeed;

		private IInputService _inputService;
		private Camera _camera;

		private void Awake() {
			_inputService = AllServices.Container.Single<IInputService>();
		}

		private void Start() =>
			_camera = Camera.main;

		private void Update() {
			var movementVector = Vector3.zero;

			if (_inputService.Axis.sqrMagnitude > Constants.EPSILON) {
				movementVector = _camera.transform.TransformDirection(_inputService.Axis);
				movementVector.y = 0;
				movementVector.Normalize();
				movementVector *= _movementSpeed;

				transform.forward = movementVector;
			}

			movementVector += Physics.gravity;

			_characterController.Move(movementVector * Time.deltaTime);
		}

		public void UpdateProgress(PlayerProgress progress) {
			progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevelName(), transform.position.AsVectorData());
		}

		public void LoadProgress(PlayerProgress progress) {
			if (CurrentLevelName() != progress.WorldData.PositionOnLevel.Level) return;

			if (progress.WorldData.PositionOnLevel.Position != null)
				Warp(progress.WorldData.PositionOnLevel.Position.AsUnityVector());
		}

		private void Warp(Vector3 to) {
			_characterController.enabled = false;
			transform.position = to + Vector3.up * _characterController.height;
			_characterController.enabled = true;
		}

		private static string CurrentLevelName() => SceneManager.GetActiveScene().name;
	}
}