using System.Collections.Generic;
using MyBox;
using UnityEngine;

namespace Utils
{
    public static class ObjectExtensions
    {
        public static void Destroy(this Object self, float delay = 0f) => Object.Destroy(self, delay);

        public static void Destroy(this IEnumerable<Object> self, float delay = 0f) =>
            self.ForEach(obj => Destroy((Object) obj, delay));
    }
}