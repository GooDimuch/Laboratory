using System;
using System.Collections;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBySteps : MonoBehaviour
{
    #region Fields

    [SerializeField] private float speed = 2000f;
    [SerializeField] private RectTransform rect;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField, ReadOnly] private int numOfSteps;
    [SerializeField, ReadOnly] private int currentStep;
    private Coroutine _moveRoutine;

    #endregion

    #region Properties

    #region Private

    private float DeltaXForStep => rect.rect.width / numOfSteps;
    private float MinSizeX => (rect.offsetMin - rect.offsetMax).x;

    #endregion

    #region Public

    public int CurrentStep => Mathf.RoundToInt(Mathf.Abs(rect.anchoredPosition.x / DeltaXForStep));
    public int NumOfSteps => rect.childCount;

    #endregion

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        rect ??= GetComponent<RectTransform>();
    }

    private void Update()
    {
        numOfSteps = NumOfSteps;
        currentStep = CurrentStep;
    }

    #endregion

    #region Public Methods

    public void SetStep(int step, Action onMoveEnd = null, bool immediately = false)
    {
        Canvas.ForceUpdateCanvases();
        numOfSteps = rect.childCount;
        if (numOfSteps <= 1)
        {
            return;
        }

        currentStep = CurrentStep;
        if (step == currentStep)
        {
            return;
        }

        if (!StepOk(step)) return;

        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
        }

        var finalPos = new Vector2(-DeltaXForStep * step, rect.anchoredPosition.y);
        if (finalPos.x < MinSizeX) finalPos.x = MinSizeX;
        if (immediately)
        {
            rect.anchoredPosition = finalPos;
        }
        else
        {
            _moveRoutine = StartCoroutine(SmoothMove(finalPos, onMoveEnd));
        }
        
        Canvas.ForceUpdateCanvases();
    }

    #endregion

    #region Private Methods
    
    private bool StepOk(int step) => step >= 0 && step < NumOfSteps;

    private IEnumerator SmoothMove(Vector2 finalPos, Action onMoveEnd = null)
    {
        var startState = scrollRect.horizontal;
        scrollRect.horizontal = false;
        while (Math.Abs(rect.anchoredPosition.x - finalPos.x) > speed * Time.deltaTime)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, finalPos, speed * Time.deltaTime);
            yield return null;
        }

        onMoveEnd?.Invoke();

        rect.anchoredPosition = finalPos;
        scrollRect.horizontal = startState;
    }

    #endregion
}