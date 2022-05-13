using System;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads {
	public interface IAdsService : IService {
		event Action RewardedVideoReady;
		bool IsRewardedVideoReady { get; }
		int Reward { get; }
		void Initialize();
		void ShowRewardedVideo(Action onVideoFinished);
	}

	public class AdsService : IAdsService, IUnityAdsListener {
		private const string ANDROID_GAME_ID = "4753232";
		private const string IOS_GAME_ID = "4753233";

		private const string ANDROID_REWARDED_VIDEO_PLACEMENT_ID = "Rewarded_Android";
		private const string IOS_REWARDED_VIDEO_PLACEMENT_ID = "Rewarded_iOS";


		public event Action RewardedVideoReady;
		public bool IsRewardedVideoReady => Advertisement.IsReady(_placementId);
		public int Reward => 15;

		private string _gameId;
		private string _placementId;

		private Action _onVideoFinished;

		public void Initialize() {
			SetIdsForCurrentPlatform();
			Advertisement.AddListener(this);
			Advertisement.Initialize(_gameId);
		}

		public void ShowRewardedVideo(Action onVideoFinished) {
			_onVideoFinished = onVideoFinished;
			Advertisement.Show(_placementId);
		}

		public void OnUnityAdsReady(string placementId) {
			Debug.Log($"OnUnityAdsReady {placementId}");

			if (placementId == _placementId)
				RewardedVideoReady?.Invoke();
		}

		public void OnUnityAdsDidError(string message) =>
			Debug.LogError($"OnUnityAdsDidError {message}");

		public void OnUnityAdsDidStart(string placementId) =>
			Debug.Log($"OnUnityAdsDidStart {placementId}");

		public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
			switch (showResult) {
				case ShowResult.Failed:
					Debug.LogError($"OnUnityAdsDidFinish {showResult}");
					break;
				case ShowResult.Skipped:
					Debug.LogError($"OnUnityAdsDidFinish {showResult}");
					break;
				case ShowResult.Finished:
					_onVideoFinished?.Invoke();
					break;
				default:
					Debug.LogError($"OnUnityAdsDidFinish {showResult}");
					break;
			}

			_onVideoFinished = null;
		}

		private void SetIdsForCurrentPlatform() {
			switch (Application.platform) {
				case RuntimePlatform.Android:
					_gameId = ANDROID_GAME_ID;
					_placementId = ANDROID_REWARDED_VIDEO_PLACEMENT_ID;
					break;
				case RuntimePlatform.IPhonePlayer:
					_gameId = IOS_GAME_ID;
					_placementId = IOS_REWARDED_VIDEO_PLACEMENT_ID;
					break;
				case RuntimePlatform.WindowsEditor:
					_gameId = ANDROID_GAME_ID;
					_placementId = ANDROID_REWARDED_VIDEO_PLACEMENT_ID;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(Application.platform), "Unsupported platform for ADS");
			}
		}
	}
}