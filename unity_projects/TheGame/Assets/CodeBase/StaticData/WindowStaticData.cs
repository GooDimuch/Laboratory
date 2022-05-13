using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData {
	[CreateAssetMenu(menuName = "Static Data/Window", fileName = "WindowData")]
	public class WindowStaticData : ScriptableObject {
		public List<WindowConfig> WindowConfigs;
	}
}