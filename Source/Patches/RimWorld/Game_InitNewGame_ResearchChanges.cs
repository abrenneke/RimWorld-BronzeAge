using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ABrenneke.BronzeAge.Patches.RimWorld
{
    [ConditionalModPatch("jpt.humanresources", typeof(Game), "InitNewGame")]
    public static class Game_InitNewGame_ResearchChanges
    {
        public static void Postfix()
        {
            // Clear current research
            var pawns = Find.World.PlayerPawnsForStoryteller.Where(p => p.IsColonist).ToList();
            var researchManager = Find.ResearchManager;
            var currentResearch = researchManager.GetPrivate<Dictionary<ResearchProjectDef, float>>("progress");

            foreach (var key in currentResearch.Keys.ToList()
                .Where(key => currentResearch[key] > 0))
            {
                currentResearch[key] = 0;
            }

            // Remove starting pawn expertises
            foreach (var pawn in pawns)
            {
                var knowledge = pawn.GetComp<HumanResources.CompKnowledge>();
                foreach (var tech in knowledge.expertise.Keys.ToList())
                {
                    knowledge.expertise.Remove(tech);
                }

                knowledge.proficientWeapons.Clear();
            }

            // Generate starting research
            var startingResearch = DefDatabase<ResearchProjectDef>.AllDefsListForReading
                .Where(def => def.GetModExtension<StartingResearch>() != null).ToList();

            var researchCount = new[] { 9, 10, 11, 12, 13, 14 }.RandomElement();

            // Give one research to the best pawn for that research, and then also mark
            // that research as completed, so it can be used immediately.
            // The same research can be given to multiple pawns, will still take one of the research "slots" (researchCount)
            for (var i = 0; i < researchCount; i++)
            {
                var research = startingResearch.RandomElementByWeight(x => x.GetModExtension<StartingResearch>().weight);
                var researchSkill = research.GetModExtension<StartingResearch>().skill;

                var pawnsBySkill = pawns.OrderByDescending(p => p.skills.GetSkill(researchSkill).Level);
                foreach (var pawn in pawnsBySkill)
                {
                    var knowledge = pawn.GetComp<HumanResources.CompKnowledge>();
                    if (knowledge.expertise.ContainsKey(research))
                        continue;

                    GivePawnResearch(knowledge, research);
                    break;
                }
            }

            var startingWeapons = DefDatabase<ThingDef>.AllDefsListForReading
                .Where(def => def.GetModExtension<StartingWeapon>() != null).ToList();

            foreach (var pawn in pawns)
            {
                var knowledge = pawn.GetComp<HumanResources.CompKnowledge>();
                foreach (var weapon in startingWeapons)
                {
                    knowledge.proficientWeapons.Add(weapon);
                }
            }
        }

        // All types not in this lib need to be boxed in object because of RW type loading stuff, even private
        private static void GivePawnResearch(object knowledgeBoxed, ResearchProjectDef research)
        {
            var knowledge = (HumanResources.CompKnowledge) knowledgeBoxed;
            if (research.prerequisites?.Count > 0)
            {
                foreach (var prereq in research.prerequisites)
                {
                    GivePawnResearch(knowledge, prereq);
                }
            }

            if (knowledge.expertise.ContainsKey(research))
                return;
        
            knowledge.expertise.Add(research, 1.0f);
            Find.ResearchManager.FinishProject(research);
        }
    }
}
