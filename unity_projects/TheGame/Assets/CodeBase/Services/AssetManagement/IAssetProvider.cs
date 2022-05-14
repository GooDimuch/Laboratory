using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.AssetManagement {
	public interface IAssetProvider : IService {
		void Initialize();
		Task<T> Load<T>(AssetReference assetReference) where T : class;
		Task<T> Load<T>(string address) where T : class;
		GameObject Instantiate(string path);
		GameObject Instantiate(string path, Vector3 at);
		GameObject Instantiate(string path, Vector3 at, Quaternion with);
		void Cleanup();
	}
}