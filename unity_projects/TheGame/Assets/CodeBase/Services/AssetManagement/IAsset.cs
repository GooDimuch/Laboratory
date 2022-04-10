using UnityEngine;

namespace CodeBase.Services.AssetManagement {
	public interface IAsset : IService {
		GameObject Instantiate(string path);
		GameObject Instantiate(string path, Vector3 at);
		GameObject Instantiate(string path, Vector3 at, Quaternion with);
	}
}