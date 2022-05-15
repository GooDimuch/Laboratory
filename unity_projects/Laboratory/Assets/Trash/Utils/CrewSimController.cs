using System;
using System.Collections.Generic;
using System.Linq;
using CrewSim;
using CrewSim.Network;
using UnityEditor;
using UnityEngine;
using Section = System.Collections.Generic.Dictionary<string, string>;

namespace BaseSolution
{
    public class CrewSimController : MonoBehaviour
    {
        const string OVERRIDES_ARG = "-overrides=";

        [Serializable]
        public class NetConnInfo
        {
            [ReadOnly] public string address;
            [ReadOnly] public int dT;
            [ReadOnly] public long responseElapsed;
            [ReadOnly] public long sendElapsed;
            [ReadOnly] public bool isAlive;

            public NetConnInfo() { }

            public NetConnInfo(NetworkConnection conn) => Update(conn);

            public void Update(NetworkConnection conn)
            {
                address = conn?.Remote.ToString();
                dT = (int?)conn?.DeltaTime ?? 0;
                responseElapsed = (int?)conn?.ResponseElapsed ?? 0;
                sendElapsed = (int?)conn?.SendElapsed ?? 0;
                isAlive = (bool?)conn?.IsAlive ?? false;
            }
        }

        public string xmlAssetName;
        [Header("Шаг симуляции")] public int simulationPeriod = 10;
        [Header("Многопоточность")] public bool multitreading = false;
        [Header("Автобалансировка")] public bool autobalance = true;
        [ConditionalField(nameof(autobalance))] public float balanceRatio = 0.5f;
        [Header("Оптимизация записи")] public bool optimizeWrite = true;
        [Header("Синхронизация")] public bool synchronized = true;
        private SimRunner simRunner;

        [Header("Про симуляцию"), ReadOnly] public int FPS = 0;
        [ReadOnly] public int dT = 0;
        [ReadOnly] public int elapsed = 0;
        public bool pause;
        [Header("Сетевые подключения"), ReadOnly] public List<NetConnInfo> Connections = new List<NetConnInfo>();

        private void Awake()
        {
            // pause = false;
            // Connections.Clear();
            // var asynchronousLoadingController = GameObject.FindObjectOfType<LoadingConfiguration>()
            //         ?.GetComponent<LoadingConfiguration>();
            // xmlAssetName = asynchronousLoadingController?.assembly ?? xmlAssetName;
            // simRunner = new SimRunner(xmlAssetName,
            //         Application.streamingAssetsPath,
            //         new UnityLogger(),
            //         asynchronousLoadingController?.mainConfig,
            //         readConfigOverrides());
            // applyParams();
            // simRunner.initialize();
        }

        private void applyParams()
        {
            simRunner.SimulatingPeriod = simulationPeriod;
            simRunner.SyncSimulation = synchronized;
            simRunner.UseThreadPool = multitreading;
            simRunner.Autobalance = autobalance;
            simRunner.OptimizeWrite = optimizeWrite;
            simRunner.BalanceRatio = balanceRatio;
        }

        private Dictionary<string, Section> readConfigOverrides()
        {
            Dictionary<string, Section> result = new Dictionary<string, Section>();
            var args = System.Environment.GetCommandLineArgs();
            var overridesArg = args.FirstOrDefault(arg => arg.Trim().ToLowerInvariant().Contains(OVERRIDES_ARG));
            if (!string.IsNullOrEmpty(overridesArg))
            {
                overridesArg = overridesArg.Remove(0, OVERRIDES_ARG.Length);
                var entries = overridesArg.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var entry in entries)
                {
                    var sDividerIdx = entry.IndexOf(':');
                    var nDividerIdx = entry.IndexOf(',');
                    string section = entry.Substring(0, sDividerIdx);
                    string name = entry.Substring(sDividerIdx + 1, nDividerIdx - sDividerIdx - 1);
                    string value = entry.Substring(nDividerIdx + 1);
                    if (!result.ContainsKey(section)) result.Add(section, new Section());
                    if (result[section].ContainsKey(name)) result[section].Remove(name);
                    result[section].Add(name, value);
                }
            }
            return result;
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            FPS = (int?)simRunner?.Time.FPS ?? 0;
            dT = (int?)simRunner?.Time.DeltaTime ?? 0;
            elapsed = (int?)simRunner?.Time.ElapsedTime ?? 0;
            int i = 0;
            foreach (NetworkConnection conn in simRunner?.NetworkConnections)
            {
                if (Connections.Count <= i)
                    Connections.Add(new NetConnInfo(conn));
                else
                    Connections[i]?.Update(conn);
                i++;
            }
            if (Connections.Count > i) Connections.RemoveRange(i, Connections.Count - i);
            simRunner?.pause(pause);
            if (simRunner != null) applyParams();
#endif
        }

        public SimObject findObjectByName(string name) => simRunner?.findSimObjectByName(name);

        private void Start() { simRunner.start(); }

        private void OnDestroy() { simRunner.stop(); }

        private static CrewSimController _instance;

        private static CrewSimController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindGameObjectWithTag(nameof(CrewSimController))?
                            .GetComponent<CrewSimController>();
                }
                return _instance;
            }
        }

        public static InOut GetInOut(string systemName, string simObjectName) =>
                GetInOut($"{systemName}.{simObjectName}");

        public static InOut GetInOut(string simObjectPath)
        {
            if (Instance != null)
            {
                return (InOut)Instance.findObjectByName($"{simObjectPath}");
            }
            else
            { //this is a crunch and it is intended to allow projects with custom crewsimcontroller to work as expected with BaseSolution states, for instance - buk SVU.
                var controller = GameObject.FindWithTag("CrewSimController")?.GetComponent(typeof(CrewSimController));
                var method = controller?.GetType().GetMethod("findObjectByName");
                if (method == null) return default;
                return (InOut)method.Invoke(controller, new object[] { simObjectPath });
            }
        }
    }
}