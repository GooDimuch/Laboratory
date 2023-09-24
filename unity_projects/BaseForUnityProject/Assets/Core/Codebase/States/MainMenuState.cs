using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.ErrorVisualizer;
using CodeBase.Infrastructure.States;
using CodeBase.Scripts;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class MainMenuState : IState
    {
        private readonly ApplicationStateMachine _appStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IMainMenuUIFactory _uiFactory;
        private IDatabase _database;
        private IErrorVisualizer _errorVisualizer;

        public MainMenuState(ApplicationStateMachine appStateMachine, ISceneLoader sceneLoader,
            LoadingCurtain loadingCurtain, IMainMenuUIFactory uiFactory, IDatabase database,
            IErrorVisualizer errorVisualizer)
        {
            _errorVisualizer = errorVisualizer;
            _database = database;
            _uiFactory = uiFactory;
            _appStateMachine = appStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter()
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(Consts.MainMenu, OnLoaded);
        }

        public void Exit() { }

        private async void OnLoaded()
        {
            var mainMenuRoot = await _uiFactory.CreateUIRoot(GameObject.FindWithTag(Consts.MainMenuTag).transform);
            _loadingCurtain.Hide();
        }

        private void OpenGame() => _appStateMachine.Enter<LoadGameState>();
    }
}