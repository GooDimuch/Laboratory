using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows {
	public class ShopWindow : BaseWindow {
		public TMP_Text MoneyValueText;

		private IPersistantProgressService _progressService;
		protected PlayerProgress Progress => _progressService.Progress;

		public new void Construct(IPersistantProgressService progressService) {
			base.Construct();
			_progressService = progressService;
		}

		protected override void Initialize() =>
			RefreshMoneyValueText();

		protected override void Subscribe() =>
			Progress.WorldData.LootData.Changed += RefreshMoneyValueText;

		protected override void Cleanup() {
			base.Cleanup();
			Progress.WorldData.LootData.Changed -= RefreshMoneyValueText;
		}

		private void RefreshMoneyValueText() =>
			MoneyValueText.text = Progress.WorldData.LootData.Collected.ToString();
	}
}