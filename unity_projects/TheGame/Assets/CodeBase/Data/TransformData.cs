using System;
using UnityEngine;

namespace CodeBase.Data {
	[Serializable]
	public class TransformData {
		public Vector3 position;
		[SerializeField] private Vector3 _rotation;
		public Vector3 scale;

		public Quaternion rotation => Quaternion.Euler(_rotation);

		public TransformData(Transform transform) {
			position = transform.position;
			_rotation = transform.rotation.eulerAngles;
			scale = transform.localScale;
		}
	}
}