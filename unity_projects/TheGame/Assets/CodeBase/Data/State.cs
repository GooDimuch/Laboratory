using System;

namespace CodeBase.Data {
	[Serializable]
	public class State {
		public float maxHp;
		public float currentHp;

		public void ResetHp() => currentHp = maxHp;
	}
}