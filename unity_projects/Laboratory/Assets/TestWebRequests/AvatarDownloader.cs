using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Common.Base.Types.Enums;
using Cysharp.Threading.Tasks;
using LoadedLions.NetModule.CronosPlayModule;
using MyBox;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace LoadedLions.GlobalModule
{
    public class AvatarDownloader : MonoBehaviour
    {
        [SerializeField] private string _statisticDirectory = @"D:\";
        [SerializeField] private int _webRequestTimeout = 5;
        [SerializeField] private int _loadDelay = 1000;
        [SerializeField] private int _amountOfTokens = 100;
        [SerializeField] private int _statsRate = 100;

        [SerializeField, ReadOnly] private bool _downloading;
        [SerializeField, ReadOnly] private long _successful;
        [SerializeField, ReadOnly] private long _failed;
        [SerializeField, ReadOnly] private string _statsPath;

        [ButtonMethod] private void StartDownload()
        {
            _cts = new CancellationTokenSource();
            StartDownloadAvatars(_cts.Token);
        }

        [ButtonMethod] private void StopDownload()
        {
            if (!_cts.IsCancellationRequested)
                _cts.Cancel();
        }

        [ButtonMethod] private async void WriteStatisticResult()
        {
            string result =
                $"[Result]: [Successful] = {_successful}\t[Failed] = {_failed}\n{string.Join("\n", _errors.Select(pair => $"[{pair.Value}] {pair.Key}"))}\n";
            Debug.Log(result);
            await AppendStats(result);
        }

        private DownloadHelper _helper;
        private CancellationTokenSource _cts;
        private readonly Dictionary<string, int> _errors = new Dictionary<string, int>();
        private int _amountOfLines;

        private void Awake()
        {
            Reset();
        }

        private void Reset()
        {
            _downloading = false;
            _successful = 0;
            _failed = 0;
            _errors.Clear();
        }

        private async void StartDownloadAvatars(CancellationToken cancellationToken)
        {
            if (_downloading)
                return;

            Reset();
            _helper = new DownloadHelper(_webRequestTimeout);
            _statsPath = Path.Combine(_statisticDirectory,
                $"Stats_{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToShortTimeString().Replace(":", "-")}.txt");
            using (File.Create(_statsPath)) { }

            Debug.Log($"[Started]");
            _downloading = true;
            var tasks = new List<UniTask>();
            for (int i = 0; i < _amountOfTokens; i++)
            {
                tasks.Add(StartDownloadAvatar(AvatarType.CuberCubs, i + 1, cancellationToken));
                tasks.Add(StartDownloadAvatar(AvatarType.LoadedLions, i + 1, cancellationToken));
            }
            while (tasks.Any(task => task.Status == UniTaskStatus.Pending))
                await UniTask.Yield();
            // await UniTask.WhenAll(tasks);
            WriteStatisticResult();
            _downloading = false;
            Debug.Log($"[Stopped]");
        }

        private async UniTask StartDownloadAvatar(AvatarType type, int tokenId, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var stat = string.Empty;
                try
                {
                    await _helper.LoadAvatar(type, tokenId);
                    // stat = $"[Successful] {type}[{tokenId}]";
                    _successful++;
                }
                catch (WebException e)
                {
                    if (e.errorCode == 429)
                        await UniTask.Delay(2000, cancellationToken: cancellationToken);
                    stat = $"[Failed] {e.Message}";
                    if (_errors.ContainsKey(e.error))
                        _errors[e.error]++;
                    else
                        _errors.Add(e.error, 1);
                    _failed++;
                }
                catch (Exception e)
                {
                    stat = $"[Failed] {e.Message}";
                    if (_errors.ContainsKey(e.Message))
                        _errors[e.Message]++;
                    else
                        _errors.Add(e.Message, 1);
                    _failed++;
                }
                try
                {
                    if (!string.IsNullOrWhiteSpace(stat))
                    {
                        Debug.Log(stat);
                        await AppendStats(stat);
                    }
                    _amountOfLines++;
                    if (_amountOfLines > _statsRate)
                    {
                        WriteStatisticResult();
                        _amountOfLines = 0;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log($"Write stat failed. {e.Message}");
                }
                await UniTask.Delay(_loadDelay, cancellationToken: cancellationToken);
            }
        }

        private async Task AppendStats(string text)
        {
            using var sw = File.AppendText(_statsPath);
            await sw.WriteLineAsync(text);
        }
    }

    public class DownloadHelper
    {
        private readonly int _webRequestTimeout;
        private readonly Dictionary<AvatarType, string> _contracts;
        private readonly CronosPlayConfig _config;

        public DownloadHelper(int webRequestTimeout)
        {
            _webRequestTimeout = webRequestTimeout;
            _config = Resources.Load<CronosPlayConfig>(nameof(CronosPlayConfig));
            _contracts = Resources.Load<ContractsData>(nameof(ContractsData)).Data;
        }

        public async UniTask<bool> LoadAvatar(AvatarType nftType, int tokenId) =>
            await LoadSpriteAsync(await GetSpriteUri(nftType, tokenId));

        private async UniTask<string> GetSpriteUri(AvatarType nftType, int tokenId)
        {
            var loadSpriteUri =
                ReplaceIpfsToHttps(await GetUri(_contracts[nftType], tokenId.ToString()));

            var request = UnityWebRequest.Get(loadSpriteUri);
            request.timeout = _webRequestTimeout;
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                throw new WebException(loadSpriteUri, request);

            string json = string.Empty;
            try
            {
                json = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                var data = JsonUtility.FromJson<AvatarResponse>(json);

                if (data == null || string.IsNullOrWhiteSpace(data.image))
                    throw new Exception();

                return ReplaceIpfsToHttps(data.image);
            }
            catch (Exception e)
            {
                throw new Exception($"Parse Json failed.\t" +
                    $"Error: [{e.Message}]\t" +
                    $"Url: [{loadSpriteUri}]\t" +
                    $"Json: '{json}'");
            }

            string ReplaceIpfsToHttps(string uri) =>
                uri.StartsWith("ipfs://") ? uri.Replace("ipfs://", "https://ipfs.io/ipfs/") : uri;
        }

        private async UniTask<bool> LoadSpriteAsync(string url)
        {
            var request = UnityWebRequestTexture.GetTexture(url);
            request.timeout = GlobalApiHelper.WebRequestTimeout;
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                throw new WebException(url, request);

            var tex = DownloadHandlerTexture.GetContent(request);
            Object.Destroy(tex);
            return true;
        }

        private async UniTask<string> GetUri(string contract, string tokenId) =>
            await ERC721.URI(_config.Chain, _config.Network, contract, tokenId);

        private class AvatarResponse
        {
            public string name;
            public string description;
            public string image;
            public AvatarResponseAttributes[] attributes;
        }

        private class AvatarResponseAttributes
        {
            public string trait_type;
            public string value;
        }
    }

    public class WebException : Exception
    {
        public int errorCode;
        public string error;

        public WebException(string url, UnityWebRequest request) : base(
            $"Url: {url}\t" +
            $"Result: {request.result}\t" +
            $"Error: {ReplaceNewLines(request.error)}")
        {
            TrySetErrorCode(request.error, 400, 404, 429, 500, 504);
            error = request.error;
        }

        private static string ReplaceNewLines(string error) =>
            Regex.Replace(
                error.Replace(Environment.NewLine, "")
                    .Replace("\r\n", "")
                    .Replace("\n", ""),
                @"\r\n?|\n", "");

        private bool TrySetErrorCode(string error, params int[] codes)
        {
            foreach (var code in codes)
            {
                if (!error.Contains($"{code}"))
                    continue;
                errorCode = code;
                return true;
            }
            return false;
        }
    }
}
