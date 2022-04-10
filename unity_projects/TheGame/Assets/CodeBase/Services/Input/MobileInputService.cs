using UnityEngine;

namespace CodeBase.Services.Input {
	public class MobileInputService : SimpleInputService {
		public override Vector2 Axis => GetSimpleInputAxis();
	}
}