using System.Threading.Tasks;
using CodeBase.Services;

namespace CodeBase.UI.Services.UIFactory {
	public interface IUIFactory : IService {
		Task WarmUp();
		Task CreateShop();
		Task CreateUIRoot();
	}
}