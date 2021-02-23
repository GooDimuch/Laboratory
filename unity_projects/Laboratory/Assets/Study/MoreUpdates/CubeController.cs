using UnityEngine;

public class CubeController : MonoBehaviour {
	public GameObject prefab;
	public int amount = 1000;

	private float STEP = 1.1f;

	private void Start() {
		// for (var i = 0; i < Mathf.Sqrt(amount); i++) {
		// 	for (var j = 0; j < Mathf.Sqrt(amount); j++) {
		// 		Instantiate(prefab, new Vector3(j * STEP, i * STEP), prefab.transform.localRotation);
		// 	}
		// }
		for (var i = 0; i < amount; i++) { Instantiate(prefab); }
	}

	private void Update() { }
}