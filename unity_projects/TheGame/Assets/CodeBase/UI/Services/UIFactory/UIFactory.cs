using CodeBase.Services.Ads;
using CodeBase.Services.AssetManagement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.WindowService;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Shop;
using UnityEngine;

namespace CodeBase.UI.Services.UIFactory {
	public class UIFactory : IUIFactory {
		private readonly IAssetProvider _assetsProvider;
		private readonly IStaticDataService _staticData;
		private readonly IPersistentProgressService _progressService;
		private readonly IAdsService _adsService;

		private Transform _uiRoot;

		public UIFactory(IAssetProvider assetsProvider, IStaticDataService staticData, IPersistentProgressService progressService,
			IAdsService adsService) {
			_assetsProvider = assetsProvider;
			_staticData = staticData;
			_progressService = progressService;
			_adsService = adsService;
		}

		public void CreateShop() {
			var config = _staticData.ForWindow(WindowId.Shop);
			var window = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
			window.Construct(_progressService, _adsService);
		}

		public void CreateUIRoot() =>
			_uiRoot = _assetsProvider.Instantiate(AssetAddress.UI_ROOT_PATH).transform;
	}
}