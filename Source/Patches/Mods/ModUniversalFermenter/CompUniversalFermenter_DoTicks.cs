namespace ABrenneke.BronzeAge.Patches.Mods.ModUniversalFermenter
{
    [ModPatch("syrchalis.universalfermenter", "UniversalFermenter.CompUniversalFermenter", "DoTicks")]
    public static class CompUniversalFermenter_DoTicks
    {
        public static void Postfix(UniversalFermenter.CompUniversalFermenter __instance)
        {
            if (__instance.ProgressTicks < -16000000)
            {
                __instance.ProgressTicks = 16000000;
            }
        }
    }
}
