using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows {
	public abstract class BaseWindow : MonoBehaviour {
		public Button CloseButton;

		protected void Construct() { }

		private void Awake() =>
			OnAwake();

		private void Start() {
			Initialize();
			Subscribe();
		}

		private void OnDestroy() =>
			Cleanup();

		protected virtual void OnAwake() =>
			CloseButton.onClick.AddListener(() => Destroy(gameObject));

		protected virtual void Initialize() { }
		protected virtual void Subscribe() { }
		protected virtual void Cleanup() { }
	}
}