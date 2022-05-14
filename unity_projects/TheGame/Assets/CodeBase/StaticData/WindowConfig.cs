using System;
using CodeBase.UI.Services.WindowService;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData {
	[Serializable]
	public class WindowConfig {
		public WindowId WindowId;
		public AssetReferenceGameObject PrefabReferance;
	}
}