using System;
using UnityEngine;

namespace CodeBase.Data.Triggers {
	[Serializable]
	public class TriggerData {
		public TransformData transform;
		public Vector3Data size;

		public TriggerData(TransformData transform, Vector3 size) {
			this.transform = transform;
			this.size = size.AsVectorData();
		}
	}
}