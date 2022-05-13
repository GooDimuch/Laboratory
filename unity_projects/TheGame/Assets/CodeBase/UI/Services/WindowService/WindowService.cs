using System;
using CodeBase.UI.Services.UIFactory;

namespace CodeBase.UI.Services.WindowService {
	public class WindowService : IWindowService {
		private readonly IUIFactory _uiFactory;
		public WindowService(IUIFactory uiFactory) {
			_uiFactory = uiFactory;
		}

		public void Open(WindowId windowId) {
			switch (windowId) {
				case WindowId.Unknown:
					break;
				case WindowId.Shop:
					_uiFactory.CreateShop();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null);
			}
		}
	}
}