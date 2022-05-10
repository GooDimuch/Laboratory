using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI {
	public class ActorUI : MonoBehaviour {
		public HpBar HpBar;

		private IHealth _health;

		private void Start() {
			if (_health == null)
				Construct(GetComponent<IHealth>());
		}

		private void OnDestroy() {
			if (_health != null)
				_health.HealthChanged -= UpdateHpBar;
		}

		public void Construct(IHealth health) {
			_health = health;
			_health.HealthChanged += UpdateHpBar;
		}

		private void UpdateHpBar() => HpBar.SetValue(_health.Current, _health.Max);
	}
}