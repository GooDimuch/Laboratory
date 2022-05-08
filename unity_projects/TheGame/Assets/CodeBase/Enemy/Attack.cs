using System.Linq;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Factory;
using CodeBase.Utils;
using UnityEngine;

namespace CodeBase.Enemy {
	[RequireComponent(typeof(EnemyAnimator))]
	public class Attack : MonoBehaviour {
		public EnemyAnimator Animator;

		public float AttackCooldown = 3.0f;
		public float Cleavage = 0.5f;
		public float EffectiveDistance = 0.5f;
		public float Damage = 10;

		private IGameFactory _factory;
		private Transform _heroTransform;
		private float _attackCooldown;
		private bool _isAttacking;
		private readonly Collider[] _hits = new Collider[1];
		private int _layerMask;
		private bool _attackIsActive;

		private void Awake() {
			_factory = AllServices.Container.Single<IGameFactory>();

			_layerMask = 1 << LayerMask.NameToLayer("Player");

			_factory.HeroCreated += OnHeroCreated;
		}

		private void Update() {
			UpdateCooldown();

			if (CanAttack())
				StartAttack();
		}

		public void EnableAttack() => _attackIsActive = true;

		public void DisableAttack() => _attackIsActive = false;

		private void OnAttack() {
			if (Hit(out var hit)) {
				PhysicsDebug.DrawDebug(WeaponPosition(), Cleavage, 1.0f);
				var component = hit.attachedRigidbody ? hit.attachedRigidbody.transform : hit.transform;
				component.GetComponent<IHealth>().TakeDamage(Damage);
			}
		}

		private void OnAttackEnded() {
			_attackCooldown = AttackCooldown;
			_isAttacking = false;
		}

		private void OnHeroCreated() => _heroTransform = _factory.HeroGameObject.transform;

		private bool CooldownIsUp() => _attackCooldown <= 0f;

		private void UpdateCooldown() {
			if (!CooldownIsUp())
				_attackCooldown -= Time.deltaTime;
		}

		private bool Hit(out Collider hit) {
			var hitAmount = Physics.OverlapSphereNonAlloc(WeaponPosition(), Cleavage, _hits, _layerMask);

			hit = _hits.FirstOrDefault();

			return hitAmount > 0;
		}

		private Vector3 WeaponPosition() => transform.position + Vector3.up * 0.5f + transform.forward * EffectiveDistance;

		private bool CanAttack() => _attackIsActive && !_isAttacking && CooldownIsUp();

		private void StartAttack() {
			transform.LookAt(_heroTransform);
			Animator.PlayAttack();
			_isAttacking = true;
		}
	}
}