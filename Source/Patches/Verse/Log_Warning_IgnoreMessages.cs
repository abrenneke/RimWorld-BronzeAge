using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    [EarlyPatch(typeof(Log), nameof(Log.Warning))]
    public static class Log_Warning_IgnoreMessages
    {
        private static readonly IList<Regex> Ignore = new List<Regex>
        {
            // Does not matter
            new Regex("Capacity (Fertility|BleedRate) does not have any bodyPartTags associated with it", RegexOptions.Compiled),
            
            // Do not care
            new Regex(@"\[Uuugggg.rimworld.Replace_Stuff.main\] Patches on methods annotated as Obsolete were detected by HugsLib: Verse\.GhostDrawer\.DrawGhostThing", RegexOptions.Compiled),

            // Not a problem
            new Regex("No relevant skills could be calculated for", RegexOptions.Compiled),
            new Regex("SeedsPlease :: Can't patch Achtung! No JobDriver_SowAll", RegexOptions.Compiled),

            // ???
            new Regex("Type Designator_Extract probably needs a StaticConstructorOnStartup attribute", RegexOptions.Compiled)
        };

        public static bool Prefix(string text)
        {
            return !Ignore.Any(x => x.IsMatch(text));
        }
    }
}
