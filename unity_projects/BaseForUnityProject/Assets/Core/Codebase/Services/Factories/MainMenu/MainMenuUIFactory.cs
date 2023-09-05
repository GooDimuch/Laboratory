using CodeBase.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class MainMenuUIFactory : IMainMenuUIFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private GameObject _uiRoot;

        public MainMenuUIFactory(IAssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
        }

        public async UniTask<GameObject> CreateUIRoot(Transform at)
        {
            var rootPrefab = await _assetsProvider.Load<GameObject>(AssetsPath.MainMenuPath);
            _uiRoot = Object.Instantiate(rootPrefab, at);
            return _uiRoot;
        }

        public void Cleanup() => _uiRoot = null;
    }
}