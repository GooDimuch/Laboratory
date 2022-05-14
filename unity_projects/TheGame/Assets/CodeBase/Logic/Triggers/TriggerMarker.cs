using UnityEngine;

namespace CodeBase.Logic.Triggers {
	public class TriggerMarker : MonoBehaviour {
		public Vector3 size = Vector3.one;
		[SerializeField] private Color Color = new Color(0, 1, 0, 0.3f);

		private void OnDrawGizmos() {
			Gizmos.color = Color;
			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
			Gizmos.DrawCube(Vector3.zero, size);
		}
	}
}