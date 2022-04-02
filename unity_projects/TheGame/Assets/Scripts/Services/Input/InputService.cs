using UnityEngine;

namespace Services.Input {
	public abstract class InputService : IInputService {
		protected const string HORIZONTAL = "Horizontal";
		protected const string VERTICAL = "Vertical";
		protected const string BUTTON = "Fire";

		public abstract Vector2 Axis { get; }

		public abstract bool IsAttackButtonDown();
	}
}