using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetsProvider : IAssetsProvider
    {
        public async UniTask<T> Load<T>(string path) where T : Object => (T) await Resources.LoadAsync<T>(path);
    }
}