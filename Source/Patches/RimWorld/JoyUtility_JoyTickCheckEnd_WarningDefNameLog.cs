using HarmonyLib;
using RimWorld;
using Verse;

namespace ABrenneke.BronzeAge.Patches.RimWorld
{
    [HarmonyPatch(typeof(JoyUtility), nameof(JoyUtility.JoyTickCheckEnd))]
    public static class JoyUtility_JoyTickCheckEnd_WarningDefNameLog
    {
        public static void Prefix(Pawn pawn)
        {
            if (pawn.CurJob.def.joyKind == null)
            {
                Log.Warning($"JoyUtility.JoyTickCheckEnd - Pawn {pawn.ToStringSafe()} has job {pawn.CurJob.def.ToStringSafe()} without joy kind.");
            }
        }
    }
}
