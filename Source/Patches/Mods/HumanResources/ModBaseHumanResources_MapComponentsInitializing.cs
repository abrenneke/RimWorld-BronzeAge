using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Mods.HumanResources
{
    [ModPatch("jpt.humanresources", "HumanResources.ModBaseHumanResources", "MapComponentsInitializing")]
    public static class ModBaseHumanResources_MapComponentsInitializing
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.Calls(typeof(GenScene).GetProperty("InPlayScene").GetGetMethod()))
                {
                    // FROM: call bool ['Assembly-CSharp']Verse.GenScene::get_InPlayScene()
                    // TO:   ldc.i4.1
                    // (Push a `true` onto the stack, instead of calling the above method)
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                }
                else
                {
                    yield return instruction;
                }
            }
        }
    }
}
