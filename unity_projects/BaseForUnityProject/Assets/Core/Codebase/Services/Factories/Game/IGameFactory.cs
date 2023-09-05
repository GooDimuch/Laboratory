using CodeBase.Infrastructure.Logic;
using CodeBase.Infrastructure.Services;
using CodeBase.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        UniTask<GameObject> CreateHud();
        void Cleanup();
    }
}