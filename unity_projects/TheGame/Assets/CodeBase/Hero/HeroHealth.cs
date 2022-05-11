using System;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Hero {
	[RequireComponent(typeof(HeroAnimator))]
	public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth {
		public HeroAnimator Animator;

		[SerializeField, ReadOnly] private State _state;

		public event Action HealthChanged;

		public float Current {
			get => _state.currentHp;
			set {
				if (!(Math.Abs(value - _state.currentHp) > float.Epsilon)) return;
				_state.currentHp = value;
				HealthChanged?.Invoke();
			}
		}

		public float Max {
			get => _state.maxHp;
			set => _state.maxHp = value;
		}


		public void TakeDamage(float damage) {
			if (Current <= 0)
				return;

			Current -= damage;
			Animator.PlayHit();
		}

		public void LoadProgress(PlayerProgress progress) {
			_state = progress.HeroState;
			HealthChanged?.Invoke();
		}

		public void UpdateProgress(PlayerProgress progress) {
			progress.HeroState.currentHp = Current;
			progress.HeroState.maxHp = Max;
		}
	}
}