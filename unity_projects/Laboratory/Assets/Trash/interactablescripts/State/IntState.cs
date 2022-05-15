using System;

public class IntState : State<int> {
	protected override int CastInOutToCurrentState(object inOut) { throw new System.NotImplementedException(); }
	protected override object CastCurrentStateToInOut(Type type) { throw new NotImplementedException(); }
	protected override bool Equals(int value, int state) => value.Equals(currentState);
}