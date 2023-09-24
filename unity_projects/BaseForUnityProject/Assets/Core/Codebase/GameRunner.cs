using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;
using Codebase.Scripts.UICommon;
using JetBrains.Annotations;
using MyBox;
using UnityEngine;
using Application = CodeBase.Infrastructure.Application;

namespace CodeBase.Scripts
{
    public class GameRunner : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain curtainPrefab;
        [SerializeField] private ErrorPopupView errorPrefab;
#if UNITY_EDITOR
        [SerializeField, ReadOnly, UsedImplicitly] private string currentState;
#endif
        private Application _app;

        public ServiceRegister Services { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            var curtain = Instantiate(curtainPrefab);
            Services = new ServiceRegister(curtain, errorPrefab, this);
        }

        private void Start()
        {
            _app = new Application(Services);
            _app.StateMachine.Enter<BootstrapState>();
        }

#if UNITY_EDITOR
        private void Update()
        {
            currentState = _app.StateMachine.ActiveState.GetType().Name;
        }
#endif
    }
}