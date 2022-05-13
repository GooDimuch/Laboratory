using CodeBase.Services.Ads;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop {
	public class RewardedAdItem : MonoBehaviour {
		public Button ShowAdButton;
		public GameObject[] AdActiveObjects;
		public GameObject[] AdInactiveObjects;

		private IPersistentProgressService _progressService;
		private IAdsService _adsService;

		public void Construct(IPersistentProgressService progressService, IAdsService adsService) {
			_progressService = progressService;
			_adsService = adsService;
		}

		public void Initialize() {
			ShowAdButton.onClick.AddListener(OnShowAdClicked);
			RefreshAvailableAd();
		}

		public void Subscribe() =>
			_adsService.RewardedVideoReady += RefreshAvailableAd;

		public void Cleanup() =>
			_adsService.RewardedVideoReady -= RefreshAvailableAd;

		private void OnShowAdClicked() =>
			_adsService.ShowRewardedVideo(OnVideoFinished);

		private void OnVideoFinished() =>
			_progressService.Progress.WorldData.LootData.Collect(_adsService.Reward);

		private void RefreshAvailableAd() {
			var isVideoReady = _adsService.IsRewardedVideoReady;

			foreach (var adActiveObject in AdActiveObjects)
				adActiveObject.SetActive(isVideoReady);

			foreach (var adInactiveObject in AdInactiveObjects)
				adInactiveObject.SetActive(!isVideoReady);
		}
	}
}