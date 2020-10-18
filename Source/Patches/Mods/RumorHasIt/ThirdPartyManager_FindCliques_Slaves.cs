using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HarmonyLib;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Mods.RumorHasIt
{
    [ModPatch("mlie.rfrumorhasit", "Rumor_Code.ThirdPartyManager", "FindCliques")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class ThirdPartyManager_FindCliques_Slaves
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var get_FreeColonistsSpawned = AccessTools.PropertyGetter(typeof(MapPawns), nameof(MapPawns.FreeColonistsSpawned));
            foreach (var instruction in instructions)
            {
                if (instruction.Calls(get_FreeColonistsSpawned))
                {
                    yield return CodeInstruction.Call(typeof(ThirdPartyManager_FindCliques_Slaves), nameof(FreeNonSlaveColonistsSpawned));
                }
                else
                {
                    yield return instruction;
                }
            }
        }

        public static IEnumerable<Pawn> FreeNonSlaveColonistsSpawned(MapPawns instance)
        {
            return instance.FreeColonistsSpawned.Where(p => !p.IsSlave());
        }
    }
}
