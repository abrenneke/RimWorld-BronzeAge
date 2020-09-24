using System;

namespace ABrenneke.BronzeAge.Patches.Mods.ModUniversalFermenter
{
    [ModPatch("syrchalis.universalfermenter", "UniversalFermenter.CompUniversalFermenter", "DoTicks")]
    public static class CompUniversalFermenter_DoTicks
    {
        public static void Postfix(object __instance)
        {
            var comp = (UniversalFermenter.CompUniversalFermenter) __instance;
            if (comp.progressTicks < 0)
            {
                throw new InvalidOperationException($"progressTicks was {comp.progressTicks}!");
            }
            // if (comp.ProgressTicks < -16000000)
            // {
            //     comp.ProgressTicks = 16000000;
            // }
        }
    }
}
