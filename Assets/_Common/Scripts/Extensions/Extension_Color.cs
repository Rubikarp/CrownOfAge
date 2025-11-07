using System;
using UnityEngine;

public static class Extension_Color
{
    public static Color SetAlpha(this Color color, float alpha) => new(color.r, color.g, color.b, alpha);

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
