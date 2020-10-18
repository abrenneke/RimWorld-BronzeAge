#nullable enable
using System;
using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;
// ReSharper disable UnusedMember.Global

namespace ABrenneke.BronzeAge
{
    [MeansImplicitUse(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, ImplicitUseTargetFlags.WithMembers)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = true)]
    public class EarlyPatchAttribute : HarmonyAttribute
    {
        /// <summary>An empty annotation can be used together with TargetMethod(s)</summary>
        public EarlyPatchAttribute()
        {
        }

        /// <summary>An annotation that specifies a class to patch</summary>
        /// <param name="declaringType">The declaring class/type</param>
        public EarlyPatchAttribute(Type declaringType)
        {
            info.declaringType = declaringType;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="declaringType">The declaring class/type</param>
        /// <param name="argumentTypes">The argument types of the method or constructor to patch</param>
        public EarlyPatchAttribute(Type declaringType, Type[] argumentTypes)
        {
            info.declaringType = declaringType;
            info.argumentTypes = argumentTypes;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="declaringType">The declaring class/type</param>
        /// <param name="methodName">The name of the method, property or constructor to patch</param>
        public EarlyPatchAttribute(Type declaringType, string methodName)
        {
            info.declaringType = declaringType;
            info.methodName = methodName;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="declaringType">The declaring class/type</param>
        /// <param name="methodName">The name of the method, property or constructor to patch</param>
        /// <param name="argumentTypes">An array of argument types to target overloads</param>
        public EarlyPatchAttribute(Type declaringType, string methodName, params Type[] argumentTypes)
        {
            info.declaringType = declaringType;
            info.methodName = methodName;
            info.argumentTypes = argumentTypes;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="declaringType">The declaring class/type</param>
        /// <param name="methodName">The name of the method, property or constructor to patch</param>
        /// <param name="argumentTypes">An array of argument types to target overloads</param>
        /// <param name="argumentVariations">Array of <see cref="T:HarmonyLib.ArgumentType" /></param>
        public EarlyPatchAttribute(
            Type declaringType,
            string methodName,
            Type[] argumentTypes,
            ArgumentType[] argumentVariations)
        {
            info.declaringType = declaringType;
            info.methodName = methodName;
            ParseSpecialArguments(argumentTypes, argumentVariations);
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="declaringType">The declaring class/type</param>
        /// <param name="methodType">The <see cref="T:HarmonyLib.MethodType" /></param>
        public EarlyPatchAttribute(Type declaringType, MethodType methodType)
        {
            info.declaringType = declaringType;
            info.methodType = methodType;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="declaringType">The declaring class/type</param>
        /// <param name="methodType">The <see cref="T:HarmonyLib.MethodType" /></param>
        /// <param name="argumentTypes">An array of argument types to target overloads</param>
        public EarlyPatchAttribute(Type declaringType, MethodType methodType, params Type[] argumentTypes)
        {
            info.declaringType = declaringType;
            info.methodType = methodType;
            info.argumentTypes = argumentTypes;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="declaringType">The declaring class/type</param>
        /// <param name="methodType">The <see cref="T:HarmonyLib.MethodType" /></param>
        /// <param name="argumentTypes">An array of argument types to target overloads</param>
        /// <param name="argumentVariations">Array of <see cref="T:HarmonyLib.ArgumentType" /></param>
        public EarlyPatchAttribute(
            Type declaringType,
            MethodType methodType,
            Type[] argumentTypes,
            ArgumentType[] argumentVariations)
        {
            info.declaringType = declaringType;
            info.methodType = methodType;
            ParseSpecialArguments(argumentTypes, argumentVariations);
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="declaringType">The declaring class/type</param>
        /// <param name="methodName">The name of the method, property or constructor to patch</param>
        /// <param name="methodType">The <see cref="T:HarmonyLib.MethodType" /></param>
        public EarlyPatchAttribute(Type declaringType, string methodName, MethodType methodType)
        {
            info.declaringType = declaringType;
            info.methodName = methodName;
            info.methodType = methodType;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="methodName">The name of the method, property or constructor to patch</param>
        public EarlyPatchAttribute(string methodName)
        {
            info.methodName = methodName;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="methodName">The name of the method, property or constructor to patch</param>
        /// <param name="argumentTypes">An array of argument types to target overloads</param>
        public EarlyPatchAttribute(string methodName, params Type[] argumentTypes)
        {
            info.methodName = methodName;
            info.argumentTypes = argumentTypes;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="methodName">The name of the method, property or constructor to patch</param>
        /// <param name="argumentTypes">An array of argument types to target overloads</param>
        /// <param name="argumentVariations">An array of <see cref="T:HarmonyLib.ArgumentType" /></param>
        public EarlyPatchAttribute(string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
        {
            info.methodName = methodName;
            ParseSpecialArguments(argumentTypes, argumentVariations);
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="methodName">The name of the method, property or constructor to patch</param>
        /// <param name="methodType">The <see cref="T:HarmonyLib.MethodType" /></param>
        public EarlyPatchAttribute(string methodName, MethodType methodType)
        {
            info.methodName = methodName;
            info.methodType = methodType;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="methodType">The <see cref="T:HarmonyLib.MethodType" /></param>
        public EarlyPatchAttribute(MethodType methodType)
        {
            info.methodType = methodType;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="methodType">The <see cref="T:HarmonyLib.MethodType" /></param>
        /// <param name="argumentTypes">An array of argument types to target overloads</param>
        public EarlyPatchAttribute(MethodType methodType, params Type[] argumentTypes)
        {
            info.methodType = methodType;
            info.argumentTypes = argumentTypes;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="methodType">The <see cref="T:HarmonyLib.MethodType" /></param>
        /// <param name="argumentTypes">An array of argument types to target overloads</param>
        /// <param name="argumentVariations">An array of <see cref="T:HarmonyLib.ArgumentType" /></param>
        public EarlyPatchAttribute(
            MethodType methodType,
            Type[] argumentTypes,
            ArgumentType[] argumentVariations)
        {
            info.methodType = methodType;
            ParseSpecialArguments(argumentTypes, argumentVariations);
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="argumentTypes">An array of argument types to target overloads</param>
        public EarlyPatchAttribute(Type[] argumentTypes)
        {
            info.argumentTypes = argumentTypes;
        }

        /// <summary>An annotation that specifies a method, property or constructor to patch</summary>
        /// <param name="argumentTypes">An array of argument types to target overloads</param>
        /// <param name="argumentVariations">An array of <see cref="T:HarmonyLib.ArgumentType" /></param>
        public EarlyPatchAttribute(Type[] argumentTypes, ArgumentType[] argumentVariations)
        {
            ParseSpecialArguments(argumentTypes, argumentVariations);
        }

        private void ParseSpecialArguments(Type[] argumentTypes, ArgumentType[]? argumentVariations)
        {
            if (argumentVariations == null || argumentVariations.Length == 0)
            {
                info.argumentTypes = argumentTypes;
            }
            else
            {
                if (argumentTypes.Length < argumentVariations.Length)
                    throw new ArgumentException("argumentVariations contains more elements than argumentTypes", nameof(argumentVariations));
                List<Type> typeList = new List<Type>();
                for (var index = 0; index < argumentTypes.Length; ++index)
                {
                    Type type = argumentTypes[index];
                    switch (argumentVariations[index])
                    {
                        case ArgumentType.Ref:
                        case ArgumentType.Out:
                            type = type.MakeByRefType();
                            break;
                        case ArgumentType.Pointer:
                            type = type.MakePointerType();
                            break;
                        case ArgumentType.Normal:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    typeList.Add(type);
                }

                info.argumentTypes = typeList.ToArray();
            }
        }
    }
}