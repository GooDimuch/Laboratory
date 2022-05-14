using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.AssetManagement {
	public interface IAssetProvider : IService {
		void Initialize();
		Task<T> Load<T>(AssetReference assetReference) where T : class;
		Task<T> Load<T>(string address) where T : class;
		Task<GameObject> InstantiateAsync(string address);
		Task<GameObject> InstantiateAsync(string address, Vector3 at);
		Task<GameObject> InstantiateAsync(string address, Vector3 at, Quaternion with);
		void Cleanup();
	}
}