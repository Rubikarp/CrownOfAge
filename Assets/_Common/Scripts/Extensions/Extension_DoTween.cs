using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public static class Extension_DoTween
{
    public static Tween DoSlide(this Scrollbar bar, float value, float duration) 
        => DOTween.To(() => bar.value, x => bar.value = x, value, duration);
    public static Tween DOFade(this TextMeshProUGUI tmp, float value, float duration) 
        => DOTween.To(() => tmp.alpha, x => tmp.alpha = x, value, duration);
    public static Tween DOFade(this RawImage rawImage, float endValue, float duration) 
        => DOVirtual.Float(rawImage.color.a, endValue, duration, (a) => rawImage.color.SetAlpha(a));
    public static Tween DOColor(this MaskableGraphic maskableGraphics, Color endValue, float duration)
        => DOTween.To(() => maskableGraphics.color, x => maskableGraphics.color = x, endValue, duration);
    public static Tween DOFade(this MaskableGraphic maskableGraphics, float endValue, float duration)
        => DOVirtual.Float(maskableGraphics.color.a, endValue, duration, (a) => maskableGraphics.color.SetAlpha(a));
}
