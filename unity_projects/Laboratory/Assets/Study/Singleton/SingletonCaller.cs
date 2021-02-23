using UnityEngine;

public class SingletonCaller : MonoBehaviour {
	private void Awake() { Debug.Log(SingletonController.Instance.Name); }

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) { Debug.Log(SingletonController.Instance.Name); }
	}
}