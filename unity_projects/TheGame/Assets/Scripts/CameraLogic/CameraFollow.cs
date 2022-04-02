using UnityEngine;

namespace CameraLogic {
	[ExecuteInEditMode]
	public class CameraFollow : MonoBehaviour {
		[SerializeField] private float _rotationAngleX;
		[SerializeField] private float _distance;
		[SerializeField] private float _offsetY;

		private Transform _following;

		private void LateUpdate() {
			if (_following == null) {
#if UNITY_EDITOR
				_following = GameObject.FindWithTag("InitialPoint")?.transform;
#endif
				return;
			}

			var newRotation = Quaternion.Euler(_rotationAngleX, 
				transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
			var newPosition = newRotation * (Vector3.forward * -_distance) + FollowingPointPosition();

			transform.rotation = newRotation;
			transform.position = newPosition;
		}

		public void Follow(GameObject following) {
			_following = following.transform;
		}

		private Vector3 FollowingPointPosition() => _following.position + Vector3.up * _offsetY;
	}
}