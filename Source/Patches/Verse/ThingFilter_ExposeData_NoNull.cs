#nullable enable
using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    [HarmonyPatch(typeof(ThingFilter), nameof(ThingFilter.ExposeData))]
    public static class ThingFilter_ExposeData_NoNull
    {
        // Remove null entries both before and after ExposeData, so that null isn't saved or loaded.
        // Null ThingDefs may happen if a mod is removed, for example
        public static void Prefix(ThingFilter __instance)
        {
            HashSet<ThingDef?> things = ((HashSet<ThingDef>)__instance.AllowedThingDefs)!;
            things.Remove(null);
        }

        public static void Postfix(ThingFilter __instance)
        {
            HashSet<ThingDef?> things = ((HashSet<ThingDef>)__instance.AllowedThingDefs)!;
            things.Remove(null);
        }
    }

    [HarmonyPatch(typeof(ThingFilter), nameof(ThingFilter.SetAllow), typeof(ThingDef), typeof(bool))]
    public static class ThingFilter_SetAllow_NoNull
    {
        // Don't allow a null in
        public static bool Prefix(ThingDef thingDef)
        {
            return thingDef != null;
        }
    }
}
