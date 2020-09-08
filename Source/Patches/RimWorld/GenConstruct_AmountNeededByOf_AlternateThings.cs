using HarmonyLib;
using RimWorld;
using Verse;

namespace ABrenneke.BronzeAge.Patches.RimWorld
{
    [HarmonyPatch(typeof(GenConstruct), nameof(GenConstruct.AmountNeededByOf))]
    public static class GenConstruct_AmountNeededByOf_AlternateThings
    {
        public static void Postfix(IConstructible c, ThingDef resDef, ref int __result)
        {
            if (__result > 0)
                return;

            foreach (var needed in c.MaterialsNeeded())
            {
                var alternates = Alternates.Get(needed.thingDef);
                foreach (var alt in alternates)
                {
                    if (alt == resDef)
                    {
                        __result = needed.count;
                        return;
                    }
                }
            }
        }
    }
}
