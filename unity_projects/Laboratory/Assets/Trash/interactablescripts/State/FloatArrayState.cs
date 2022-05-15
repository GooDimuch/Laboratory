using System;
using System.Linq;

public class FloatArrayState : State<float[]> {
	protected override float[] CastInOutToCurrentState(object inOut) { throw new System.NotImplementedException(); }
	protected override object CastCurrentStateToInOut(Type type) { throw new NotImplementedException(); }
	protected override bool Equals(float[] value, float[] state) => value?.SequenceEqual(state) ?? false;
}