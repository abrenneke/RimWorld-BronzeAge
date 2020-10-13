using HarmonyLib;
using UnityEngine;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    [HarmonyPatch(typeof(WindManager), nameof(WindManager.WindManagerTick))]
    public static class WindManager_WindManagerTick_AddLimits
    {
        private static readonly AccessTools.FieldRef<WindManager, float> WindSpeed = AccessTools.FieldRefAccess<WindManager, float>("cachedWindSpeed");

        public static void Postfix(WindManager __instance)
        {
            ref var windSpeed = ref WindSpeed(__instance);
            windSpeed = Mathf.Clamp(windSpeed, 0, 3);
        }
    }
}
