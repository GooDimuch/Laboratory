using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Services.AssetManagement {
	public class Asset : IAsset {
		public GameObject Instantiate(string path) =>
			Instantiate(path, Vector3.zero, Quaternion.identity);

		public GameObject Instantiate(string path, Vector3 at) =>
			Instantiate(path, at, Quaternion.identity);

		public GameObject Instantiate(string path, Vector3 at, Quaternion with) {
			var prefab = Resources.Load<GameObject>(path);
			if (prefab == null) throw new NullReferenceException($"Prefab not found by path '{path}'");
			return Object.Instantiate(prefab, at, with);
		}
	}
}