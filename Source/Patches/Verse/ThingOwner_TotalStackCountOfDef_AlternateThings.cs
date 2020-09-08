using HarmonyLib;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    [HarmonyPatch(typeof(ThingOwner), nameof(ThingOwner.TotalStackCountOfDef))]
    public static class ThingOwner_TotalStackCountOfDef_AlternateThings
    {
        public static void Postfix(ThingDef def, ThingOwner __instance, ref int __result)
        {
            foreach (var alt in Alternates.Get(def))
            {
                __result += __instance.TotalStackCountOfDef(alt);
            }
        }
    }
}
