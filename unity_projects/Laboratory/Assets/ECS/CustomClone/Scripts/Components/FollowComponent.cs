using System.Text;
using UnityEngine;

namespace ECS.CustomClone.Component {
	public struct FollowComponent {
		public Transform target;

		public override string ToString() {
			var sb = new StringBuilder();
			sb.AppendLine($"transform {target}");
			return sb.ToString();
		}
	}
}