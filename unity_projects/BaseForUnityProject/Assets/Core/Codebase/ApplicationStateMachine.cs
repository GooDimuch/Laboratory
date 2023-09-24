using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;

namespace CodeBase.Infrastructure
{
    public class ApplicationStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        public IExitableState ActiveState { get; private set; }

        public ApplicationStateMachine(ServiceRegister services)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, services.SceneLoader),
                [typeof(MainMenuState)] = new MainMenuState(this, services.SceneLoader, services.Curtain,
                    services.MainMenuUIFactory, services.Database, services.ErrorVisualizer),
                [typeof(LoadGameState)] =
                    new LoadGameState(this, services.SceneLoader, services.Curtain, services.GameFactory),
                [typeof(GameLoopState)] = new GameLoopState(this),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void Enter<TState, TPayload1, TPayload2>(TPayload1 payload1, TPayload2 payload2)
            where TState : class, IPayloadedState<TPayload1, TPayload2>
        {
            var state = ChangeState<TState>();
            state.Enter(payload1, payload2);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            ActiveState?.Exit();

            var state = GetState<TState>();
            ActiveState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}