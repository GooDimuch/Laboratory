using UnityEngine;
using UnityEngine.UI;
using Utils;

public class MaskedScrollWithPin : ScrollWithPin
{
    private readonly Vector2 TopPivot = new Vector2(0.5f, 1f);
    private readonly Vector2 BottomPivot = new Vector2(0.5f, 0f);
    [field: SerializeField] public Vector2 MaskOffset { get; set; }
    [field: SerializeField] public Image TopMask { get; set; }
    [field: SerializeField] public Image BottomMask { get; set; }

    protected override void Init()
    {
        CalculateMaskSizes();
        base.Init();
    }

    private void CalculateMaskSizes()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(Scroll.GetComponent<RectTransform>());
        var topMaskTransform = TopMask.rectTransform;
        var bottomMaskTransform = BottomMask.rectTransform;
        topMaskTransform.pivot = BottomPivot;
        bottomMaskTransform.pivot = TopPivot;
        var topHeight = TopBehaviour.RectTransform.rect.height;
        var bottomHeight = BottomBehaviour.RectTransform.rect.height;
        topMaskTransform.sizeDelta = topMaskTransform.sizeDelta.WithY(-topHeight) + MaskOffset;
        bottomMaskTransform.sizeDelta = bottomMaskTransform.sizeDelta.WithY(topHeight) - MaskOffset;
        bottomMaskTransform.anchoredPosition += new Vector2(0, topHeight);
        bottomMaskTransform.MoveAnchorsToCorners();
        bottomMaskTransform.sizeDelta = bottomMaskTransform.sizeDelta.WithY(-bottomHeight) + MaskOffset;
    }

    protected override void HidePinned()
    {
        TopMask.enabled = false;
        BottomMask.enabled = false;
        base.HidePinned();
    }

    protected override void PinOnTop()
    {
        TopMask.enabled = true;
        BottomMask.enabled = false;
        base.PinOnTop();
    }

    protected override void PinOnBottom()
    {
        TopMask.enabled = false;
        BottomMask.enabled = true;
        base.PinOnBottom();
    }
}