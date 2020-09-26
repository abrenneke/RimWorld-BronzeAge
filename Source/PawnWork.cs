﻿#nullable enable
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ABrenneke.BronzeAge
{
    public static class PawnWork
    {
        public static void SetUpInitialWorkPriorities(Pawn pawn)
        {
            // Firefighting and patient 1
            pawn.SetWorkPriority("Firefighter", 1);
            pawn.SetWorkPriority("Patient", 1);

            // Studying should be priority 2 (if assigned, do this first)
            pawn.SetWorkPriority("HR_Learn", 1);

            // Haul+, Basic, and Bed Rest Priority 3
            pawn.SetWorkPriority("BasicWorker", 3);
            pawn.SetWorkPriority("PatientBedRest", 3);
            pawn.SetWorkPriority("HaulingUrgent", 3);

            // Cleaning hauling 4
            pawn.SetWorkPriority("Cleaning", 4);
            pawn.SetWorkPriority("Hauling", 4);

            // Turn off unused ones at beginning
            pawn.SetWorkPriority("Study", 0);
            pawn.SetWorkPriority("Teach", 0);
            pawn.SetWorkPriority("NuclearWork", 0);
            pawn.SetWorkPriority("RimefellerCrafting", 0);
            pawn.SetWorkPriority("RB_BeekeepingWork", 0);

            // Doctors should highly prioritize doctoring
            var doctor = Work("Doctor");
            if (!pawn.WorkTypeIsDisabled(doctor) && pawn.skills.AverageOfRelevantSkillsFor(doctor) >= 4)
            {
                pawn.SetWorkPriority(doctor, 1);
            }

            // Remaining priorities are based on passion
            var used = new HashSet<string>(new[]
            {
                "Firefighter", 
                "Patient", 
                "Study",
                "Teach",
                "NuclearWork",
                "RimefellerCrafting",
                "RB_BeekeepingWork",
                "BasicWorker",
                "PatientBedRest",
                "HaulingUrgent",
                "Doctor"
            });

            var remainingWork = DefDatabase<WorkTypeDef>.AllDefsListForReading
                .Where(def => !used.Contains(def.defName));

            foreach (var work in remainingWork)
            {
                var passion = pawn.skills.MaxPassionOfRelevantSkillsFor(work);
                var interestDef = DInterests.InterestBase.interestList[(int) passion];

                var map = new Dictionary<string, int>
                {
                    { "DCompulsion", 2 },
                    { "DNaturalGenius", 3 },
                    { "DMajorPassion", 3 },
                    { "DMinorPassion", 4 },
                    { "DForgetful", 4 },
                    { "DInvigorating", 5 },
                    { "DInspiring", 3 },
                    { "DStagnant", 3 },
                    { "DVocalHatred", 5 },
                    { "DBored", 4 },
                    { "DAllergic", 4 }
                };

                if (interestDef != null && map.TryGetValue(interestDef.defName, out var priority))
                {
                    pawn.SetWorkPriority(work, priority);
                }
                else if(pawn.skills.AverageOfRelevantSkillsFor(work) >= 6)
                {
                    pawn.SetWorkPriority(work, 5);
                }
                else
                {
                    pawn.SetWorkPriority(work, 0);
                }
            }
        }

        private static void SetWorkPriority(this Pawn pawn, string workType, int priority)
        {
            SetWorkPriority(pawn, Work(workType), priority);
        }

        private static void SetWorkPriority(this Pawn pawn, WorkTypeDef? workType, int priority)
        {
            if (workType == null)
            {
                Log.Warning($"Cannot set priority for work for {pawn.Name}, workType is null");
                return;
            }

            if (!pawn.WorkTypeIsDisabled(workType))
            {
                WorkTab.PriorityManager.Get[pawn].SetPriority(workType, priority, null);
            }
        }

        private static WorkTypeDef? Work(string name)
        {
            var work = DefDatabase<WorkTypeDef>.GetNamed(name);
            if (work == null)
                Log.Warning($"Could not find work type {name}.");
            return work;
        }
    }
}