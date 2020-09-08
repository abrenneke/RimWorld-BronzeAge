using System;
using JetBrains.Annotations;

namespace ABrenneke.BronzeAge
{
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
    public class ModPatchAttribute : Attribute
    {
        public string ModPackageId { get; }

        public string ModTypeName { get; }

        public string ModMethodName { get; }

        public ModPatchAttribute(string modPackageId, string modTypeName, string modMethodName)
        {
            ModPackageId = modPackageId;
            ModTypeName = modTypeName;
            ModMethodName = modMethodName;
        }
    }
}
