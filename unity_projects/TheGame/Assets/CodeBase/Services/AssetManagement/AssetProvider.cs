﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace CodeBase.Services.AssetManagement {
	public class AssetProvider : IAssetProvider {
		private readonly Dictionary<string, AsyncOperationHandle> _completedCache =
			new Dictionary<string, AsyncOperationHandle>();

		private readonly Dictionary<string, List<AsyncOperationHandle>> _handles =
			new Dictionary<string, List<AsyncOperationHandle>>();

		public void Initialize() =>
			Addressables.InitializeAsync();

		public async Task<T> Load<T>(AssetReference assetReference) where T : class {
			if (_completedCache.TryGetValue(assetReference.AssetGUID, out var completedHandle))
				return completedHandle.Result as T;

			return await RunWithCacheOnComplete(
				Addressables.LoadAssetAsync<T>(assetReference),
				cacheKey: assetReference.AssetGUID);
		}

		public async Task<T> Load<T>(string address) where T : class {
			if (_completedCache.TryGetValue(address, out var completedHandle))
				return completedHandle.Result as T;

			return await RunWithCacheOnComplete(
				Addressables.LoadAssetAsync<T>(address),
				cacheKey: address);
		}

		public void Cleanup() {
			foreach (var handle in _handles.Values.SelectMany(resourceHandles => resourceHandles))
				Addressables.Release(handle);

			_completedCache.Clear();
			_handles.Clear();
		}

		public GameObject Instantiate(string path) =>
			Instantiate(path, Vector3.zero, Quaternion.identity);

		public GameObject Instantiate(string path, Vector3 at) =>
			Instantiate(path, at, Quaternion.identity);

		public GameObject Instantiate(string path, Vector3 at, Quaternion with) {
			var prefab = Resources.Load<GameObject>(path);
			if (prefab == null) throw new NullReferenceException($"Prefab not found by path '{path}'");
			return Object.Instantiate(prefab, at, with);
		}

		private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class {
			handle.Completed += completeHandle => { _completedCache[cacheKey] = completeHandle; };

			AddHandle(cacheKey, handle);

			return await handle.Task;
		}

		private void AddHandle(string key, AsyncOperationHandle handle) {
			if (!_handles.TryGetValue(key, out var resourceHandles)) {
				resourceHandles = new List<AsyncOperationHandle>();
				_handles[key] = resourceHandles;
			}

			resourceHandles.Add(handle);
		}
	}
}