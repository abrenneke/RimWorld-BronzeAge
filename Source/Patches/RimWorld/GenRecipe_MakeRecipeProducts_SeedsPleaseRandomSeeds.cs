﻿#nullable enable
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using SeedsPlease;
using Verse;

namespace ABrenneke.BronzeAge.Patches.RimWorld
{
    [HarmonyPatch(typeof(GenRecipe), nameof(GenRecipe.MakeRecipeProducts)), UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class GenRecipe_MakeRecipeProducts_SeedsPleaseRandomSeeds
    {
        private static IList<SeedDef>? allSeedDefs;

        public static IEnumerable<Thing?> Postfix(IEnumerable<Thing> recipeProducts, Pawn worker)
        {
            foreach (var thing in recipeProducts)
            {
                if (thing == null || thing.def != Defs.RandomSeed)
                {
                    yield return thing;
                    continue;
                }

                var stackCount = thing.stackCount;
                thing.Destroy();

                allSeedDefs ??= DefDatabase<ThingDef>.AllDefsListForReading.OfType<SeedDef>().ToList();

                var possibleSeeds = allSeedDefs.Where(def => def.sources.Any(plantThing =>
                {
                    if (plantThing == null)
                        return false;

                    if (plantThing.plant == null)
                    {
                        if (plantThing.IsResearchFinished)
                            Log.Message($"Allowing {plantThing.defName} because does not have PlantProperties (IsResearchFinished).");
                        return plantThing.IsResearchFinished;
                    }

                    // if(worker.Map.Biome.CommonalityOfPlant(plantThing) <= 0.0f)
                    // {
                    //     Log.Message($"Not allowing {plantThing.defName} because the biome ${worker.Map.Biome} is not valid for the plant.");
                    //     return false;
                    // }

                    var allowed = plantThing.plant.sowResearchPrerequisites?.Any(research => research.IsFinished) ?? true;

                    if (allowed && plantThing.plant.sowResearchPrerequisites == null)
                    {
                        Log.Message($"Allowing {plantThing.defName} because does not have sowResearchPrerequisites");
                    }
                    else if (allowed)
                    {
                        Log.Message($"Allowing {plantThing.defName} because it has been researched.");
                    }
                    else
                    {
                        Log.Message($"Not allowing {plantThing.defName} because it has not been researched.");
                    }

                    return allowed;
                }));

                var seedDef = possibleSeeds.RandomElementWithFallback();

                if (seedDef != null)
                {
                    Messages.Message($"{worker.LabelIndefinite()} found {stackCount} {seedDef.plant.label} seeds", worker, MessageTypeDefOf.TaskCompletion, false);
                    var seed = ThingMaker.MakeThing(seedDef);
                    seed.stackCount = stackCount;

                    yield return seed;
                }
            }
        }
    }
}
