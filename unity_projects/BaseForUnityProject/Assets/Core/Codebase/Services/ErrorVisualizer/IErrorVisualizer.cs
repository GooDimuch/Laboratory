using System;

namespace CodeBase.Infrastructure.Services.ErrorVisualizer
{
    public interface IErrorVisualizer
    {
        const string DefaultErrorPopupTitle = "Error";

        void ShowError(string errorMessage, string errorTitle = DefaultErrorPopupTitle, Action okClick = null);
    }
}