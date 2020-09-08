#nullable enable
using System;
using System.Globalization;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace ABrenneke.BronzeAge.Patches.UnityEngine
{
    /*
    /// <summary>Pure C# implementation of TryParseHtmlString.</summary>
    [EarlyPatch(typeof(ColorUtility), nameof(ColorUtility.TryParseHtmlString), 
        new [] { typeof(string), typeof(Color) }, 
        new [] { ArgumentType.Normal, ArgumentType.Out })]
    public static class ColorUtility_TryParseHtmlString
    {
        // ReSharper disable once RedundantAssignment
        public static bool Prefix(string htmlString, out Color color, ref bool __result)
        {
            color = default;

            if (string.IsNullOrWhiteSpace(htmlString) || !htmlString.StartsWith("#", StringComparison.Ordinal))
            {
                __result = false;
                return false;
            }

            var code = htmlString.Substring(1);

            switch (code.Length)
            {
                // rrggbb (#ffffff)
                case 6:
                {
                    var (r, g, b, _) = code.SplitBy(2);
                    __result = ToColor(r, g, b, null, out color);
                    break;
                }
                // rrggbbaa (#ffffffff)
                case 8:
                {
                    var (r, g, b, a, _) = code.SplitBy(2);
                    __result = ToColor(r, g, b, a, out color);
                    break;
                }
                // rgb (#fff)
                case 3:
                {
                    var (r, g, b, _) = code;
                    __result = ToColor($"{r}{r}", $"{g}{g}", $"{b}{b}", null, out color);
                    break;
                }
                // rgba (#ffff)
                case 4:
                {
                    var (r, g, b, a, _) = code;
                    __result = ToColor($"{r}{r}", $"{g}{g}", $"{b}{b}", $"{a}{a}", out color);
                    break;
                }
                default:
                    __result = false;
                    break;
            }

            return false;
        }

        private static bool ToColor(string r, string g, string b, string? a, out Color color)
        {
            color = default;
            if (!byte.TryParse(r, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var rByte))
                return false;

            if (!byte.TryParse(g, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var gByte))
                return false;

            if (!byte.TryParse(b, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var bByte))
                return false;

            byte aByte = 0xff;
            if (a != null)
            {
                if (!byte.TryParse(a, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out aByte))
                    return false;
            }

            color = new Color(rByte / 255f, gByte / 255f, bByte / 255f, aByte / 255f);
            return true;
        }
    }
    */
}
