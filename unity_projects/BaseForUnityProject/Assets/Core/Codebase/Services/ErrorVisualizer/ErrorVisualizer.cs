using System;
using Codebase.Scripts.UICommon;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Services.ErrorVisualizer
{
    public class ErrorVisualizer : IErrorVisualizer
    {
        private ErrorPopupView _errorPrefab;
        public ErrorVisualizer(ErrorPopupView errorPrefab)
        {
            _errorPrefab = errorPrefab;
        }

        public void ShowError(string errorMessage, string errorTitle = IErrorVisualizer.DefaultErrorPopupTitle, Action okClick = null)
        {
            var error = Object.Instantiate(_errorPrefab);
            error.Initialize(errorMessage, errorTitle, okClick);
        }
    }
}