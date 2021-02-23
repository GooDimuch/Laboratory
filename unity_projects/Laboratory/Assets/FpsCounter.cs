using Unity.Collections;
using UnityEngine;

public class FpsCounter : MonoBehaviour {
	[SerializeField] [ReadOnly] private float currentFPS;
	[SerializeField] [ReadOnly] private float minFPS;
	[SerializeField] [ReadOnly] private float averageFPS;
	[SerializeField] [ReadOnly] private float maxFPS;
	public int isWaitFrames;

	private int _frames;

	private float totalFPS;
	private int numberOfFPS;

	private void Start() {
		minFPS = int.MaxValue;
		maxFPS = int.MinValue;
	}

	private void Update() {
		if (_frames++ < isWaitFrames) { return; }
		numberOfFPS++;
		currentFPS = 1f / Time.unscaledDeltaTime;
		totalFPS += currentFPS;

		minFPS = Mathf.Min(minFPS, currentFPS);
		averageFPS = totalFPS / numberOfFPS;
		maxFPS = Mathf.Max(maxFPS, currentFPS);
	}
}