using System;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    [HarmonyPatch(typeof(WindManager), nameof(WindManager.WindManagerTick))]
    public static class WindManager_WindManagerTick_AddLimits
    {
        public static Func<WindManager, float> GetWindSpeed = AccessTools.Field(typeof(WindManager), "cachedWindSpeed").CreateGetter<WindManager, float>();
        public static Action<WindManager, float> SetWindSpeed = AccessTools.Field(typeof(WindManager), "cachedWindSpeed").CreateSetter<WindManager, float>();

        public static void Postfix(WindManager __instance)
        {
            var windSpeed = GetWindSpeed(__instance);
            SetWindSpeed(__instance, Mathf.Clamp(windSpeed, 0, 3));
        }
    }
}
