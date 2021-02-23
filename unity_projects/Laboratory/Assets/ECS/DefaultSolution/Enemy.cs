using UnityEngine;

public class Enemy : MonoBehaviour {
	private Transform player;
	private float speed;

	private void Start() { }

	private void Update() {
		var direction = player.position - transform.position;
		transform.position += direction.normalized * Time.deltaTime * speed;
		transform.localRotation = Quaternion.Euler(Vector3.left * 90);
	}

	public void AddTarget(Transform player) { this.player = player; }
	public void SetSpeed(float speed) { this.speed = speed; }
}