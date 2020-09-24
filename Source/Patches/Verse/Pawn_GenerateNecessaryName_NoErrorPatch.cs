using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.GenerateNecessaryName))]
    public static class Pawn_GenerateNecessaryName_NoErrorPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.Calls(AccessTools.DeclaredPropertyGetter(typeof(Faction), nameof(Faction.OfPlayer))))
                {
                    var silentFail = AccessTools.DeclaredPropertyGetter(typeof(Faction), nameof(Faction.OfPlayerSilentFail));
                    yield return CodeInstruction.Call(typeof(Faction), silentFail.Name);
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }
}
