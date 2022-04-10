﻿using UnityEngine;

namespace CodeBase.Data {
	public static class DataExtension {
		public static Vector2Data AsVectorData(this Vector2 vector) =>
			new Vector2Data(vector.x, vector.y);

		public static Vector3Data AsVectorData(this Vector3 vector) =>
			new Vector3Data(vector.x, vector.y, vector.z);

		public static Vector3 AsUnityVector(this Vector3Data vector3Data) =>
			new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);

		public static string ToJson<T>(this T obj) =>
			JsonUtility.ToJson(obj);

		public static T ToDeserialize<T>(this string json) =>
			JsonUtility.FromJson<T>(json);
	}
}