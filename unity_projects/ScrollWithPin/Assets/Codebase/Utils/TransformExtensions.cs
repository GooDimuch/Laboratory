using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace Utils
{
    public static class TransformExtensions
    {
        public static void DestroyChildren(this Transform self, float delay = 0f)
        {
            self.GetChildren()
                .GetGameObjects()
                .Destroy(delay);
        }

        public static IEnumerable<Transform> GetChildren(this Transform self)
        {
            List<Transform> result = new List<Transform>(self.childCount);

            for (int i = 0; i < self.childCount; i++)
            {
                result.Add(self.GetChild(i));
            }

            return result;
        }
    }

    public static class ComponentExtensions
    {
        public static IEnumerable<GameObject> GetGameObjects(this IEnumerable<Component> self)
        {
            var result = new List<GameObject>();
            result.AddRange(self.Select(component => component.gameObject));
            return result;
        }
    }
}