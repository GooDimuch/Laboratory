using System;
using BaseSolution;
using CrewSim;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class State<T> : CrewSimInOutAutocomplete {
	public enum WorkMode {
		Input,
		Output
	}

	public WorkMode Mode { get; set; } = WorkMode.Output;
	[Header("State")] [ReadOnly] public T prevState;
	[ReadOnly] public T currentState;

	private InOut simObject;
	private bool force;

	public T CurrentState {
		get => currentState;
		set {
			if (Equals(value, CurrentState) && !force) { return; }
			prevState = CurrentState;
			currentState = value;
			if (simObject != null) { simObject.Value = CastCurrentStateToInOutLocal(simObject.Value.GetType()); }
			OnVariableChanged?.Invoke(CurrentState);
		}
	}

	protected override void InitializeOnSceneLoading(string sceneName) {
		var crewSimController = GameObject.FindGameObjectWithTag("CrewSimController").GetComponent<CrewSimController>();
		if (string.IsNullOrWhiteSpace(simObjectName)) return;
		simObject = (InOut) crewSimController?.findObjectByName(
				$"{(string.IsNullOrEmpty(systemName) || systemName == "Current" ? gameObject.scene.name : systemName)}" +
				$".{simObjectName}");

		force = true;
		CurrentState = CastInOutToCurrentState();
		force = false;
	}

	private T CastInOutToCurrentState() {
		try { return (T) (simObject?.Value ?? currentState); } catch (InvalidCastException) {
			return CastInOutToCurrentState(simObject?.Value);
		}
	}

	private object CastCurrentStateToInOutLocal(Type type) {
		try { return Convert.ChangeType(CurrentState, type); } catch (InvalidCastException) {
			return CastInOutToCurrentState(simObject?.Value);
		}
	}

	protected abstract T CastInOutToCurrentState(object inOut);
	protected abstract object CastCurrentStateToInOut(Type type);

	private void Update() {
		switch (Mode) {
			case WorkMode.Output:
				CurrentState = CastInOutToCurrentState();
				return;
			case WorkMode.Input:
				if (Type.GetTypeCode(typeof(T)) == TypeCode.Object) { force = true; }
				return;
			default: return;
		}
	}

	protected abstract bool Equals(T value, T state);

	public delegate void OnVariableChangeDelegate(T newState);

	public event OnVariableChangeDelegate OnVariableChanged;
}