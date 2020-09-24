using System.Linq;
using Verse;

namespace ABrenneke.BronzeAge.Patches.RimWorld
{
    [ConditionalModPatch("fluffy.worktab", typeof(Game), nameof(Game.InitNewGame))]
    public static class Game_InitNewGame_WorkDefaults
    {
        public static void Postfix()
        {
            WorkTab.PriorityManager.ShowPriorities = true;

            foreach (var pawn in Find.World.PlayerPawnsForStoryteller.Where(x => x.IsColonist))
            {
                PawnWork.SetUpInitialWorkPriorities(pawn);
            }
        }
    }
}
