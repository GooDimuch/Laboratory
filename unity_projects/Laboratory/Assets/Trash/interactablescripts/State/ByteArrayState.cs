using System;
using System.Linq;

public class ByteArrayState : State<byte[]> {
	protected override byte[] CastInOutToCurrentState(object inOut) { throw new System.NotImplementedException(); }
	protected override object CastCurrentStateToInOut(Type type) { throw new NotImplementedException(); }
	protected override bool Equals(byte[] value, byte[] state) => value?.SequenceEqual(state) ?? false;
}