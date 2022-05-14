using CodeBase.Services.SaveLoadService;
using UnityEngine;

namespace CodeBase.Logic.Triggers {
	[RequireComponent(typeof(BoxCollider))]
	public class SaveTrigger : MonoBehaviour {
		private ISaveLoadService _saveLoadService;

		public void Construct(ISaveLoadService saveLoadService) =>
			_saveLoadService = saveLoadService;

		private void OnTriggerEnter(Collider other) {
			_saveLoadService.SaveProgress();
			gameObject.SetActive(false);
		}
	}
}