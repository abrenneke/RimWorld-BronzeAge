using HarmonyLib;
using RimWorld;
using Verse;

namespace ABrenneke.BronzeAge.Patches.RimWorld
{
    [HarmonyPatch(typeof(ThoughtUtility), nameof(ThoughtUtility.RemovePositiveBedroomThoughts))]
    public static class ThoughtUtility_RemovePositiveBedroomThoughts_ErrorFix
    {
        // pawn.needs null while loading, just ignore that, should be fine
        public static bool Prefix(Pawn pawn)
        {
            return pawn?.needs?.mood != null;
        }
    }
}
