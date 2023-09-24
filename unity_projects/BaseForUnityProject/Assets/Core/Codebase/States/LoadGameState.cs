using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Logic;
using CodeBase.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadGameState : IState
    {
        private readonly ApplicationStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;

        public LoadGameState(ApplicationStateMachine appStateMachine, ISceneLoader sceneLoader,
            LoadingCurtain loadingCurtain,
            IGameFactory gameFactory)
        {
            _stateMachine = appStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
        }

        public void Enter()
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(Consts.Game, () => OnLoaded());
        }

        public void Exit() =>
            _loadingCurtain.Hide();

        private async UniTask OnLoaded()
        {
            var game = new Game();
            _stateMachine.Enter<GameLoopState, Game>(game);
        }
    }
}