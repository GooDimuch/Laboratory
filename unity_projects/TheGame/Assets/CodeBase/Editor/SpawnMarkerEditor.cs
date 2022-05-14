using CodeBase.Logic.EnemySpawners;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor {
	[CustomEditor(typeof(SpawnMarker))]
	public class SpawnMarkerEditor : UnityEditor.Editor {
		private const string TERRAIN_TAG = "Terrain";

		private static Terrain _terrain;
		private static Terrain Terrain => _terrain ??= GetTerrain();


		[DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
		public static void RenderCustomGizmo(SpawnMarker spawner, GizmoType gizmo) {
			UpdateSpawnerPosition(spawner.transform);

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(spawner.transform.position, 0.5f);
		}

		private static void UpdateSpawnerPosition(Transform spawnerTransform) {
			try {
				var terrainHeight = Terrain.SampleHeight(spawnerTransform.position);
				spawnerTransform.position =
					new Vector3(spawnerTransform.position.x, terrainHeight, spawnerTransform.position.z);
			}
			catch (MissingReferenceException e) { }
		}

		private static Terrain GetTerrain() =>
			GameObject.FindWithTag(TERRAIN_TAG)?.GetComponent<Terrain>();
	}
}