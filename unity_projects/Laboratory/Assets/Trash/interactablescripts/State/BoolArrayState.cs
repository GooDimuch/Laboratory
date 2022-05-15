using System;
using System.Linq;

public class BoolArrayState : State<bool[]> {
	protected override bool[] CastInOutToCurrentState(object inOut) { throw new System.NotImplementedException(); }
	protected override object CastCurrentStateToInOut(Type type) { throw new NotImplementedException(); }
	protected override bool Equals(bool[] value, bool[] state) => value?.SequenceEqual(state) ?? false;
}