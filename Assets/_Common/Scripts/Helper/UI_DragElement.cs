using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace WG.Common
{
    public class UI_DragElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Canvas canvas;
        private RectTransform rect;

        private void Start()
        {
            canvas = GetComponentInParent<Canvas>();
            rect = transform.GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData) { }
        public void OnEndDrag(PointerEventData eventData) { }

        public void OnDrag(PointerEventData eventData)
        {
            if (!Mouse.current.leftButton.isPressed) return;

            rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}
