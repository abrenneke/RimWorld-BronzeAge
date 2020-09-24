using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Mods.RimSeasoning
{
    [ModPatch("tikubonn.RimSeasoning", "RimSeasoning.ThoughtWorker_IsDifferentCondimentLoverSocial", "CurrentSocialStateInternal")]
    public static class ThoughtWorker_IsDifferentCondimentLoverSocial_NoWarning
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionList = instructions.ToList();
            foreach (var instruction in instructionList)
            {
                if (instruction.Calls(AccessTools.Method(typeof(Log), nameof(Log.Warning))))
                {
                    continue;
                }

                yield return instruction;
            }
        }
    }
}
