#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ABrenneke.BronzeAge
{
    public static class Alternates
    {
        private static IDictionary<ThingDef, IList<ThingDef>>? alternatesCache;

        public static IList<ThingDef> Get(ThingDef def)
        {
            alternatesCache ??= DefDatabase<ThingDef>.AllDefsListForReading.Where(d => d.GetModExtension<AlternateResources>() != null)
                .ToDictionary(d => d, d => (IList<ThingDef>)d.GetModExtension<AlternateResources>().thingDefs);

            return alternatesCache.TryGetValue(def, out var alts) ? alts : Array.Empty<ThingDef>();
        }

        public static bool TryGet(ThingDef def, out IList<ThingDef> alternates)
        {
            alternatesCache ??= DefDatabase<ThingDef>.AllDefsListForReading.Where(d => d.GetModExtension<AlternateResources>() != null)
                .ToDictionary(d => d, d => (IList<ThingDef>)d.GetModExtension<AlternateResources>().thingDefs);

            return alternatesCache.TryGetValue(def, out alternates);
        }
    }
}
