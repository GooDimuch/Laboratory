using System;
using UnityEngine;

namespace CodeBase.Data {
	[Serializable]
	public class TransformData {
		public Vector3Data position;
		[SerializeField] private Vector3Data _rotation;
		public Vector3Data scale;

		public Quaternion rotation => Quaternion.Euler(_rotation.AsUnityVector());

		public TransformData(Transform transform) {
			position = transform.position.AsVectorData();
			_rotation = transform.rotation.eulerAngles.AsVectorData();
			scale = transform.localScale.AsVectorData();
		}
	}
}