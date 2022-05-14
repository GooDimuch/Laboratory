using System.Threading.Tasks;
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
		private readonly IAssetProvider _assetProvider;
		private readonly IStaticDataService _staticData;
		private readonly IPersistentProgressService _progressService;
		private readonly IAdsService _adsService;

		private Transform _uiRoot;

		public UIFactory(IAssetProvider assetProvider, IStaticDataService staticData,
			IPersistentProgressService progressService,
			IAdsService adsService) {
			_assetProvider = assetProvider;
			_staticData = staticData;
			_progressService = progressService;
			_adsService = adsService;
		}

		public async Task WarmUp() { }

		public async Task CreateShop() {
			var config = _staticData.ForWindow(WindowId.Shop);
			var prefab = await _assetProvider.Load<GameObject>(config.PrefabReferance);
			var window = Object.Instantiate(prefab, _uiRoot).GetComponent<ShopWindow>();
			window.Construct(_progressService, _adsService);
		}

		public async Task CreateUIRoot() {
			var uiRootGameObject = await _assetProvider.InstantiateAsync(AssetAddress.UI_ROOT_ADDRESS);
			_uiRoot = uiRootGameObject.transform;
		}
	}
}