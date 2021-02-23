using UnityEngine;

public class ScaleCube : MonoBehaviour {
	private void Update() {
		var val = Mathf.Sin(Time.time);
		transform.localScale = Vector3.one * val;
	}
}