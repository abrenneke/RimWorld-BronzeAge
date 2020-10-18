using Verse;

namespace ABrenneke.BronzeAge.Patches.Mods.RumorHasIt
{
    [ModPatch("mlie.rfrumorhasit", "Rumor_Code.ThirdPartyManager", "DoesEveryoneLocallyHate")]
    public static class ThirdPartyManager_DoesEveryoneLocallyHate_Slaves
    {
        public static void Postfix(Pawn p, ref bool __result)
        {
            __result = __result && !p.IsSlave();
        }
    }
}
