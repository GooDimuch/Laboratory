using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy {
	[RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
	public class EnemyDeath : MonoBehaviour {
		public EnemyHealth Health;
		
		public Follow Follow;
		public CheckAttackRange CheckAttackRange;
		public Attack Attack;
		public EnemyAnimator Animator;

		public GameObject DeathFx;

		public event Action Happened;

		private void Start() => 
			Health.HealthChanged += OnHealthChanged;

		private void OnDestroy() => 
			Health.HealthChanged -= OnHealthChanged;

		private void OnHealthChanged() {
			if (Health.Current <= 0)
				Die();
		}

		private void Die() {
			Health.HealthChanged -= OnHealthChanged;
			Follow.enabled = false;
			CheckAttackRange.enabled = false;
			Attack.DisableAttack();

			Animator.PlayDeath();
			SpawnDeathFx();

			StartCoroutine(DestroyTimer());

			Happened?.Invoke();
		}

		private void SpawnDeathFx() => 
			Instantiate(DeathFx, transform.position, Quaternion.identity);

		private IEnumerator DestroyTimer() {
			yield return new WaitForSeconds(3);
			Destroy(gameObject);
		}
	}
}