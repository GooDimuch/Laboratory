using UnityEngine;

namespace Utils
{
    public static class VectorExtensions
    {
        public static Vector2 WithX(this Vector2 vector2, float x) => new Vector2(x, vector2.y);

        public static Vector2 WithY(this Vector2 vector2, float y) => new Vector2(vector2.x, y);
    }
}