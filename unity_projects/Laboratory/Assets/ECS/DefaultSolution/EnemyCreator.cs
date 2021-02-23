using System.Collections.Generic;
using ECS.CustomClone;
using UnityEngine;

public class EnemyCreator : MonoBehaviour {
	public Player player;
	public EnemyInitData enemyPrefab;
	private readonly List<GameObject> enemyList = new List<GameObject>();

	[Range(0, 1000000)] public int _enemyOfNumber = 1;
	public int EnemyOfNumber => _enemyOfNumber;

	private void Start() { }

	private void Update() {
		Sync();

		if (Input.GetKeyDown(KeyCode.Equals)) { _enemyOfNumber += 10; }
		if (Input.GetKeyDown(KeyCode.Minus)) { _enemyOfNumber -= 10; }

		_enemyOfNumber = Mathf.Clamp(_enemyOfNumber, 0, _enemyOfNumber);
	}

	private void Sync() {
		var diff = EnemyOfNumber - enemyList.Count;
		if (diff > 0) {
			for (var i = 0; i < Mathf.Abs(diff); i++) { CreateEnemy(GetRandomPosition(), player.player.transform); }
		}
		else if (diff < 0) {
			for (var i = 0; i < Mathf.Abs(diff); i++) { RemoveEnemy(); }
		}
	}

	private static Vector3 GetRandomPosition() { return Vector2.one * Random.Range(-2f, 2f); }

	private void CreateEnemy(Vector3 atPosition, Transform target) {
		var enemy = Instantiate(enemyPrefab.enemyPrefab, atPosition, Quaternion.identity);
		var component = enemy.AddComponent<Enemy>();
		component.AddTarget(target);
		component.SetSpeed(enemyPrefab.defaultSpeed);
		enemyList.Add(enemy);
	}

	private void RemoveEnemy() {
		if (enemyList.Count == 0) { return; }
		Destroy(enemyList[0]);
		enemyList.RemoveAt(0);
	}
}