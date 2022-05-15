using System;
using System.Globalization;

public class BoolState : State<bool> {
	protected override bool CastInOutToCurrentState(object inOut) => Convert.ToBoolean(inOut);

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

	protected override bool Equals(bool value, bool state) => value == currentState;
}