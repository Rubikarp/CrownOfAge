using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class LifePoint : MonoBehaviour
{
    [Header("Setting")]
    public bool IsOn
    {
        get => _isOn;
        private set
        {
            bool change = _isOn != value;

            _isOn = value;

            if (change)
            {
                if (_isOn)
                {
                    var rect = imageLifePoint.transform as RectTransform;
                    rect.localScale = Vector3.one;
                    rect.gameObject.SetActive(true);
                }
                else
                {
                    LosePoint();
                }
            }
        }
    }
    [SerializeField] private bool _isOn = true;

    [Header("Components")]
    [SerializeField, Required] private Image imageLifePoint;
    [HideInInspector] public Image imageBackground;

    [Button] public void Toogle() => IsOn = !_isOn;
    public void SetState(bool state) => IsOn = state;
    private IEnumerator feedback;

    private void OnValidate()
    {
        IsOn = _isOn;
    }

    private void LosePoint()
    {
        var rect = imageLifePoint.transform as RectTransform;
        if (Application.isPlaying)
        {
            if (feedback != null) return;

            rect.localScale = Vector3.one;
            rect.gameObject.SetActive(true);

            feedback = ShakeFeedback(rect);
            StartCoroutine(feedback);
        }
        else
        {
            rect.localScale = Vector3.zero;
            rect.gameObject.SetActive(false);
        }
    }
    public IEnumerator ShakeFeedback(RectTransform rect)
    {
        imageBackground?.gameObject.SetActive(false);
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(rect.DOShakeRotation(1f, Vector3.forward * 30, 15, randomnessMode: ShakeRandomnessMode.Harmonic));
        mySequence.Join(rect.DOShakeAnchorPos(1f, Vector2.one * 2, 10));
        mySequence.AppendCallback(() => imageBackground?.gameObject.SetActive(true));
        mySequence.Join(rect.DOScale(Vector3.zero, .5f).SetEase(Ease.InOutSine));
        mySequence.Join(imageLifePoint.DOFade(0, .5f));

        yield return mySequence.WaitForCompletion();

        rect.gameObject.SetActive(false);
        feedback = null;
    }
}
