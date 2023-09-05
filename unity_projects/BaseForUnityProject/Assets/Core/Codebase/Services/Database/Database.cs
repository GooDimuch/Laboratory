using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Constants;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace CodeBase.Infrastructure.Services
{
    public class Database : IDatabase
    {
        private enum RequestType
        {
            Get,
            Post,
        }

        private string s_token = null;

        public string Token
        {
            get => s_token;
            set => s_token = $"Bearer {value}";
        }

        public Database() { }

        #region Get

        public void SendGet(string url, Action<UnityWebRequest> requestHandler = null,
            Action<UnityWebRequest> errorHandler = null, bool needToLogError = true, bool checkToken = true) =>
            Get(url, requestHandler, errorHandler, needToLogError, checkToken);

        public async UniTask SendGetTask(
            string url, Action<UnityWebRequest> requestHandler = null, Action<UnityWebRequest> errorHandler = null,
            bool needToLogError = true, bool tryUntilSuccess = false, bool checkToken = true) =>
            await Send(RequestType.Get, url, requestHandler, errorHandler, 0, needToLogError, tryUntilSuccess,
                checkToken);

        private async UniTask Get(string url, Action<UnityWebRequest> requestHandler = null,
            Action<UnityWebRequest> errorHandler = null, bool needToLogError = true, bool checkToken = true)
        {
            var www = new UnityWebRequest(url);
            www.downloadHandler = new DownloadHandlerBuffer();
#if UNITY_SERVER || DEVELOP
            Debug.Log(url);
#endif
            if (checkToken) CheckToken(www);

            await www.SendWebRequest();

            ProcessResponse(www, requestHandler, errorHandler, needToLogError);
        }

        #endregion

        #region Post

        public void SendPost<TData>(TData data, string url,
            Action<UnityWebRequest> requestHandler = null, Action<UnityWebRequest> errorHandler = null,
            bool needToLogError = true, bool checkToken = true) =>
            Post(data, url, requestHandler, errorHandler, needToLogError, checkToken);

        public async UniTask SendPostTask<TData>(TData data, string url, Action<UnityWebRequest> requestHandler = null,
            Action<UnityWebRequest> errorHandler = null, bool needToLogError = true, bool tryUntilSuccess = false,
            bool checkToken = true) =>
            await Send(RequestType.Post, url, requestHandler, errorHandler, data, needToLogError, tryUntilSuccess,
                checkToken);

        private async UniTask Post<TData>(TData data, string url,
            Action<UnityWebRequest> requestHandler = null, Action<UnityWebRequest> errorHandler = null,
            bool needToLogError = true, bool checkToken = true)
        {
            var form = new WWWForm();
            var json = JsonConvert.SerializeObject(data);
#if UNITY_SERVER || DEVELOP
            Debug.Log($"Sent data: {json}, to {url}");
#endif            
            var www = UnityWebRequest.Post(url, form);

            var bytes = Encoding.UTF8.GetBytes(json);

            www.uploadHandler = new UploadHandlerRaw(bytes);
            www.SetRequestHeader(RequestHeaders.ContentType, RequestHeaders.ApplicationJson);

            if (checkToken) CheckToken(www);

            await www.SendWebRequest();

            ProcessResponse(www, requestHandler, errorHandler, needToLogError);
        }

        #endregion


        #region Common

        private async UniTask Send<TData>(RequestType type, string url, Action<UnityWebRequest> requestHandler,
            Action<UnityWebRequest> errorHandler, TData data, bool needToLogError = true,
            bool tryUntilSuccess = false, bool checkToken = true)
        {
            var success = false;
            var loading = true;

            void OnSuccess(UnityWebRequest www)
            {
                requestHandler?.Invoke(www);
                loading = false;
                success = true;
            }

            void OnFail(UnityWebRequest www)
            {
                errorHandler?.Invoke(www);
                loading = false;
            }

            do
            {
                switch (type)
                {
                    case RequestType.Get:
                        SendGet(url, OnSuccess, OnFail, needToLogError, checkToken);
                        break;
                    case RequestType.Post:
                        SendPost(data, url, OnSuccess, OnFail, needToLogError, checkToken);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                while (loading)
                {
                    await UniTask.Yield();
                }
            } while (tryUntilSuccess && !success);
        }

        private void ProcessResponse(UnityWebRequest www, Action<UnityWebRequest> requestHandler,
            Action<UnityWebRequest> errorHandler = null, bool needToLogError = true)
        {
            if (www.result == UnityWebRequest.Result.Success)
            {
                requestHandler?.Invoke(www);
#if UNITY_SERVER || DEVELOP
                Debug.Log($"Request: {www.url} " +
                               $"{www.result} " +
                               $"{www.responseCode} " +
                               $"{www.downloadHandler.text} ");
#endif
                return;
            }

            errorHandler?.Invoke(www);
            if (needToLogError)
            {
#if UNITY_SERVER || DEVELOP
                Debug.LogError($"Error Request: {www.error} " +
                               $"{www.result} " +
                               $"{www.responseCode} " +
                               $"{www.downloadHandler.text} " +
                               $"{www.url} ");
#endif
            }
        }

        private void CheckToken(UnityWebRequest www)
        {
            if (string.IsNullOrEmpty(Token))
            {
                Debug.LogError("Authorization Token Not Set! 401: Unauthorized");
                return;
            }

            www.SetRequestHeader(RequestHeaders.Authorization, Token);
        }

        #endregion
    }
    
    public static class UnityWebRequestExtension
    {
        public static TaskAwaiter<UnityWebRequest.Result> GetAwaiter(this UnityWebRequestAsyncOperation reqOp)
        {
            TaskCompletionSource<UnityWebRequest.Result> tsc = new();
            reqOp.completed += asyncOp => tsc.TrySetResult(reqOp.webRequest.result);
 
            if (reqOp.isDone)
                tsc.TrySetResult(reqOp.webRequest.result);
 
            return tsc.Task.GetAwaiter();
        }
    }
}