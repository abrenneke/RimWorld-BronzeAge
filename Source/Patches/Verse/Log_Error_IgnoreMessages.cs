using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    [EarlyPatch(typeof(Log), nameof(Log.Error))]
    public static class Log_Error_IgnoreMessages
    {
        private static readonly IList<Regex> Ignore = new List<Regex>
        {
            // DefOfs that don't exist but also don't matter
            new Regex(@"Failed to find Verse\.\w+Def named (HR_Learn|RandomSeed)", RegexOptions.Compiled),
        };

        public static bool Prefix(string text)
        {
            return !Ignore.Any(x => x.IsMatch(text));
        }
    }
}
