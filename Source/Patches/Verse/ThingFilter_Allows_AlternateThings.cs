using HarmonyLib;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    /*[HarmonyPatch(typeof(ThingFilter), nameof(ThingFilter.Allows), typeof(ThingDef))]
    public static class ThingFilter_Allows_AlternateThings
    {
        public static void Postfix(ThingDef def, ThingFilter __instance, ref bool __result)
        {
            if (__result)
                return;

            // Slow?
            foreach (var allowed in __instance.AllowedThingDefs)
            {
                if (Alternates.TryGet(allowed, out var alternates))
                {
                    foreach (var alt in alternates)
                    {
                        if (alt == def)
                        {
                            __result = true;
                            return;
                        }
                    }
                }
            }
        }
    }*/
}
