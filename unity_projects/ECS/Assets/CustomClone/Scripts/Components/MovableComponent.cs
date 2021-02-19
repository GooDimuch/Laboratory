using System.Text;
using UnityEngine;

namespace ECS.CustomClone.Component {
	public struct MovableComponent {
		public Transform transform;
		public float moveSpeed;
		public bool isMoving;

		public override string ToString() {
			var sb = new StringBuilder();
			sb.AppendLine($"transform {transform}");
			sb.AppendLine($"moveSpeed {moveSpeed}");
			sb.AppendLine($"isMoving {isMoving}");
			return sb.ToString();
		}
	}
}