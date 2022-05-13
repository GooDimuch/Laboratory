using CodeBase.Services.AssetManagement;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.UI.Services.WindowService;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.UIFactory {
	public class UIFactory : IUIFactory {
		private readonly IAsset _assets;
		private readonly IStaticDataService _staticData;
		private readonly IPersistantProgressService _progressService;

		private Transform _uiRoot;

		public UIFactory(IAsset assets, IStaticDataService staticData, IPersistantProgressService progressService) {
			_assets = assets;
			_staticData = staticData;
			_progressService = progressService;
		}

		public void CreateShop() {
			var config = _staticData.ForWindow(WindowId.Shop);
			var window = (ShopWindow) Object.Instantiate(config.Prefab, _uiRoot);
			window.Construct(_progressService);
		}

		public void CreateUIRoot() =>
			_uiRoot = _assets.Instantiate(AssetPath.UI_ROOT_PATH).transform;
	}
}