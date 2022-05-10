using UnityEngine;

namespace CodeBase.Services.Input {
	public class StandaloneInputService : SimpleInputService {
		public override Vector2 Axis {
			get {
				var axis = GetSimpleInputAxis();

				if (axis == Vector2.zero) {
					axis = GetUnityAxis();
				}

				return axis;
			}
		}

		private static Vector2 GetUnityAxis() =>
			new Vector2(UnityEngine.Input.GetAxis(HORIZONTAL), UnityEngine.Input.GetAxis(VERTICAL));

		public override bool IsAttackButtonDown() =>
			base.IsAttackButtonDown() ||
			UnityEngine.Input.GetMouseButtonDown(0);
	}
}