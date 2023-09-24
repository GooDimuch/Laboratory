using CodeBase.Infrastructure.States;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class BootstrapState : IState
    {
        public const string BootSceneName = Consts.Boot;
        private readonly ApplicationStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;

        public BootstrapState(ApplicationStateMachine stateMachine, ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            if (SceneManager.GetActiveScene().name == BootSceneName)
            {
                EnterMainMenu();
                return;
            }

            _sceneLoader.Load(BootSceneName, onLoaded: EnterMainMenu);
        }

        public void Exit() { }

        private void EnterMainMenu() =>
            _stateMachine.Enter<MainMenuState>();
    }
}