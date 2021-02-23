using UnityEngine;

public class ScaleCubeExtended : MonoCached {
	protected override void OnTick() {
		var val = Mathf.Sin(Time.time);
		transform.localScale = Vector3.one * val;
	}
}