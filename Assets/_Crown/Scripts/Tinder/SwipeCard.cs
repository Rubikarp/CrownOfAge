using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(DragElement))]
public class SwipeCard : MonoBehaviour
{
    public RectTransform RectTransform => rectTransform;
    private RectTransform rectTransform;

    [Header("Components")]
    public DragElementFollow movementvisual;
    [HideInInspector] public DragElement movement;
    [HideInInspector] public SwipeBoundary boundary;

    [Header("Info")]
    [SerializeField, ReadOnly] private float distFromCenter;

    private void Start()
    {
        movement = GetComponent<DragElement>();

        movement.DragEvent.AddListener(OnDrag);
        movement.EndDragEvent.AddListener(OnDragRelease);
    }


    private void OnDrag(DragElement self, PointerEventData releaseEvent)
    {
        boundary.ReactToPos(this);
    }

    private void OnDragRelease(DragElement self, PointerEventData releaseEvent)
    {
        boundary.ReactToRelease(this);
    }
}
