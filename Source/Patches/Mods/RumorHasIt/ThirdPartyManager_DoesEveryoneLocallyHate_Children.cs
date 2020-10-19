using Verse;

namespace ABrenneke.BronzeAge.Patches.Mods.RumorHasIt
{
    [ModPatch("mlie.rfrumorhasit", "Rumor_Code.ThirdPartyManager", "DoesEveryoneLocallyHate")]
    public static class ThirdPartyManager_DoesEveryoneLocallyHate_Children
    {
        public static void Postfix(Pawn p, ref bool __result)
        {
            // TODO configurable age
            __result = __result && p.ageTracker.AgeBiologicalYears < 15;
        }
    }
}
