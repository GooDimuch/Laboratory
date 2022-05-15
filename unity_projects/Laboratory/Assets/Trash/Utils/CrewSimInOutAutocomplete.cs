using UnityEngine;

public abstract class CrewSimInOutAutocomplete : SceneDependentMonoBehavior {
	[Header("CrewSim Settings")]
	[Tooltip("Ignore in Editor script")]
	public bool ignoreEmptinessInES;

	[ReadOnly] public string systemName;
	[ReadOnly] public string simObjectName;
	[HideInInspector] public int listIdx;
	[HideInInspector] public int listIOIdx;

	public string GetFullPath()
    {
		return systemName + "." + simObjectName;
	}
}