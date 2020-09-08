using HarmonyLib;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    [HarmonyPatch(typeof(ThingRequest), nameof(ThingRequest.Accepts))]
    public static class ThingRequest_Accepts_AlternateThings
    {
        public static void Postfix(Thing t, ThingRequest __instance, ref bool __result)
        {
            if (__result || __instance.singleDef == null || !Alternates.TryGet(t.def, out var alternates))
                return;

            foreach (var alt in alternates)
            {
                if (alt == t.def)
                {
                    __result = true;
                    return;
                }
            }
        }
    }
}
