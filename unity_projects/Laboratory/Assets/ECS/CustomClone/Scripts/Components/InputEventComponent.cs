using System.Text;
using UnityEngine;

namespace ECS.CustomClone.Component {
	public struct InputEventComponent {
		public Vector2 direction;

		public override string ToString() {
			var sb = new StringBuilder();
			sb.AppendLine($"direction {direction}");
			return sb.ToString();
		}
	}
}