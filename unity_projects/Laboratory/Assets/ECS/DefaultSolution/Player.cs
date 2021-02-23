using ECS.CustomClone;
using UnityEngine;

public class Player : MonoBehaviour {
	public PlayerInitData playerPrefab;

	public GameObject player { get; private set; }

	private void Start() { player = Instantiate(playerPrefab.playerPrefab, Vector3.zero, Quaternion.identity); }

	private void Update() {
		var direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		player.transform.position += direction.normalized * Time.deltaTime * playerPrefab.defaultSpeed;
		player.transform.localRotation = Quaternion.Euler(Vector3.left * 90);
	}
}