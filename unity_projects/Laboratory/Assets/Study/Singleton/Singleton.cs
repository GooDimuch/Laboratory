using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	public static bool isAppQuitting;
	private static T _instance;
	private static object _lock = new object();

	public static T Instance {
		get {
			if (isAppQuitting) return null;
			lock (_lock) {
				if (_instance == null) {
					_instance = FindObjectOfType<T>();
					if (_instance == null) {
						_instance = new GameObject($"[SINGLETON] {typeof(T)}").AddComponent<T>();
						DontDestroyOnLoad(_instance);
					}
				}
				return _instance;
			}
		}
	}

	public virtual void OnDestroy() { isAppQuitting = true; }
}