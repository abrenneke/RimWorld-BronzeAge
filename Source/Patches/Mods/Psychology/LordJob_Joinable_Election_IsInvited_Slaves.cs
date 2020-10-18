using Verse;

namespace ABrenneke.BronzeAge.Patches.Mods.Psychology
{
    [ModPatch("community.psychology.unofficialupdate", "Psychology.LordJob_Joinable_Election", "IsInvited")]
    public static class LordJob_Joinable_Election_IsInvited_Slaves
    {
        public static void Postfix(Pawn p, ref bool __result)
        {
            __result = __result && !p.IsSlave();
        }
    }
}
