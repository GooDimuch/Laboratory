/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System;
using UnityEngine;

namespace MeshToTerrainConvertor.Scripts
{
    public class MeshToTerrainBoundsHelper : MonoBehaviour
    {
        public Action OnBoundChanged;
        public Action OnDestroyed;
        public Bounds bounds;
    }
}