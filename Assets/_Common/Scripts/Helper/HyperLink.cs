using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

namespace SevenLies.core
{
    public class HyperLink : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public string url = "https://youtu.be/dQw4w9WgXcQ";

        [Button]
        public void OpenLink()
        {
            Application.OpenURL(url);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //OpenLink();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OpenLink();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}
