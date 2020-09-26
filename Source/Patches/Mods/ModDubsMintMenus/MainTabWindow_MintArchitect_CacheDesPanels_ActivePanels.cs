#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Mods.ModDubsMintMenus
{
    [ModPatch("dubwise.dubsmintmenus", "DubsMintMenus.MainTabWindow_MintArchitect", "get_CacheDesPanels")]
    public static class MainTabWindow_MintArchitect_CacheDesPanels_ActivePanels
    {
        private static readonly HashSet<string> IgnoreDesignators = new HashSet<string>
        {
            "Designator_RemovePipe",
            "Designator_CleanOil" ,
            "Designator_RemovePipeline",
            "Designator_RemoveSkylights"
        };

        private static readonly List<ArchitectCategoryTab> CachedList = new List<ArchitectCategoryTab>();

        private static List<ArchitectCategoryTab>? desCategories;

        // ReSharper disable once RedundantAssignment
        public static bool Prefix(ref List<ArchitectCategoryTab> __result)
        {
            if (desCategories == null)
            {
                var allCategories = DefDatabase<DesignationCategoryDef>.AllDefsListForReading;
                var alphaSortedCategories = allCategories.OrderBy(x => (string)x.LabelCap, StringComparer.CurrentCulture);

                desCategories = alphaSortedCategories.Select(cat => new ArchitectCategoryTab(cat)).ToList();
            }

            // TODO cache more, could be faster. Don't notice slowdown right now, though.

            CachedList.Clear();
            foreach (var cat in desCategories)
            {
                bool allow = false;
                foreach (var designator in cat.def.ResolvedAllowedDesignators)
                {
                    if (designator.Visible && !(
                        designator is Designator_Cancel
                        || designator is Designator_Deconstruct
                        || designator is Designator_Uninstall
                        || IgnoreDesignators.Contains(designator.GetType().Name) ))
                    {
                        allow = true;
                        break;
                    }
                }

                if (allow)
                {
                    CachedList.Add(cat);
                }
            }

            __result = CachedList;
            return false;
        }
    }
}
