using CodeBase.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetsProvider _assetsProvider;

        public GameFactory(IAssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
        }

        public async UniTask<GameObject> CreateHud()
        {
            var hudPrefab = await _assetsProvider.Load<GameObject>(AssetsPath.HudPath);
            var hud = Object.Instantiate(hudPrefab);
            return hud;
        }

        public void Cleanup() { }
    }
}