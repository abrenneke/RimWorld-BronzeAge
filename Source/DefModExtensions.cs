#nullable enable
using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using Verse;
// ReSharper disable InconsistentNaming
// ReSharper disable UnassignedField.Global
// ReSharper disable ConvertToConstant.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

#pragma warning disable 8618
namespace ABrenneke.BronzeAge
{
    [PublicAPI]
    public class StartingResearch : DefModExtension
    {
        public SkillDef skill;

        public float weight = 1;
    }

    [PublicAPI]
    public class StartingWeapon : DefModExtension
    {
    }

    [PublicAPI]
    public class AlternateResources : DefModExtension
    {
        public List<ThingDef> thingDefs;
    } 
}
#pragma warning restore 8618
