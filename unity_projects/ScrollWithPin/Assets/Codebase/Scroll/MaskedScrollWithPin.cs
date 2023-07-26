using UnityEngine;
using Utils;

public class MaskedScrollWithPin : ScrollWithPin
{
    private readonly Vector2 TopPivot = new Vector2(0.5f, 1f);
    private readonly Vector2 CenterPivot = new Vector2(0.5f, 0.5f);
    private readonly Vector2 BottomPivot = new Vector2(0.5f, 0f);
    [field: SerializeField] public Vector2 MaskOffset { get; set; }
    [field: SerializeField] public RectTransform MaskTransform { get; set; }
    private bool _ignoreChange;

    protected override void HidePinned()
    {
        var newSizeDelta = Vector2.zero;
        if (MaskTransform.sizeDelta == newSizeDelta) return;
        if (MaskTransform.pivot == BottomPivot)
        {
            Scroll.content.anchoredPosition -= new Vector2(0, PinnedBehaviour.RectTransform.rect.height);
        }

        MaskTransform.sizeDelta = newSizeDelta;
        MaskTransform.pivot = CenterPivot;
        base.HidePinned();
    }

    protected override void PinOnTop()
    {
        var newSizeDelta = MaskTransform.sizeDelta.WithY(-TopBehaviour.RectTransform.rect.height) + MaskOffset;
        if (MaskTransform.sizeDelta == newSizeDelta) return;
        Scroll.content.anchoredPosition += new Vector2(0, PinnedBehaviour.RectTransform.rect.height);
        MaskTransform.sizeDelta = newSizeDelta;
        MaskTransform.pivot = BottomPivot;
        base.PinOnTop();
    }

    protected override void PinOnBottom()
    {
        var newSizeDelta = MaskTransform.sizeDelta.WithY(-BottomBehaviour.RectTransform.rect.height) + MaskOffset;
        if (MaskTransform.sizeDelta == newSizeDelta) return;
        MaskTransform.sizeDelta = newSizeDelta;
        MaskTransform.pivot = TopPivot;
        base.PinOnBottom();
    }
}