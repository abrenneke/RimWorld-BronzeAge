using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace ABrenneke.BronzeAge.Patches.RimWorld
{
    [HarmonyPatch(typeof(WorkGiver_ConstructDeliverResources), "ResourceValidator")]
    public static class WorkGiver_ConstructDeliverResources_ResourceValidator_AlternateThings
    {
        public static void Postfix(Pawn pawn, ThingDefCountClass need, Thing th, ref bool __result)
        {
            if (__result || need.thingDef == th.def || !Alternates.TryGet(need.thingDef, out var alternates))
                return;

            foreach (var def in alternates)
            {
                if (def == th.def && !th.IsForbidden(pawn) && pawn.CanReserve(th))
                {
                    __result = true;
                    break;
                }
            }
        }
    }
}
