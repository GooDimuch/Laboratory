using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader : ISceneLoader
    {
        public SceneLoader() { }

        public async UniTask Load(string name, Action onLoaded = null)
        {
            // if (SceneManager.GetActiveScene().name == nextScene) {
            //     onLoaded?.Invoke();
            //     return;
            // }

            var waitNextScene = SceneManager.LoadSceneAsync(name);
            await waitNextScene.ToUniTask();
            onLoaded?.Invoke();
        }
    }
}