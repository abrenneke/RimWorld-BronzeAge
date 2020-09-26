using RimWorld;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Mods.Rimocracy
{
    [ModPatch("garwel.rimocracy", "Rimocracy.StatPart_Governance", "Multiplier")]
    public static class StatPart_Governance_Multiplier_ExceptionFix
    {
        // Mod does not check if req.Thing.Faction is null
        public static bool Prefix(StatRequest req, ref float __result)
        {
            var isError = req.HasThing && req.Thing is Building && req.Thing.Faction == null;
            if (isError)
                __result = 1;

            return !isError;
        }
    }
}
