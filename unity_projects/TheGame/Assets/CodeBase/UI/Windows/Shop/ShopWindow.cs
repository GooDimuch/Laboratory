using CodeBase.Data;
using CodeBase.Services.Ads;
using CodeBase.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows.Shop {
	public class ShopWindow : BaseWindow {
		public TMP_Text MoneyValueText;
		public RewardedAdItem AdItem;

		private IPersistentProgressService _progressService;
		private PlayerProgress Progress => _progressService.Progress;

		public new void Construct(IPersistentProgressService progressService, IAdsService adsService) {
			base.Construct();
			_progressService = progressService;
			AdItem.Construct(progressService, adsService);
		}

		protected override void Initialize() {
			AdItem.Initialize();
			RefreshMoneyValueText();
		}

		protected override void Subscribe() {
			AdItem.Subscribe();
			Progress.WorldData.LootData.Changed += RefreshMoneyValueText;
		}

		protected override void Cleanup() {
			base.Cleanup();
			AdItem.Cleanup();
			Progress.WorldData.LootData.Changed -= RefreshMoneyValueText;
		}

		private void RefreshMoneyValueText() =>
			MoneyValueText.text = Progress.WorldData.LootData.Collected.ToString();
	}
}