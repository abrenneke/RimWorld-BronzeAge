using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Mods.Psychology
{
    [ModPatch("community.psychology.unofficialupdate", "Psychology.GameCondition_Election", "Init")]
    public static class GameCondition_Election_Slaves
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var ins = instructions.ToList();
            for (var i = 0; i < ins.Count; i++)
            {
                if (i > 10
                    && ins[i].opcode == OpCodes.Ldloc_1
                    && ins[i-1].opcode == OpCodes.Stloc_1
                    && ins[i-2].operand != null
                    && ((MethodInfo)ins[i-2].operand).Name == "Where"
                    && ins[i-3].opcode == OpCodes.Stsfld
                    && ins[i-4].opcode == OpCodes.Dup
                    && ins[i-5].opcode == OpCodes.Newobj
                    && ins[i-6].opcode == OpCodes.Ldftn)
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return CodeInstruction.Call(typeof(GameCondition_Election_Slaves), nameof(ExcludeSlaves));
                    yield return new CodeInstruction(OpCodes.Stloc_1);
                }

                yield return ins[i];
            }
        }

        [UsedImplicitly]
        private static IEnumerable<Pawn> ExcludeSlaves(IEnumerable<Pawn> pawns)
        {
            return pawns.Where(p => !p.IsSlave());
        }
    }
}
