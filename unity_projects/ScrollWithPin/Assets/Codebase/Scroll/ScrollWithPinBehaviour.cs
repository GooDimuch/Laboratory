using UnityEngine;

public abstract class ScrollWithPinBehaviour : MonoBehaviour
{
    public RectTransform RectTransform => (RectTransform) transform;
    public abstract void Show<T>(T pinnedElem) where T : ScrollWithPinBehaviour;
}