using UnityEngine;
using UnityEngine.UI;
using Utils;

public class ScrollWithPin : MonoBehaviour
{
    [field: SerializeField] public ScrollRect Scroll { get; set; }
    [field: SerializeField] public Canvas Canvas { get; set; }
    [field: SerializeField] public ScrollWithPinBehaviour TopBehaviour { get; set; }
    [field: SerializeField] public ScrollWithPinBehaviour BottomBehaviour { get; set; }
    [field: SerializeField] public ScrollWithPinBehaviour PinnedBehaviour { get; set; }
    [field: SerializeField] public bool Active { get; set; } = true;

    private void Start()
    {
        Init();
    }

    public void SetPinned(ScrollWithPinBehaviour behaviour)
    {
        PinnedBehaviour = behaviour;
        TopBehaviour.Show(behaviour);
        BottomBehaviour.Show(behaviour);
        UpdatePinned();
    }

    public void ActivateBottomWithForce() => PinOnBottom();

    public void ActivateTopWithForce() => PinOnTop();

    protected virtual void Init()
    {
        Scroll.onValueChanged.AddListener(OnScroll);
        HidePinned();
        if (PinnedBehaviour != null)
        {
            TopBehaviour.Show(PinnedBehaviour);
            BottomBehaviour.Show(PinnedBehaviour);
        }

        UpdatePinned();
    }

    protected virtual void PinOnTop()
    {
        BottomBehaviour.RectTransform.gameObject.SetActive(false);
        TopBehaviour.RectTransform.gameObject.SetActive(true);
    }

    protected virtual void PinOnBottom()
    {
        BottomBehaviour.RectTransform.gameObject.SetActive(true);
        TopBehaviour.RectTransform.gameObject.SetActive(false);
    }

    protected virtual void HidePinned()
    {
        TopBehaviour.RectTransform.gameObject.SetActive(false);
        BottomBehaviour.RectTransform.gameObject.SetActive(false);
    }

    protected virtual bool CheckIfUpdateNeeded() => PinnedBehaviour == null || !Active;

    private void OnScroll(Vector2 delta) => UpdatePinned();

    private void UpdatePinned()
    {
        if (CheckIfUpdateNeeded()) return;
        if (PinnedBehaviour.RectTransform.IsFullyVisibleFrom(Scroll.viewport,
                Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Canvas.worldCamera))
        {
            HidePinned();
            return;
        }

        if (PinnedBehaviour.RectTransform.IsBelowScroll(Scroll.viewport))
            PinOnBottom();
        else
            PinOnTop();
    }
}