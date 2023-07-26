using TMPro;
using UnityEngine;

public class TestPinned : ScrollWithPinBehaviour
{
    [SerializeField] private TMP_Text text;
    public PinnedViewData Data;

    private void Awake()
    {
        Data ??= new PinnedViewData() { Text = text.text };
    }

    public void Show(PinnedViewData data)
    {
        Data = data;
        text.text = data.Text;
    }

    public void Show(TestPinned pinnedElem)
    {
        Data = (PinnedViewData) pinnedElem.Data.Clone();
        text.text = pinnedElem.Data.Text;
    }

    public override void Show<T>(T pinnedElem) => Show(pinnedElem as TestPinned);
}