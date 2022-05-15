using System;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : BaseOutputController<float> {
	[Serializable]
	public class CurveConstructor {
		[HideInInspector] public AnimationCurve curve;
		public float minValue;
		public float maxValue;
		public float StartAngle;
		public float EndAngle;
		public bool create;
		[HideInInspector] public bool correct;
	}

#region constants
#endregion

#region editor
	[HideInInspector] public string savedSimObjectNameForIntState;
	[HideInInspector] public string savedSystemNameForIntState;
#endregion

#region inspector
	public Axis axisOfRotation;
	public bool stateConverted01;
	public List<CurveConstructor> constructors = new List<CurveConstructor>();
#endregion

#region internal variable
	private IntState intState;
	private bool multiMode;
#endregion

	private new void Start() {
		base.Start();
		multiMode = constructors.Count > 1 && TryGetComponent(out intState);
		if (multiMode) { intState.OnVariableChanged += newState => Animate(newState); }
	}

	private void Update() { }

	protected override void PlaySound(float newState) { }

	protected override void Animate(float newState) {
		var constructor = constructors[multiMode ? intState.CurrentState : 0];
		var newAngle = constructor.curve.Evaluate(stateConverted01
				? newState * (constructor.maxValue - constructor.minValue) + constructor.minValue
				: newState);
		var eulerAngles = transform.localRotation.eulerAngles;
		eulerAngles[(int) axisOfRotation] = newAngle;
		transform.localRotation = Quaternion.Euler(eulerAngles);
	}
}