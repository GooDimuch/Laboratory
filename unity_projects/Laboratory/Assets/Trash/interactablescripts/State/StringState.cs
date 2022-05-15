using System;

public class StringState : State<string> {
	protected override string CastInOutToCurrentState(object inOut) { throw new NotImplementedException(); }
	protected override object CastCurrentStateToInOut(Type type) { throw new NotImplementedException(); }

	protected override bool Equals(string value, string state) =>
			value.Equals(currentState, StringComparison.OrdinalIgnoreCase);
}