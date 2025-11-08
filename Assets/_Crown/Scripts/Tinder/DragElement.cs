using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using Sirenix.OdinInspector;

public class DragElementEvent : UnityEvent<DragElement, PointerEventData> { }

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class DragElement : MonoBehaviour,
    IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    public CanvasGroup CanvasGroup => canvasGroup;
    private CanvasGroup canvasGroup;
    public RectTransform RectTransform => rectTransform;
    private RectTransform rectTransform;
    public bool IsInteractable => CanvasGroup.interactable;

    [field: Header("Info")]
    [field: SerializeField, ReadOnly] public bool IsHovering { get; private set; }
    [field: SerializeField, ReadOnly] public bool isPressed { get; private set; }
    [field: SerializeField, ReadOnly] public bool isDragging { get; private set; }
    [SerializeField, ReadOnly] private Vector3 dragOffset;
    [SerializeField, ReadOnly] private Vector3 targetPosition;

    [Header("Events")]
    public DragElementEvent EnterHoverEvent = new DragElementEvent();
    public DragElementEvent ExitHoverEvent = new DragElementEvent();
    [Space]
    public DragElementEvent MouseDownEvent = new DragElementEvent();
    public DragElementEvent MouseUpEvent = new DragElementEvent();
    [Space]
    public DragElementEvent BeginDragEvent = new DragElementEvent();
    public DragElementEvent DragEvent = new DragElementEvent();
    public DragElementEvent EndDragEvent = new DragElementEvent();

    protected void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsInteractable) return;

        isDragging = true;
        BeginDragEvent.Invoke(this, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsInteractable) return;

        // Convert pointer position to local point in parent rect
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPointerPos
        );
        targetPosition = (Vector3)localPointerPos + dragOffset;
        rectTransform.localPosition = targetPosition;

        DragEvent.Invoke(this, eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsInteractable) return;

        isDragging = false;
        EndDragEvent.Invoke(this, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsInteractable) return;

        IsHovering = true;
        EnterHoverEvent.Invoke(this, eventData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsInteractable) return;

        IsHovering = false;
        ExitHoverEvent.Invoke(this, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsInteractable) return;

        // Convert pointer position to local point in parent rect
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPointerPos
        );
        dragOffset = rectTransform.localPosition - (Vector3)localPointerPos;

        isPressed = true;
        MouseDownEvent.Invoke(this, eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsInteractable) return;

        isPressed = false;
        MouseUpEvent.Invoke(this, eventData);
    }
}
