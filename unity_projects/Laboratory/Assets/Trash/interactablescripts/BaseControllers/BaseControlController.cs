using System;
using System.Linq;
using UnityEngine;

[HelpURL("https://git.logika.tech/sim/landvehicles/-/wikis/Controls/BaseControl")]
// [RequireComponent(typeof(State<T>))]
public abstract class BaseControlController<T> : MonoBehaviour {
	public enum Axis {
		X,
		Y,
		Z
	}

#region constants
#endregion

#region inspector
#endregion

#region internal variable
	private State<T> state;

	protected T CurrentState => state.CurrentState;
#endregion

	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	private void Reset() {
		switch (Type.GetTypeCode(typeof(T))) {
			case TypeCode.Byte:
				gameObject.AddComponent<ByteState>();
				break;
			case TypeCode.Boolean:
				gameObject.AddComponent<BoolState>();
				break;
			case TypeCode.Int32:
				gameObject.AddComponent<IntState>();
				break;
			case TypeCode.Single:
				gameObject.AddComponent<FloatState>();
				break;
			case TypeCode.String:
				gameObject.AddComponent<StringState>();
				break;
			case TypeCode.Object:
				switch (Type.GetTypeCode(typeof(T).GetElementType())) {
					case TypeCode.Byte:
						gameObject.AddComponent<ByteArrayState>();
						break;
					case TypeCode.Boolean:
						gameObject.AddComponent<BoolArrayState>();
						break;
					case TypeCode.Int32:
						gameObject.AddComponent<IntArrayState>();
						break;
					case TypeCode.Single:
						gameObject.AddComponent<FloatArrayState>();
						break;
					default: throw new ArgumentOutOfRangeException();
				}
				break;
			default: throw new ArgumentOutOfRangeException();
		}
		state = gameObject.GetComponent<State<T>>();
	}

	protected void Awake() { }

	protected void Start() {
		state = gameObject.GetComponent<State<T>>();
		state.OnVariableChanged += OnVariableChanged;

		Animate(CurrentState);
	}

	private void OnVariableChanged(T newState) {
		PlaySound(newState);
		Animate(newState);
	}

	protected abstract void PlaySound(T newState);

	protected abstract void Animate(T newState);
}