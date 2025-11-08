using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DragElement))]
public class SwipeCard : MonoBehaviour
{
    private DragElement movement;

    [Header("Components")]
    [SerializeField] private SwipeBoundary boundary;

    private void Start()
    {
        movement = GetComponent<DragElement>();
        movement.DragEvent.AddListener(OnDrag);
        movement.EndDragEvent.AddListener(OnDragRelease);
    }

    private void OnDrag(DragElement self, PointerEventData releaseEvent)
    {
        boundary.ReactToPos(movement.RectTransform);
    }

    private void OnDragRelease(DragElement self, PointerEventData releaseEvent)
    {
        boundary.ReactToRelease(movement.RectTransform);
    }
}
