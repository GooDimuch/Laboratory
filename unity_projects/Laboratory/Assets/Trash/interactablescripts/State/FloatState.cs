using System;
using System.Globalization;
using UnityEngine;

public class FloatState : State<float> {
	protected override float CastInOutToCurrentState(object inOut) => Convert.ToSingle(inOut);

	protected override object CastCurrentStateToInOut(Type type) {
		switch (Type.GetTypeCode(type)) {
			case TypeCode.Boolean: return Convert.ToBoolean(CurrentState);
			case TypeCode.Int32: return Convert.ToInt32(CurrentState);
			case TypeCode.Single: return Convert.ToSingle(CurrentState);
			case TypeCode.String: return Convert.ToString(CurrentState, CultureInfo.InvariantCulture);
			case TypeCode.UInt32: return Convert.ToUInt32(CurrentState);
			default: throw new ArgumentOutOfRangeException();
		}
	}

	protected override bool Equals(float value, float state) => Math.Abs(value - currentState) < Mathf.Epsilon;
}