#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Mods.JobsOfOpportunity
{
    [ModPatch("codeoptimist.jobsofopportunity", "JobsOfOpportunity.JobsOfOpportunity+Patch_TryOpportunisticJob", "TryOpportunisticJob")]
    public static class Patch_TryOpportunisticJob_NotWhenGearing
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var inst = instructions.ToList();
            for (var i = 0; i < inst.Count; i++)
            {
                if (i + 3 >= inst.Count || i < 10)
                {
                    yield return inst[i];
                    continue;
                }

                if (inst[i].opcode == OpCodes.Ldloc_0
                    && inst[i + 1].opcode == OpCodes.Ldloc_1
                    && inst[i + 2].operand is MethodInfo info
                    && info.Name == "TryHaul"
                    && inst[i + 3].opcode == OpCodes.Ret
                    && inst[i - 3].opcode == OpCodes.Ble_Un_S)
                {
                    var label = inst[i - 3].operand;

                    yield return CodeInstruction.Call(typeof(Patch_TryOpportunisticJob_NotWhenGearing), nameof(IsGearing));
                    yield return new CodeInstruction(OpCodes.Brfalse_S, label);
                    yield return new CodeInstruction(OpCodes.Ldnull);
                    yield return new CodeInstruction(OpCodes.Ret);
                }
                
                yield return inst[i];
            }
        }

        public static bool IsGearing()
        {
            if (!BronzeAgeMod.ModIsRunning("Gear Up And Go", "uuugggg.gearupandgo"))
                return false;

            var comp = (GearUpAndGo.GearUpPolicyComp?) Current.Game.GetComponent(typeof(GearUpAndGo.GearUpPolicyComp));
            return comp is not null && comp.IsOn();
        }
    }
}
