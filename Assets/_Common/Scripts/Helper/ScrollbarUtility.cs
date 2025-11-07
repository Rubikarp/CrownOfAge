using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using Sirenix.OdinInspector;

public class ScrollbarUtility : MonoBehaviour
{
    public Scrollbar bar;

    public void MoveTo(float value)
    {
        transform.DOKill();

        float currentPos = bar.value;
        float distance = Mathf.Abs(currentPos - value);

        DOTween.To(() => bar.value, x => bar.value = x, value, distance).SetEase(Ease.InOutSine);
    }

    [Button] public void MoveToStart() => MoveTo(0f);
    [Button] public void MoveToEnd() => MoveTo(1f);
}
