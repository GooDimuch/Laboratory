using UnityEngine;

namespace CodeBase.Infrastructure {
	public class GameRunner : MonoBehaviour {
		public GameBootstrapper gameBootstrapperPrefab;

		private void Awake() {
			var bootstrapper = FindObjectOfType<GameBootstrapper>();
			if (!bootstrapper)
				Instantiate(gameBootstrapperPrefab);
		}
	}
}