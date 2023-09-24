using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace CodeBase.Infrastructure.Services
{
    public interface IDatabase
    {
        string Token { get; set; }

        void SendGet(string url, Action<UnityWebRequest> requestHandler = null,
            Action<UnityWebRequest> errorHandler = null, bool needToLogError = true, bool checkToken = true);

        UniTask SendGetTask(
            string url, Action<UnityWebRequest> requestHandler = null, Action<UnityWebRequest> errorHandler = null,
            bool needToLogError = true, bool tryUntilSuccess = false, bool checkToken = true);

        void SendPost<TData>(TData data, string url,
            Action<UnityWebRequest> requestHandler = null, Action<UnityWebRequest> errorHandler = null,
            bool needToLogError = true, bool checkToken = true);

        UniTask SendPostTask<TData>(TData data, string url, Action<UnityWebRequest> requestHandler = null,
            Action<UnityWebRequest> errorHandler = null, bool needToLogError = true, bool tryUntilSuccess = false,
            bool checkToken = true);
    }
}