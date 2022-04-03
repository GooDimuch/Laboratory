using Infrastructure;
using Services.Input;
using UnityEngine;

namespace Hero {
	[RequireComponent(typeof(CharacterController))]
	public class RaptorMove : MonoBehaviour {
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private float _movementSpeed;

		private IInputService _inputService;
		private Camera _camera;

		private void Awake() {
			_inputService = Game.InputService;
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
	}
}