#nullable enable
using System;
using HugsLib;
using JetBrains.Annotations;
using Verse;

namespace ABrenneke.BronzeAge
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature, ImplicitUseTargetFlags.WithMembers)]
    public class BronzeAgeMod : ModBase
    {
        public override string LogIdentifier => "abrenneke.bronze-age";

        public BronzeAgeMod()
        {
            // HugsLib Documentation: https://github.com/UnlimitedHugs/RimworldHugsLib/wiki
        }
    }
}
