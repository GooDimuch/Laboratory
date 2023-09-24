using CodeBase.Infrastructure.Services;
using CodeBase.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IMainMenuUIFactory : IService
    {
        UniTask<GameObject> CreateUIRoot(Transform at);
        void Cleanup();
    }
}