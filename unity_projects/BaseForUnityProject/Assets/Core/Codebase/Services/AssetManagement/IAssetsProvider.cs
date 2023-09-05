using CodeBase.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssetsProvider : IService
    {
        UniTask<T> Load<T>(string path) where T : Object;
    }
}