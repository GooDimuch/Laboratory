using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.ErrorVisualizer;
using CodeBase.Scripts;
using Codebase.Scripts.UICommon;

namespace CodeBase.Infrastructure.Services
{
    public class ServiceRegister
    {
        public LoadingCurtain Curtain { get; private set; }
        public ISceneLoader SceneLoader { get; private set; }
        public ICoroutineRunner CoroutineRunner { get; private set; }
        public IGameFactory GameFactory { get; private set; }
        public IMainMenuUIFactory MainMenuUIFactory { get; private set; }
        public IAssetsProvider AssetsProvider { get; private set; }
        public IErrorVisualizer ErrorVisualizer { get; private set; }
        public IDatabase Database { get; private set; }

        public ServiceRegister(LoadingCurtain loadingCurtain, ErrorPopupView errorPopup, ICoroutineRunner coroutineRunner)
        {
            Curtain = loadingCurtain;
            CoroutineRunner = coroutineRunner;
            Database = new Database();
            ErrorVisualizer = new ErrorVisualizer.ErrorVisualizer(errorPopup);
            SceneLoader = new SceneLoader();
            AssetsProvider = new AssetsProvider();
            GameFactory = new GameFactory(AssetsProvider);
            MainMenuUIFactory = new MainMenuUIFactory(AssetsProvider);
        }
    }
}