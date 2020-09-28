using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace ABrenneke.BronzeAge
{
    [DefOf]
    public static class Defs
    {
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public static ThingDef RandomSeed = null!;

        static Defs()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(Defs));
        }
    }
}
