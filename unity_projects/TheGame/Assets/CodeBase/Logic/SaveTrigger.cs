using CodeBase.Services;
using CodeBase.Services.SaveLoadService;
using UnityEngine;

namespace CodeBase.Logic {
	[RequireComponent(typeof(Collider))]
	public class SaveTrigger : MonoBehaviour {
		private ISaveLoadService _saveLoadService;
		private BoxCollider _collider;

		private BoxCollider Collider {
			get => _collider ? _collider : _collider = GetComponent<BoxCollider>();
			set => _collider = value;
		}

		private void Awake() {
			_saveLoadService = AllServices.Container.Single<ISaveLoadService>();
			Collider = GetComponent<BoxCollider>();
		}

		private void OnTriggerEnter(Collider other) {
			_saveLoadService.SaveProgress();
			gameObject.SetActive(false);
		}

		private void OnDrawGizmos() {
			Gizmos.color = new Color(0, 1, 0, 0.3f);
			Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
		}
	}
}