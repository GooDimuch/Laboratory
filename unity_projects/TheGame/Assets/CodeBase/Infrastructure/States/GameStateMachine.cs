﻿using System;
using System.Collections.Generic;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Factory;
using CodeBase.Services.PersistantProgress;
using CodeBase.Services.SaveLoadService;

namespace CodeBase.Infrastructure.States {
	public class GameStateMachine {
		private readonly Dictionary<Type, IExitableState> _states;
		private IExitableState _activeState;

		public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services) {
			_states = new Dictionary<Type, IExitableState> {
				[typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
				[typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain,
					services.Single<IGameFactory>(), services.Single<IPersistantProgressService>()),
				[typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IPersistantProgressService>(),
					services.Single<ISaveLoadService>()),
				[typeof(GameLoopState)] = new GameLoopState(this),
			};
		}

		public void Enter<TState>() where TState : class, IState {
			var state = ChangeState<TState>();
			state.Enter();
		}

		public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload> {
			var state = ChangeState<TState>();
			state.Enter(payload);
		}

		private TState ChangeState<TState>() where TState : class, IExitableState {
			_activeState?.Exit();

			var state = GetState<TState>();
			_activeState = state;

			return state;
		}

		private TState GetState<TState>() where TState : class, IExitableState =>
			_states[typeof(TState)] as TState;
	}
}