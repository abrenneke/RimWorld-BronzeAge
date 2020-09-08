using HarmonyLib;
using RimWorld;
using Verse;

namespace ABrenneke.BronzeAge.Patches.RimWorld
{
    [HarmonyPatch(typeof(ItemAvailability), nameof(ItemAvailability.ThingsAvailableAnywhere))]
    public static class ItemAvailability_ThingsAvailableAnywhere_AlternateThing
    {
        public static void Postfix(ThingDefCountClass need, Pawn pawn, ref bool __result, ItemAvailability __instance)
        {
            if (__result || !Alternates.TryGet(need.thingDef, out var alternates))
                return;

            // Potential infinite recursion
            foreach (var def in alternates)
            {
                if (__instance.ThingsAvailableAnywhere(new ThingDefCountClass(def, need.count), pawn))
                {
                    __result = true;
                    return;
                }
            }

            //TODO 5 + 5 = 10
        }
    }
}
