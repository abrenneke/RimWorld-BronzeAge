using System;
using HarmonyLib;
using JetBrains.Annotations;

namespace ABrenneke.BronzeAge
{
    [MeansImplicitUse(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, ImplicitUseTargetFlags.WithMembers)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = true)]
    public class ConditionalModPatchAttribute : HarmonyAttribute
    {
        public string ModPackageId { get; }

        public ConditionalModPatchAttribute(string modPackageId, Type declaringType, string methodName)
        {
            ModPackageId = modPackageId;
            info.declaringType = declaringType;
            info.methodName = methodName;
        }
    }
}
