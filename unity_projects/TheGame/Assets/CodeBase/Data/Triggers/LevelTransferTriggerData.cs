using System;
using UnityEngine;

namespace CodeBase.Data.Triggers {
	[Serializable]
	public class LevelTransferTriggerData : TriggerData {
		public string TransferTo;

		public LevelTransferTriggerData(TransformData transform, Vector3 size, string transferTo) : base(transform, size) {
			TransferTo = transferTo;
		}
	}
}