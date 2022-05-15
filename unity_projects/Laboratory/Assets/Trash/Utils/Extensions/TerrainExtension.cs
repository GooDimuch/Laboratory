using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TerrainExtension
{
    public enum PlacementType { Object, Tree, Detail }

    public const float INACCURACY_MAX_PX = 1f;
    public const float SPREAD_MAX_PX = 1f;

    public const string DETAIL_NONE = "None";
    public const string DETAIL_UNKNOWN = "Unknown";

    public static string DetailName(DetailPrototype prototype) => prototype?.prototype?.name ?? prototype?.prototypeTexture.name ?? DETAIL_UNKNOWN;
    public static string DetailName(TreePrototype prototype) => prototype?.prefab?.name ?? DETAIL_UNKNOWN;

    public static string[] AllDetails()
    {
        var result = new List<string>() { DETAIL_NONE };
        foreach (var terrain in Terrain.activeTerrains)
            foreach (var proto in terrain.terrainData.detailPrototypes)
            {
                var name = DetailName(proto);
                if (!result.Contains(name))
                    result.Add(name);

            }
        return result.ToArray();
    }

    public static string[] AllTrees()
    {
        var result = new List<string>() { DETAIL_NONE };
        foreach (var terrain in Terrain.activeTerrains)
            foreach (var proto in terrain.terrainData.treePrototypes)
            {
                var name = DetailName(proto);
                if (!result.Contains(name))
                    result.Add(name);

            }
        return result.ToArray();
    }

    private static Dictionary<Vector2Int, float> pixelsCache = new Dictionary<Vector2Int, float>();
    private static int[,] pixelDataCache = new int[,] { { 0 } };

    private static Vector2 detailToWorld(Vector3 startPos, Vector2 detailCoords, Vector2 pixelSize, float pixelOffset = 0f) 
        => new Vector2(startPos.x + (detailCoords.x + pixelOffset) * pixelSize.x, startPos.z + (detailCoords.y + pixelOffset) * pixelSize.y);

    private static bool isInside(Vector2 point, Vector2 center, Vector2 halfRight, Vector2 halfUp)
    {
        var d = point - center;
        var x = Vector2.Dot(d, halfRight) / halfRight.magnitude;
        var y = Vector2.Dot(d, halfUp) / halfUp.magnitude;
        return Mathf.Abs(x) <= 1f && Mathf.Abs(y) <= 1f;
    }

    public static Vector2 DetailPixelSize(this Terrain _this, int detailWidth = -1, int detailHeight = -1)
    {
        detailWidth = detailWidth > 0 ? detailWidth : _this.terrainData.detailWidth;
        detailHeight = detailHeight > 0 ? detailHeight : _this.terrainData.detailHeight;
        return new Vector2(_this.terrainData.size.x / detailWidth, _this.terrainData.size.z / detailHeight);
    }

    public static Vector3 ToDetailPoint(this Terrain _this, Vector3 worldPoint, int detailWidth = -1, int detailHeight = -1)
    {
        detailWidth = detailWidth > 0 ? detailWidth : _this.terrainData.detailWidth;
        detailHeight = detailHeight > 0 ? detailHeight : _this.terrainData.detailHeight;
        var localPoint = _this.transform.InverseTransformPoint(worldPoint);
        var detailPoint = localPoint.Unscale(_this.terrainData.size);
        detailPoint.Scale(new Vector3(detailWidth, 0f, detailHeight));
        return new Vector3(detailPoint.x, detailPoint.z, localPoint.y - _this.SampleHeight(worldPoint));
    }

    public static Vector2 ToWorldPoint2D(this Terrain _this, Vector2 detailCoords, float pixelOffset = 0f, int detailWidth = -1, int detailHeight = -1) 
        => detailToWorld(_this.transform.position, detailCoords, _this.DetailPixelSize(detailWidth, detailHeight), pixelOffset);

    public static Vector3 ToWorldPoint3D(this Terrain _this, Vector2 detailCoords, float pixelOffset = 0f, int detailWidth = -1, int detailHeight = -1)
    {
        var point2D = ToWorldPoint2D(_this, detailCoords, pixelOffset, detailWidth, detailWidth);
        var result = new Vector3(point2D.x, 0f, point2D.y);
        result.y = _this.transform.position.y + _this.SampleHeight(result);
        return result;
    }

    public static Rect ToWorldArea(this Terrain _this, RectInt detailArea, int detailWidth = -1, int detailHeight = -1)
    {
        var pixelSize = _this.DetailPixelSize(detailWidth, detailHeight);
        return new Rect(detailToWorld(_this.transform.position, new Vector2(detailArea.x, detailArea.y), pixelSize, 0), detailArea.size * pixelSize);
    }

    public static List<int> GetTreesInArea(this Terrain _this, Rect area, out TreeInstance[] allTrees, List<int> prototypeFilter = null)
    {
        List<int> result = new List<int>();
        allTrees = _this.terrainData.treeInstances;
        for (int i = 0; i < allTrees.Length; i++)
        {
            var tree = allTrees[i];
            var treePos = tree.position;
            treePos.Scale(_this.terrainData.size);
            treePos = _this.transform.TransformPoint(treePos);
            if ((prototypeFilter == null || prototypeFilter.Contains(tree.prototypeIndex)) && area.Contains(new Vector2(treePos.x, treePos.z)))
                result.Add(i);
        }
        return result;
    }

    public static List<int> GetTreesInPixel(this Terrain _this, Vector2Int pixel, ref TreeInstance[] allTrees, ref List<int> treeInstanceFilter, int detailWidth = -1, int detailHeight = -1)
    {
        detailWidth = detailWidth > 0 ? detailWidth : _this.terrainData.detailWidth;
        detailHeight = detailHeight > 0 ? detailHeight : _this.terrainData.detailHeight;
        List<int> result = new List<int>();
        allTrees = allTrees ?? _this.terrainData.treeInstances;
        treeInstanceFilter = treeInstanceFilter ?? Enumerable.Range(0, allTrees.Length - 1).ToList();
        foreach (var idx in treeInstanceFilter)
        {
            var tree = allTrees[idx];
            var pixelSize = _this.DetailPixelSize(detailWidth, detailHeight);
            var pixelCenter = detailToWorld(_this.transform.position, new Vector2(pixel.x, pixel.y), pixelSize, 0.5f);
            var treePos = tree.position;
            treePos.Scale(_this.terrainData.size);
            treePos = _this.transform.TransformPoint(treePos);
            var treeCenter = new Vector2(treePos.x, treePos.z);

            var halfRight = new Vector2(_this.transform.right.x, _this.transform.right.z) * pixelSize.x * 0.5f;
            var halfUp = new Vector2(_this.transform.forward.x, _this.transform.forward.z) * pixelSize.y * 0.5f;
            var inside = isInside(treeCenter, pixelCenter, halfRight, halfUp);

            //var d = treeCenter - pixelCenter;
            //var inside = Mathf.Abs(2 * d.x) <= pixelSize.x  && Mathf.Abs(2 * d.y) <= pixelSize.y;

            if (inside)
                result.Add(idx);
        }
        return result;
    }

    public static List<int> GetTreesInPixel(this Terrain _this, Vector2Int detailPixel, ref TreeInstance[] allTrees, int detailWidth = -1, int detailHeight = -1)
    {
        List<int> filter = null;
        return GetTreesInPixel(_this, detailPixel, ref allTrees, ref filter, detailWidth, detailHeight);
    }

    public static List<int> GetTreesInPixel(this Terrain _this, ref Vector2Int detailPixel, int detailWidth = -1, int detailHeight = -1)
    {
        TreeInstance[] allTrees = null;
        return GetTreesInPixel(_this, detailPixel, ref allTrees, detailWidth, detailHeight);
    }

    public static float NormalizeY(this Terrain _this, float worldY) => (worldY - _this.transform.position.y) / _this.terrainData.size.y;

    public static Vector3 SnapToSurface(this Terrain _this, Vector3 worldPos, float distance = 0f)
    {
        var tCoords = _this.ToDetailPoint(worldPos, 1, 1);
        var tNorm = _this.transform.TransformVector(_this.terrainData.GetInterpolatedNormal(tCoords.x, tCoords.y)).normalized;
        var dPos = (distance - tCoords.z) * tNorm;
        return worldPos + dPos;
    }

    public static void SnapToSurface(this Terrain _this, Transform target, out Vector3 position, out Quaternion rotation, float distance = 0f)
    {
        var tCoords = _this.ToDetailPoint(target.position, 1, 1);
        var tNorm = _this.transform.TransformVector(_this.terrainData.GetInterpolatedNormal(tCoords.x, tCoords.y)).normalized;
        var dPos = (distance - tCoords.z) * tNorm;
        var right = target.right;
        var forward = target.forward;
        forward = Vector3.Cross(right, tNorm).normalized;
        //right = Vector3.Cross(-forward.normalized, tNorm).normalized;
        //forward = Vector3.Cross(right, tNorm).normalized;
        position = target.position + dPos;
        rotation = Quaternion.LookRotation(forward, tNorm);
    }

    public static Transform SnapToSurface(this Terrain _this, Transform target, float distance = 0f) {
        SnapToSurface(_this, target, out Vector3 pos, out Quaternion rot, distance);
        target.position = pos;
        target.rotation = rot;
        return target;
    }

    public static bool PaintDetail(Terrain terrain, int layer, Vector2Int pixel, int density)
    {
        var detail = terrain.terrainData.GetDetailLayer(pixel.x, pixel.y, 1, 1, layer);
        if (detail[0, 0] != density)
        {
            pixelDataCache[0, 0] = density;
            terrain.terrainData.SetDetailLayer(pixel.x, pixel.y, layer, pixelDataCache);
            return true;
        }
        return false;
    }

    public static void PaintDetail(this Terrain terrain, Vector3 start, Vector3 end, int layer, int density)
    {
        if (layer >= 0)
        {
            var area = LineBrush3D(terrain.ToDetailPoint(start), terrain.ToDetailPoint(end), ref pixelsCache);
            var valid = area.x > 0 && area.y > 0 && area.x < terrain.terrainData.detailWidth && area.y < terrain.terrainData.detailHeight;
            if (valid)
            {
                foreach (var pixelData in pixelsCache)
                    PaintDetail(terrain, layer, pixelData.Key, density);
            }
        }
    }

    public static void Place(this Terrain terrain, PlacementType type, GameObject prefab, Transform parent, bool group, Texture2D map, Texture2D mask, float alphaThreshold,
                                                    float accuracy, float spread, int density, Vector2 width, Vector2 height, Vector2 rotation, bool snapToHeightmap)
    {
        switch (type)
        {
            case PlacementType.Object:
                var txWidth = TxWidth(map, mask);
                var txHeight = TxWidth(map, mask);
                var newObjects = setupInstances(terrain, prefab, map, mask, alphaThreshold, accuracy, spread, density, width, height, rotation, snapToHeightmap);
                var pGo = parent ? parent : terrain.transform;
                if (group)
                    pGo = GameObject.Instantiate(new GameObject(prefab.name), pGo).transform;
                if (newObjects.Count > 0)
                {
                    foreach (var instance in newObjects)
                    {
                        var wPos = terrain.ToWorldPoint3D(new Vector2((instance.position.x - 1) * txWidth, (instance.position.z - 1) * txHeight), txWidth, txHeight); //Terrain.transform.InverseTransformPoint(instance.position);
                        var qRot = Quaternion.AngleAxis(instance.rotation * Mathf.Rad2Deg, terrain.transform.up);
#if UNITY_EDITOR
                        var tree = UnityEditor.PrefabUtility.InstantiatePrefab(prefab, pGo.transform) as GameObject;
                        tree.transform.SetPositionAndRotation(wPos, qRot);
#else
                        var tree = GameObject.Instantiate(prefab, wPos, qRot, pGo);
#endif
                    }
                }
                break;
            case PlacementType.Tree:
                List<TreePrototype> treePrototypes = new List<TreePrototype>(terrain.terrainData.treePrototypes);
                int treeProtoIdx = treePrototypes.FindIndex(p => p.prefab == prefab);
                if (treeProtoIdx < 0)
                {
                    treePrototypes.Add(new TreePrototype() { prefab = prefab });
                    terrain.terrainData.treePrototypes = treePrototypes.ToArray();
                    treePrototypes = new List<TreePrototype>(terrain.terrainData.treePrototypes);
                    treeProtoIdx = treePrototypes.FindIndex(p => p.prefab == prefab);
                }

                var newTrees = setupInstances(terrain, prefab, map, mask, alphaThreshold, accuracy, spread, density, width, height, rotation, snapToHeightmap, treeProtoIdx);
                if (newTrees.Count > 0)
                {
                    List<TreeInstance> allTrees = new List<TreeInstance>(terrain.terrainData.treeInstances);
                    allTrees.AddRange(newTrees);
                    terrain.terrainData.SetTreeInstances(allTrees.ToArray(), snapToHeightmap);
                }
                break;
            case PlacementType.Detail:
                break;
        }
    }

    public static void Clear(this Terrain terrain, PlacementType type, GameObject prefab, Texture2D map, Texture2D mask, float alphaThreshold,
                            bool byMask = true, bool byMap = false, bool byPrefab = true)
    {
        switch (type)
        {
            case PlacementType.Object:
                break;
            case PlacementType.Tree:
                int treeProtoIdx = prefab ? terrain.terrainData.treePrototypes.FindIndex(p => p.prefab == prefab) : -1;
                var allTrees = terrain.terrainData.treeInstances;
                HashSet<int> toClear = new HashSet<int>();
                for (int i = 0; i < allTrees.Length; i++)
                {
                    var tree = allTrees[i];
                    bool maskTest = true, mapTest = true;

                    if (byMask)
                    {
                        var maskPx = mask ? new Vector2(tree.position.x * mask.width, tree.position.z * mask.height) : Vector2.zero;
                        maskTest = mask && mask.TestPixelAlpha(maskPx.x, maskPx.y, alphaThreshold, true);
                    }

                    if (byMap)
                    {
                        var mapPx = map ? new Vector2(tree.position.x * map.width, tree.position.z * map.height) : Vector2.zero;
                        mapTest = map && map.TestPixelAlpha(mapPx.x, mapPx.y, alphaThreshold);
                    }

                    var prefabTest = byPrefab ? prefab && treeProtoIdx == tree.prototypeIndex : !prefab || treeProtoIdx == tree.prototypeIndex;


                    if (maskTest && mapTest && prefabTest)
                        toClear.Add(i);
                }
                TreeInstance[] updated = allTrees.Where((t, idx) => !toClear.Contains(idx)).ToArray();
                terrain.terrainData.SetTreeInstances(updated, false);
                break;
            case PlacementType.Detail:
                break;

        }
    }

    private static int TxWidth(Texture2D map, Texture2D mask) => map?.width ?? mask?.width ?? 0;
    private static int TxHeight(Texture2D map, Texture2D mask) => map?.height ?? mask?.height ?? 0;

    private static List<TreeInstance> setupInstances(Terrain terrain, GameObject prefab, Texture2D map, Texture2D mask, float alphaThreshold, float accuracy,
                                                float spread, int density, Vector2 width, Vector2 height, Vector2 rotation, bool snapToHeightmap, int treeProtoIdx = -1)
    {
        List<TreeInstance> allTrees = new List<TreeInstance>();

        Vector2 maskScale = map ? mask ? new Vector2((float)mask.width / map.width, (float)mask.height / map.height) : Vector2.one : Vector2.one;
        Vector2 pxSize = Vector2.one.Unscale(terrain.terrainData.size.x, terrain.terrainData.size.z);

        var txWidth = TxWidth(map, mask);
        var txHeight = TxWidth(map, mask);

        for (int y = 0; y < txHeight; y++)
        {
            for (int x = 0; x < txWidth; x++)
            {
                //Color data = map.GetPixel(x, y);

                var maskTest = !mask || mask.GetPixel(Mathf.RoundToInt(x * maskScale.x), Mathf.RoundToInt(y * maskScale.y)).a <= alphaThreshold; //mask.TestPixelAlpha(x * maskScale.x, y * maskScale.y, alphaThreshold, true);
                var mapTest = !map || map.GetPixel(x, y).a >= alphaThreshold; //map.TestPixelAlpha(x, y, alphaThreshold);

                if (mapTest && maskTest)
                {
                    //Vector3 pos = Terrain.ToWorldPoint3D(new Vector2(x, y), 0, mask.width, mask.height);

                    Vector2 inaccuracy = (1f - accuracy) * Random.insideUnitCircle * pxSize * 0.5f * INACCURACY_MAX_PX;
                    Vector3 origin = new Vector3((pxSize.x * 0.5f) + ((float)x / txWidth), 0, (pxSize.y * 0.5f) + ((float)y / txHeight));

                    for (int i = 0; i < density; i++)
                    {
                        Vector2 pxOffset = inaccuracy + (density > 1 ? spread * Random.insideUnitCircle * pxSize * 0.5f * SPREAD_MAX_PX : Vector2.zero);
                        Vector3 pos = origin + new Vector3(pxOffset.x, 0, pxOffset.y);
                        if (snapToHeightmap)
                            pos.y = terrain.terrainData.GetInterpolatedHeight(pos.x, pos.y);

                        Color col = Color.white;
                        Vector2 scale = new Vector2(Random.Range(width.x, width.y), Random.Range(height.x, height.y));
                        float rot = Random.Range(rotation.x * Mathf.Deg2Rad, rotation.y * Mathf.Deg2Rad);

                        TreeInstance tree = new TreeInstance()
                        {
                            prototypeIndex = treeProtoIdx,
                            position = pos,
                            color = col,
                            heightScale = scale.y,
                            widthScale = scale.x,
                            rotation = rot,
                            lightmapColor = Color.white
                        };
                        //Terrain.AddTreeInstance(tree);
                        allTrees.Add(tree);
                    }
                }
            }
        }
        return allTrees;
    }


    private static RectInt LineBrush3D(Vector3 start, Vector3 end, ref Dictionary<Vector2Int, float> pixels)
    {
        pixels = pixels ?? new Dictionary<Vector2Int, float>();
        pixels.Clear();

        float frac = 1 / Mathf.Sqrt(Mathf.Pow(end.x - start.x, 2) + Mathf.Pow(end.y - start.y, 2));
        var point = start;
        Vector2Int pixel, min = Vector2Int.one * int.MaxValue, max = -min;
        var ctr = 0f;

        while ((int)point.x != (int)end.x || (int)point.y != (int)end.y)
        {
            point = Vector3.Lerp(start, end, ctr);
            pixel = Vector2Int.RoundToInt(point);
            pixels[pixel] = point.z;
            min = Vector2Int.Min(min, pixel);
            max = Vector2Int.Max(max, pixel);
            ctr += frac;
        }
        return new RectInt(min, max - min);
    }

}
