using System;
using UnityEngine;

public class ConnectorImitator : MonoBehaviour {
	public enum TypeValue {
		Bool,
		Int,
		Float,
	}

	public bool sendValue;
	public GameObject controller;

	public State<object>.WorkMode mode;
	public TypeValue type;
	[Header("Для BoolState")] public bool valueBool;
	[Header("Для IntState")] public int valueInt;
	[Header("Для FloatState")] public float valueFloat;

	private BaseTouchInputController<bool> inputBool;
	private BaseTouchInputController<int> inputInt;
	private BaseTouchInputController<float> inputFloat;

	private State<bool> outputBool;
	private State<int> outputInt;
	private State<float> outputFloat;

	void Awake() {
		sendValue = false;
		enabled = false;
	}

	void Start() {
		switch (type) {
			case TypeValue.Bool:
				if (mode == State<object>.WorkMode.Input) {
					inputBool = controller.GetComponent<BaseTouchInputController<bool>>();
				} else { outputBool = controller.GetComponent<State<bool>>(); }
				break;
			case TypeValue.Int:
				if (mode == State<object>.WorkMode.Input) {
					inputInt = controller.GetComponent<BaseTouchInputController<int>>();
				} else { outputInt = controller.GetComponent<State<int>>(); }
				break;
			case TypeValue.Float:
				if (mode == State<object>.WorkMode.Input) {
					inputFloat = controller.GetComponent<BaseTouchInputController<float>>();
				} else { outputFloat = controller.GetComponent<State<float>>(); }
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}

	void Update() {
		switch (type) {
			case TypeValue.Bool:
				if (mode == State<object>.WorkMode.Input) {
					if (sendValue) { inputBool.BlockAndSetState(valueBool); } else if (inputBool.BlockedTouch) {
						inputBool.ReleaseBlockByExternalCall();
					}
				} else {
					if (sendValue) { outputBool.CurrentState = valueBool; }
				}
				break;
			case TypeValue.Int:
				if (mode == State<object>.WorkMode.Input) {
					if (sendValue) { inputInt.BlockAndSetState(valueInt); } else if (inputInt.BlockedTouch) {
						inputInt.ReleaseBlockByExternalCall();
					}
				} else {
					if (sendValue) { outputInt.CurrentState = valueInt; }
				}
				break;
			case TypeValue.Float:
				if (mode == State<object>.WorkMode.Input) {
					if (sendValue) { inputFloat.BlockAndSetState(valueFloat); } else if (inputFloat.BlockedTouch) {
						inputFloat.ReleaseBlockByExternalCall();
					}
				} else {
					if (sendValue) { outputFloat.CurrentState = valueFloat; }
				}
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}
}