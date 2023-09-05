using CodeBase.Infrastructure.Logic;
using CodeBase.Infrastructure.States;

namespace CodeBase.Infrastructure
{
    public class GameLoopState : IPayloadedState<Game>
    {
        private readonly ApplicationStateMachine _appStateMachine;
        private Game _game;

        public GameLoopState(ApplicationStateMachine appStateMachine)
        {
            _appStateMachine = appStateMachine;
        }

        public void Enter(Game game)
        {
            _game = game;
        }

        public void Exit() { }
    }
}