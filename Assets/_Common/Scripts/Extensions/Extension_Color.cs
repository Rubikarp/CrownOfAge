using System;
using UnityEngine;
using UnityEngine.UI;

public static class Extension_Color
{
    public static Color WithAlpha(this Color color, float alpha) => color = new(color.r, color.g, color.b, alpha);
    public static void SetAlpha(this Image image, float alpha) => image.color = image.color.WithAlpha(alpha);

    public static string ToHex(this Color color) => $"#{ColorUtility.ToHtmlStringRGBA(color)}";
    public static Color FromHex(this string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        throw new ArgumentException("Invalid hex string", nameof(hex));
    }
}
