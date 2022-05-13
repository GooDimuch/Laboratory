using System;
using CodeBase.UI.Services.WindowService;
using CodeBase.UI.Windows;

namespace CodeBase.StaticData {
	[Serializable]
	public class WindowConfig {
		public WindowId WindowId;
		public BaseWindow Prefab;
	}
}