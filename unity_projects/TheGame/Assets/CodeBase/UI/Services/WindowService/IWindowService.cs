using CodeBase.Services;

namespace CodeBase.UI.Services.WindowService {
	public interface IWindowService : IService {
		void Open(WindowId windowId);
	}
}