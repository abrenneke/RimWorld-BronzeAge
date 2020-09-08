using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ABrenneke.BronzeAge.Patches.RimWorld
{
    [HarmonyPatch(typeof(Bill), nameof(Bill.IsFixedOrAllowedIngredient), typeof(Thing))]
    public static class Bill_IsFixedOrAllowedIngredient_Thing_AlternateThings
    {
        public static void Postfix(Thing thing, Bill __instance, bool __result)
        {

        }
    }

    [HarmonyPatch(typeof(Bill), nameof(Bill.IsFixedOrAllowedIngredient), typeof(ThingDef))]
    public static class Bill_IsFixedOrAllowedIngredient_ThingDef_AlternateThings
    {
    }
}
