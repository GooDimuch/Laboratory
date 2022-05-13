using CodeBase.Services;

namespace CodeBase.UI.Services.UIFactory {
	public interface IUIFactory : IService {
		void CreateShop();
		void CreateUIRoot();
	}
}