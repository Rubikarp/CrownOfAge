using UnityEngine.EventSystems;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class ScrollViewController : UIBehaviour
{
    private RectTransform rect
    {
        get
        {
            if (_rect == null) _rect = GetComponent<RectTransform>();
            return _rect;
        }
    }
    private RectTransform _rect;

    [Header("References")]
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform viewport;

    [Header("Scroll Settings")]
    [SerializeField, ReadOnly] private Vector2 currentScrollPos;
    [DisableIf("useStep"), SerializeField, PropertyRange(0, 1)] private float scrollPositionX = 0f;
    [DisableIf("useStep"), SerializeField, PropertyRange(0, 1)] private float scrollPositionY = 0f;
    public Vector2 AimScrollPos
    {
        get => new Vector2(scrollPositionX, scrollPositionY);
        set
        {
            scrollPositionX = Mathf.Clamp01(value.x);
            scrollPositionY = Mathf.Clamp01(value.y);
            OnScrollPosChanged();
        }
    }
    public Vector2 RawScrollPos
    {
        get => currentScrollPos;
        set
        {
            if (currentMoveTween != null) currentMoveTween.Kill(false);

            currentScrollPos = value;
            RawMoveInsideViewport(currentScrollPos);
        }
    }
    [Header("Step Settings")]
    [SerializeField] public bool useStep = false;

    [ShowIf("useStep"), SerializeField, PropertyRange(0, "stepXQuantity")] private int stepX = 0;
    [ShowIf("useStep"), SerializeField, Min(1)] public int stepXQuantity = 2;
    [ShowIf("useStep"), SerializeField, PropertyRange(0, "stepYQuantity")] private int stepY = 0;
    [ShowIf("useStep"), SerializeField, Min(1)] public int stepYQuantity = 2;
    public Vector2 AimStepPos
    {
        get => new Vector2(stepX, stepY);
        set
        {
            stepX = Mathf.Clamp((int)value.x, 0, stepXQuantity - 1);
            stepY = Mathf.Clamp((int)value.y, 0, stepYQuantity - 1);
            OnScrollStepChanged();
        }
    }

    [Header("Scroll Movement")]
    [SerializeField] private bool instantMove = false;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private Ease moveEase = Ease.Linear;
    private Tween currentMoveTween = null;

    protected override void Awake()
    {
        base.Awake();

        // Ensure the component has the necessary hierarchy
        if (viewport == null) viewport = GetComponent<RectTransform>();
        if (content == null)
        {
            if (viewport.childCount > 0)
            {
                content = viewport.GetChild(0).GetComponent<RectTransform>();
            }
            else Debug.LogError("ScrollViewController requires a child RectTransform as the content.");
        }
    }
    protected void OnValidate()
    {
        if (useStep)
        {
            OnScrollStepChanged();
        }
        else
        {
            OnScrollPosChanged();
        }
    }

    public void RawFocusOnPosInContent(Vector2 position)
    {
        position = new Vector2(Mathf.Clamp01(position.x), Mathf.Clamp01(position.y));

        if (content == null || viewport == null) return;
        Vector2 contentSize = content.rect.size;
        Vector2 viewportSize = viewport.rect.size;

        float newX = -position.x * contentSize.x;
        float newY = (1f - position.y) * contentSize.y;

        // Counteract pivot at center
        content.anchoredPosition = new Vector2(viewportSize.x * .5f, -viewportSize.y * .5f);
        // Adjust to focus on the desired position
        content.anchoredPosition += new Vector2(newX, newY);
    }
    public void RawMoveInsideViewport(Vector2 position)
    {
        if (content == null || viewport == null) return;

        Vector2 contentSize = content.rect.size;
        Vector2 viewportSize = viewport.rect.size;
        Vector2 availableSpace = Vector2.Max(Vector2.zero, contentSize - viewportSize);

        float newX = -position.x * availableSpace.x;
        float newY = (1f - position.y) * availableSpace.y;

        content.anchoredPosition = new Vector2(newX, newY);
    }

    public void ScrollViewHorizontallyTo(float position)
    {
        scrollPositionX = Mathf.Clamp01(position);
        OnScrollPosChanged();
    }
    public void ScrollViewVerticallyTo(float position)
    {
        scrollPositionY = Mathf.Clamp01(position);
        OnScrollPosChanged();
    }

    public void MoveToNextHorizontalStep() => MoveToSpecificHorizontalStep(stepX + 1);
    public void MoveToPreviousHorizontalStep() => MoveToSpecificHorizontalStep(stepX - 1);
    public void MoveToSpecificHorizontalStep(int step)
    {
        stepX = Mathf.Clamp(step, 0, stepXQuantity);
        OnScrollStepChanged();
    }

    public void MoveToNextVerticalStep() => MoveToSpecificVerticalStep(stepY + 1);
    public void MoveToPreviousVerticalStep() => MoveToSpecificVerticalStep(stepY - 1);
    public void MoveToSpecificVerticalStep(int step)
    {
        stepY = Mathf.Clamp(step, 0, stepYQuantity);
        OnScrollStepChanged();
    }

    private void OnScrollPosChanged()
    {
        if (!instantMove && Application.isPlaying)
        {
            currentMoveTween?.Kill(false);
            currentMoveTween = DOTween.To(
                () => currentScrollPos,
                x => currentScrollPos = x,
                AimScrollPos,
                Vector2.Distance(currentScrollPos, AimScrollPos) * moveDuration)
                .SetEase(moveEase)
                .OnUpdate(() => RawMoveInsideViewport(currentScrollPos));
            currentMoveTween.Play();
        }
        else
        {
            currentScrollPos = AimScrollPos;
            RawMoveInsideViewport(currentScrollPos);
        }
    }
    private void OnScrollStepChanged()
    {
        stepX = Mathf.Clamp(stepX, 0, stepXQuantity);
        scrollPositionX = (float)(stepXQuantity - stepX) / stepXQuantity;

        stepY = Mathf.Clamp(stepY, 0, stepYQuantity);
        scrollPositionY = (float)(stepYQuantity - stepY) / stepYQuantity;

        if (!instantMove && Application.isPlaying)
        {
            currentMoveTween?.Kill(false);
            currentMoveTween = DOTween.To(
                () => currentScrollPos,
                x => currentScrollPos = x,
                AimScrollPos,
                Vector2.Distance(currentScrollPos, AimScrollPos) * moveDuration)
                .SetEase(moveEase)
                .OnUpdate(() => RawMoveInsideViewport(currentScrollPos));
            currentMoveTween.Play();
        }
        else
        {
            currentScrollPos = AimScrollPos;
            RawMoveInsideViewport(currentScrollPos);
        }
    }

}

