using RimWorld;

namespace ABrenneke.BronzeAge.Patches.RimWorld
{
    [ConditionalModPatch("fluffy.worktab", typeof(Pawn_WorkSettings), nameof(Pawn_WorkSettings.EnableAndInitialize))]
    public static class Pawn_WorkSettings_EnableAndInitialize_CustomPriorities
    {
        public static void Postfix(Pawn_WorkSettings __instance)
        {
            var pawn = WorkTab.VanillaWorkSettings.Pawn(__instance);
            PawnWork.SetUpInitialWorkPriorities(pawn);
        }
    }
}
