#nullable enable
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    [HarmonyPatch(typeof(ListerThings), nameof(ListerThings.ThingsMatching))]
    public static class ListerThings_ThingsMatching_AlternateThings
    {
        public static void Postfix(ThingRequest req, ListerThings __instance, ref List<Thing> __result)
        {
            if (req.singleDef == null || !Alternates.TryGet(req.singleDef, out var alternates))
                return;

            var resultOverwritten = false;

            foreach (var alt in alternates)
            {
                // Worse perf than implemented in RW, might be bad
                // Verse never allocates new arrays
                var matching = __instance.ThingsMatching(ThingRequest.ForDef(alt));

                if (matching.Count > 0 && !resultOverwritten)
                {
                    __result = __result.ToList();
                    resultOverwritten = true;
                }

                __result.AddRange(matching);
            }
        }
    }
}
