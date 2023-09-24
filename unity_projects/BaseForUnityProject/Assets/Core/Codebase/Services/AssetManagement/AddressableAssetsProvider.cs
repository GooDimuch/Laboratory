using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

// using UnityEngine.AddressableAssets;
// using UnityEngine.ResourceManagement.AsyncOperations;
// using UnityEngine.ResourceManagement.ResourceLocations;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AddressableAssetsProvider : IAssetsProvider
    {
        public async UniTask<T> Load<T>(string address) where T : Object
        {
            throw new NotImplementedException();

            if (Utils.IsSameOrSubclass(typeof(Component), typeof(T)))
                return await LoadForComponent<T>(address);
            // var loadLocations = await TryLoadLocations<T>(address);
            // if (!loadLocations.isSuccess) return null;
            // var asyncOpHandler = Addressables.LoadAssetAsync<T>(loadLocations.locationsResult.First());
            // return await Load(asyncOpHandler);
        }

        private async UniTask/*<(bool isSuccess, IList<IResourceLocation> locationsResult)>*/ TryLoadLocations<T>(
            string address)
        {
            throw new NotImplementedException();

            // var locationsAsync = Addressables.LoadResourceLocationsAsync(address, typeof(T));
            // await locationsAsync.ToUniTask();
            // var locationsResult = locationsAsync.Result;
            // if (locationsAsync.Status == AsyncOperationStatus.Failed || locationsResult.Count == 0)
            // {
                // Debug.LogError($"Can't find addressable with Type = {typeof(T)} on address = {address}");
                // return (false, locationsResult);
            // }

            // return (true, locationsResult);
        }

        private async UniTask<T> LoadForComponent<T>(string address) where T: Object
        {
            var gameObject = await Load<GameObject>(address);
            if (!gameObject || !gameObject.TryGetComponent<T>(out var result))
            {
                Debug.LogError($"Can't find component with Type = {typeof(T)} on gameObject = {gameObject} with address = {address}");
                return null;
            }
            return result;
        }

        // private async UniTask<T> Load<T>(AsyncOperationHandle<T> asyncOpHandler) where T : Object
        // {
            // await asyncOpHandler.ToUniTask();
            // return asyncOpHandler.Result;
        // }
    }
}