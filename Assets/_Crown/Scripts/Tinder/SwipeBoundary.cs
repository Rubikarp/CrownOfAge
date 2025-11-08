using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SwipeBoundary : MonoBehaviour
{
    public RectTransform RectTransform => rectTransform;
    private RectTransform rectTransform;

    [Header("Components")]
    public Image rejectBorder;
    public Image acceptBorder;

    [Header("Settings")]
    [SerializeField] private float visibilityDistanceMultiplier = 1.5f;
    [SerializeField] private AnimationCurve alphaOverDetection = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    public float DetectionDistance => rectTransform.rect.width * BorderDetectionRatio;
    public float BorderDetectionRatio
    {
        get => borderDetectionPercent * 0.01f;
        set => borderDetectionPercent = Mathf.Clamp(value, 0f, 100f);
    }
    [SerializeField, Range(0f, 100f)] private float borderDetectionPercent = 33f;

    [Header("Info")]
    [SerializeField, ReadOnly] private float distFromLeftBorder;
    [SerializeField, ReadOnly] private float distFromRightBorder;
    [SerializeField, ReadOnly] private Vector3[] elementCorners = new Vector3[4];
    [SerializeField, ReadOnly] private Vector3[] boundaryCorners = new Vector3[4];

    [Header("Events")]
    public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        elementCorners = new Vector3[4];
        boundaryCorners = new Vector3[4];
        rectTransform.GetLocalCorners(boundaryCorners);

        rejectBorder.SetAlpha(0f);
        acceptBorder.SetAlpha(0f);
    }

    public void ReactToPos(RectTransform element)
    {
        element.GetLocalCorners(elementCorners);
        distFromLeftBorder = (elementCorners[0].x + element.position.x) - (boundaryCorners[0].x + rectTransform.position.x);
        distFromRightBorder = (boundaryCorners[2].x + rectTransform.position.x) - (elementCorners[2].x + element.position.x);

        float rawLeftAlpha = Mathf.InverseLerp(DetectionDistance * visibilityDistanceMultiplier, 0f, distFromLeftBorder);
        rejectBorder.SetAlpha(alphaOverDetection.Evaluate(rawLeftAlpha));
        float rawRightAlpha = Mathf.InverseLerp(DetectionDistance * visibilityDistanceMultiplier, 0f, distFromRightBorder);
        acceptBorder.SetAlpha(alphaOverDetection.Evaluate(rawRightAlpha));
    }

    public void ReactToRelease(RectTransform element)
    {
        element.GetLocalCorners(elementCorners);
        distFromLeftBorder = (elementCorners[0].x + element.position.x) - (boundaryCorners[0].x + rectTransform.position.x);
        distFromRightBorder = (boundaryCorners[2].x + rectTransform.position.x) - (elementCorners[2].x + element.position.x);

        if (distFromLeftBorder < DetectionDistance)
            OnSwipeLeft.Invoke();
        else if (distFromRightBorder < DetectionDistance)
            OnSwipeRight.Invoke();
    }
}
