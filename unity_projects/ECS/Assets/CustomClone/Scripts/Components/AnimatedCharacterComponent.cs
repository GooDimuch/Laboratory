using System.Text;
using UnityEngine;

namespace ECS.CustomClone.Component {
	public struct AnimatedCharacterComponent {
		public string moveAnimationName;
		public Animator animator;

		public override string ToString() {
			var sb = new StringBuilder();
			sb.AppendLine($"moveAnimationName {moveAnimationName}");
			sb.AppendLine($"animator {animator}");
			return sb.ToString();
		}
	}
}