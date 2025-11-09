using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class ScreenMovement : MonoBehaviour
{
    public RectTransform RectTransform => rectTransform;
    private RectTransform rectTransform;

    [field: SerializeField] public int ScreenPos { get; private set; } = 0;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        RectTransform.anchorMin = new Vector2(ScreenPos, 0f);
        RectTransform.anchorMax = new Vector2(ScreenPos + 1, 1f);
    }

    public Tween Move(int move, float duration = 0.5f)
    {
        Sequence result = DOTween.Sequence();
        var newScreenPos = ScreenPos + move;
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        if (newScreenPos < -1)
        {
            newScreenPos = 1;
            RectTransform.DOKill(true);
            RectTransform.anchorMin = new Vector2(newScreenPos, 0f);
            RectTransform.anchorMax = new Vector2(newScreenPos + 1, 1f);
            RectTransform.sizeDelta = Vector2.zero;

        }
        else if (newScreenPos > 1)
        {
            newScreenPos = -1;
            RectTransform.DOKill(true);
            RectTransform.anchorMin = new Vector2(newScreenPos, 0f);
            RectTransform.anchorMax = new Vector2(newScreenPos + 1, 1f);
            RectTransform.sizeDelta = Vector2.zero;
        }
        else
        {
            if (Application.isPlaying)
            {
                RectTransform.DOKill(true);
                result.Join(RectTransform.DOAnchorMin(new Vector2(newScreenPos, 0f), duration));
                result.Join(RectTransform.DOAnchorMax(new Vector2(newScreenPos + 1, 1f), duration));
            }
            else
            {
                RectTransform.DOKill(true);
                RectTransform.anchorMin = new Vector2(newScreenPos, 0f);
                RectTransform.anchorMax = new Vector2(newScreenPos + 1, 1f);
                RectTransform.sizeDelta = Vector2.zero;
            }
        }
        ScreenPos = newScreenPos;
        return result;
    }
}