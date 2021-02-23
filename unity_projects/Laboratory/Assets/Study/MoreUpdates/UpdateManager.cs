using UnityEngine;

public class UpdateManager : MonoBehaviour {
	private void OnEnable() { }

	private void OnDisable() { }

	private void Update() {
		for (var i = 0; i < MonoCached.allTicks.Count; i++) { MonoCached.allTicks[i].Tick(); }
	}

	private void FixedUpdate() {
		for (var i = 0; i < MonoCached.allFixedTicks.Count; i++) { MonoCached.allFixedTicks[i].FixedTick(); }
	}

	private void LateUpdate() {
		for (var i = 0; i < MonoCached.allLateTicks.Count; i++) { MonoCached.allLateTicks[i].LateTick(); }
	}
}