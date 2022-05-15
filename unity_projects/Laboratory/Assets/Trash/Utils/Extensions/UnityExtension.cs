using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UnityExtension {
	/// <summary> 
	/// Find all objects in the scene of type T with the given name, even if they are inactive. If name isn't specified, 
	/// any objects of type T will be returned instead. 
	/// </summary> 
	/// <typeparam name="T">the type of object to find</typeparam> 
	/// <param name="obj">extension method, for ease of use</param> 
	/// <param name="name">the name of the object to find</param> 
	/// <returns>the objects of type T in the scene</returns> 
	public static List<T> FindAll<T>(this Object obj, string name = null) where T : Object {
		var objects = Resources.FindObjectsOfTypeAll<T>()
				.Where(o => o.hideFlags != HideFlags.NotEditable &&
						o.hideFlags != HideFlags.HideAndDontSave &&
						(name == null || o.name == name));
		//#if UNITY_EDITOR
		//objects = objects.Where(o => UnityEditor.EditorUtility.IsPersistent(o)); 
		//#endif
		return objects.ToList();
	}

	/// <summary>
	/// Find the first GameObject with the given name, if it is loaded with scene.
	/// </summary>
	/// <param name="obj">extension method, for ease of use</param>
	/// <param name="name">the name of the object to find</param>
	/// <returns>GameObject with the given name</returns>
	public static GameObject FindFirst(this Object obj, string name = null) =>
			obj.FindAll<GameObject>(name).FirstOrDefault(o => o.scene.isLoaded);

	/// <summary>
	/// Find the first GameObject with the given name, if it is loaded with scene, and return component T.
	/// </summary>
	/// <typeparam name="T">the type of object to find</typeparam>
	/// <param name="obj">extension method, for ease of use</param>
	/// <param name="name">the name of the object to find</param>
	/// <returns>Component with given type <T></returns>
	public static T FindFirst<T>(this Object obj, string name = null) where T : Object {
		var gameObject = obj.FindFirst(name);
		return gameObject == null ? null : gameObject.GetComponent<T>();
	}

    /// <summary>
    /// Fills RenderTexture with specified color. Mainly used to clear the one.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static RenderTexture Clear(this RenderTexture obj, Color color)
    {
        var temp = RenderTexture.active;
        RenderTexture.active = obj;
        GL.Clear(true, true, color);
        RenderTexture.active = temp;
        return obj;
    }

    /// <summary>
    /// Fills Texture2D with specified color. Mainly used to clear the one. CPU intensive
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Texture2D Clear(this Texture2D obj, Color color)
    {
        obj.SetPixels((new Color[obj.width * obj.height]).Clear(color));
        obj.Apply();
        return obj;
    }

    public static Texture2D SetPixel(this Texture2D obj, float x, float y, Color color)
    {
        obj.SetPixel(Mathf.CeilToInt(x), Mathf.CeilToInt(y), color);
        obj.SetPixel(Mathf.FloorToInt(x), Mathf.CeilToInt(y), color);
        obj.SetPixel(Mathf.CeilToInt(x), Mathf.FloorToInt(y), color);
        obj.SetPixel(Mathf.FloorToInt(x), Mathf.FloorToInt(y), color);
        return obj;
    }

    public static bool TestPixelAlpha(this Texture2D obj, float x, float y, float threshold, bool less = false)
    {
        var sign = less ? -1 : 1;
        return sign * (obj.GetPixel(Mathf.CeilToInt(x), Mathf.CeilToInt(y)).a - threshold) >= 0
        || sign * (obj.GetPixel(Mathf.CeilToInt(x), Mathf.FloorToInt(y)).a - threshold) >= 0
        || sign * (obj.GetPixel(Mathf.FloorToInt(x), Mathf.CeilToInt(y)).a - threshold) >= 0
        || sign * (obj.GetPixel(Mathf.FloorToInt(x), Mathf.FloorToInt(y)).a - threshold) >= 0;
    }

    /// <summary>
    /// Sets GameObject`s layer recursively (as it would be done if "for all childs as well" option selected in Editor)
    /// </summary>
    /// <param name="_this"></param>
    /// <param name="layer"></param>
    public static void SetLayerRecursively(this GameObject _this, int layer)
    {
        _this.layer = layer;
        foreach (Transform child in _this.transform)
            SetLayerRecursively(child.gameObject, layer);
    }

    public static List<string> GetLayers(this Animator _this) {
        List<string> layers = new List<string>();
        if (_this)
        {
            for (int i = 0; i < _this.layerCount; i++)
                layers.Add(_this.GetLayerName(i));
        }
        return layers;
    }

    public static List<T> FindComponentsOfTypeWithLayer<T>(int layer) where T : Behaviour
    {
        var allComponents = GameObject.FindObjectsOfType<T>();
        var result = new List<T>();
        foreach (var c in allComponents)
            if (c.gameObject.layer == (c.gameObject.layer & layer))
                result.Add(c);
        return result;
    }

    public static Vector2 TimeRange(this AnimationCurve _this) => new Vector2(_this.keys[0].time, _this.keys[_this.keys.Length - 1].time);
}

public static class RectTransformExtensions {
	public static void SetLeft(this RectTransform rt, float left) {
		rt.offsetMin = new Vector2(left, rt.offsetMin.y);
	}

	public static void SetRight(this RectTransform rt, float right) {
		rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
	}

	public static void SetTop(this RectTransform rt, float top) {
		rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
	}

	public static void SetBottom(this RectTransform rt, float bottom) {
		rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
	}
}