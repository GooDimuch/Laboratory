using JetBrains.Annotations;
using MyBox;
using UnityEngine;
using Utils;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private TestPinned prefab;
    [SerializeField] private TestPinned pinnedElem;
    [SerializeField] private int count;
    [SerializeField] private ScrollWithPin pinner;
    [SerializeField, ReadOnly] private Vector3 pinnedLocalPos;
    [SerializeField, ReadOnly] private Vector3 pinnedPos;

    private void Update()
    {
        if (pinnedElem == null)
            return;
        pinnedPos = pinnedElem.transform.position;
        pinnedLocalPos = pinnedElem.transform.localPosition;
    }

    [ButtonMethod, UsedImplicitly]
    public void Spawn()
    {
        parent.DestroyChildren();
        for (int i = 0; i < count; i++)
        {
            SpawnItem(i);
        }
    }

    [ButtonMethod, UsedImplicitly]
    public void Pin() => pinner.SetPinned(pinnedElem);

    private void SpawnItem(int i)
    {
        var item = Instantiate(prefab, parent);
        var text = $"Item {i + 1}";
        item.Show(new PinnedViewData() { Text = text });
        item.name = text;
    }
}