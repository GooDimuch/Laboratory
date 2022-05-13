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
		private readonly IAsset _assets;
		private readonly IStaticDataService _staticData;
		private readonly IPersistentProgressService _progressService;
		private readonly IAdsService _adsService;

		private Transform _uiRoot;

		public UIFactory(IAsset assets, IStaticDataService staticData, IPersistentProgressService progressService,
			IAdsService adsService) {
			_assets = assets;
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
			_uiRoot = _assets.Instantiate(AssetPath.UI_ROOT_PATH).transform;
	}
}