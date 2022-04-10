using UnityEngine;

namespace CodeBase.Services.Input {
	public abstract class SimpleInputService : InputService {
		protected static Vector2 GetSimpleInputAxis() =>
			new Vector2(SimpleInput.GetAxis(HORIZONTAL), SimpleInput.GetAxis(VERTICAL));

		public override bool IsAttackButtonDown() => SimpleInput.GetButtonDown(BUTTON);
	}
}