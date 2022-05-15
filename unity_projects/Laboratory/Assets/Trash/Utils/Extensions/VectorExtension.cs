using System;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extension {
	public const int VECTOR2_SIZE = 2;
	public enum Axis {
		X = 0,
		Y = 1,
		Pitch = 0,
		Yaw = 1,
	}

	public static Vector2 Set(this ref Vector2 vector, Vector2 newVector) {
		vector.x = newVector.x;
		vector.y = newVector.y;
		return vector;
	}

	public static Vector2 SwapAxises(this Vector2 vector) {
		var temp = vector[(int) Axis.X];
		vector[(int) Axis.X] = vector[(int) Axis.Y];
		vector[(int) Axis.Y] = temp;
		return vector;
	}

	public static Vector2 Invert(this Vector2 vector, params Axis[] axises) {
		foreach (var axis in axises) { vector[(int) axis] = -vector[(int) axis]; }
		return vector;
	}
	
	public static Vector2 FloatsToVector2(IReadOnlyList<float> array, int index) =>
			new Vector2(array[index * VECTOR2_SIZE + 0], array[index * VECTOR2_SIZE + 1]);

    public static Vector2 Clamp(this Vector2 _this, float min, float max) {
        _this.Set(Mathf.Clamp(_this.x, min, max), Mathf.Clamp(_this.y, min, max));
        return _this;
    }

    public static Vector2 MinXMaxY(this Vector2 _this, Vector2 other) {
        _this.Set(Mathf.Min(float.IsNegativeInfinity(_this.x) ? other.x : _this.x, other.x), Mathf.Max(float.IsNegativeInfinity(_this.y) ? other.y : _this.y, other.y));
        return _this;
    }

    public static float MinXY(this Vector2 _this) => Mathf.Min(_this.x, _this.y);
    public static float MaxXY(this Vector2 _this) => Mathf.Max(_this.x, _this.y);
    public static bool InRangeXY(this Vector2 _this, float value, bool exclusive = false) => exclusive ? value > _this.x && value < _this.y : value >= _this.x && value <= _this.y;
    public static Vector2 Clamp01(this Vector2 _this) => Clamp(_this, 0f, 1f);
    public static float LerpXY(this Vector2 _this, float t) => Mathf.Lerp(_this.x, _this.y, t);
    public static float UnlerpXY(this Vector2 _this, float t) => (t - _this.x) / (_this.y - _this.x);
    public static Vector2 Unscale(this Vector2 _this, float x, float y) { _this.Set(_this.x / x, _this.y / y); return _this; }
    public static Vector2 Unscale(this Vector2 _this, Vector2 other) => _this.Unscale(other.x, other.y);

    public static Vector2 Projection(this Vector2 _this, Ray2D line, out float fromLineOrigin) => line.origin + line.direction.normalized * (fromLineOrigin = line.Project(_this));
    public static Vector2 Projection(this Vector2 _this, Ray2D line) => Projection(_this, line, out float fromLineOrigin);

    public static Vector2 ProjectionVector(this Vector2 _this, Ray2D line, out float fromLineOrigin) => _this.Projection(line, out fromLineOrigin) - _this;
    public static Vector2 ProjectionVector(this Vector2 _this, Ray2D line) => ProjectionVector(_this, line, out float fromLineOrigin);
    public static float Distance(this Vector2 _this, Ray2D line) => ProjectionVector(_this, line).magnitude;


    public static float Project(this Ray2D _this, Vector2 point) => Vector2.Dot(point - _this.origin, _this.direction.normalized);
    public static Vector2 LNormal(this Ray2D _this) => Vector2.Perpendicular(_this.direction).normalized;
    public static Vector2 RNormal(this Ray2D _this) => -LNormal(_this);
}

public static class Vector3Extension {
	public const int VECTOR3_SIZE = 3;
	public enum Axis {
		X = 0,
		Y = 1,
		Z = 2,
		Pitch = 0,
		Yaw = 1,
		Roll = 2,
	}

	public static Vector3 Set(this ref Vector3 vector, Vector3 newVector) {
		vector.x = newVector.x;
		vector.y = newVector.y;
		vector.z = newVector.z;
		return vector;
	}

	public static Vector3 SwapAxises(this Vector3 vector, Axis axis1 = Axis.X, Axis axis2 = Axis.Y) {
		var temp = vector[(int) axis1];
		vector[(int) axis1] = vector[(int) axis2];
		vector[(int) axis2] = temp;
		return vector;
	}

	public static Vector3 Invert(this Vector3 vector, params Axis[] axises) {
		foreach (var axis in axises) { vector[(int) axis] = -vector[(int) axis]; }
		return vector;
	}

	public static Vector3 FloatsToVector3(IReadOnlyList<float> array, int index) =>
			new Vector3(array[index * VECTOR3_SIZE + 0],
					array[index * VECTOR3_SIZE + 1],
					array[index * VECTOR3_SIZE + 2]);

    public static Vector3 Unscale(this Vector3 _this, Vector3 other) => _this.Unscale(other.x, other.y, other.z);

    public static Vector3 Unscale(this Vector3 _this, float x, float y, float z)
    {
        _this.Set(_this.x / x, _this.y / y, _this.z / z);
        return _this;
    }

    public static Vector3 ProjectionVector(this Vector3 _this, Ray line) => Vector3.Cross(line.direction, _this - line.origin);

    public static Vector3 Projection(this Vector3 _this, Vector3 a, Vector3 b) => a + Vector3.Project(_this - a, b - a);
    public static Vector3 Projection(this Vector3 _this, Ray line) => Vector3.Project(_this - line.origin, line.direction);
    public static float Project(this Ray _this, Vector3 point)
    {
        var projection = point.Projection(_this);
        var projectionOnRay = projection - _this.origin;
        var sign = Mathf.Sign(Vector3.Dot(projectionOnRay.normalized, _this.origin + _this.direction));
        return projectionOnRay.magnitude * sign;
    }
    public static Vector3 Project(this Vector3 _this, Vector3 a, Vector3 dir) => Projection(_this, a, a + dir);
    public static float Distance(this Vector3 _this, Ray line) => ProjectionVector(_this, line).magnitude;


    public static float Frac(float value) { return (float)(value - Math.Truncate(value)); }
    public static Vector4 Frac(this Vector4 _this) { _this.Set(Frac(_this.x), Frac(_this.y), Frac(_this.z), Frac(_this.w)); return _this; }
}