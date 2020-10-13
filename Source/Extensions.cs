#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace ABrenneke.BronzeAge
{
    public static class Extensions
    {
        public static IEnumerable<(Type, T)> GetTypesWithAttribute<T>() where T : Attribute
        {
            return from type in Assembly.GetExecutingAssembly().GetTypes()
                let attribute = type.GetCustomAttribute<T>()
                where attribute != null
                select (type, attribute);
        }

        public static void PatchMod(this Harmony harmony, string modPackageId, string typeName, string methodName, Type patchType)
        {
            try
            {
                ((Action) (() =>
                {
                    if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId == modPackageId))
                    {
                        var modType = AccessTools.TypeByName(typeName);

                        if (modType == null)
                        {
                            Log.Warning($"[BronzeAge] Could not find {typeName} in mod {modPackageId}, but mod should be loaded.");
                            return;
                        }

                        var method = AccessTools.Method(modType, methodName);

                        if (method != null)
                        {
                            var postfix = patchType.GetAnyMethod("Postfix") != null ? new HarmonyMethod(patchType, "Postfix") : null; 
                            var prefix = patchType.GetAnyMethod("Prefix") != null ? new HarmonyMethod(patchType, "Prefix") : null; 
                            var transpiler = patchType.GetAnyMethod("Transpiler") != null ? new HarmonyMethod(patchType, "Transpiler") : null; 
                            var finalizer = patchType.GetAnyMethod("Finalizer") != null ? new HarmonyMethod(patchType, "Finalizer") : null; 

                            harmony.Patch(method, prefix, postfix, transpiler, finalizer);
                        }
                        else if (BronzeAgeMod.Debug)
                        {
                            Log.Warning($"[BronzeAge] Could not find {typeName}.{methodName} in mod {modPackageId}, but mod should be loaded.");
                        }
                    }
                }))();
            }
            catch (TypeLoadException ex)
            {
                if (BronzeAgeMod.Debug)
                {
                    Log.Message(ex.ToStringSafe());
                }
            }
        }

        public static MethodInfo GetAnyMethod(this Type type, string name)
        {
            return type.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static T GetFieldValue<T>(this Type type, object? instance, string name)
        {
            var field = type.GetField(name, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return (T) field.GetValue(instance);
        }

        public static T GetPrivate<T>(this object obj, string name)
        {
            return obj.GetType().GetFieldValue<T>(obj, name);
        }

        // https://stackoverflow.com/a/55794983/365416
        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out IEnumerable<T> rest)
        {
            var enumerable = seq as T[] ?? seq.ToArray();
            first = enumerable.FirstOrDefault();
            rest = enumerable.Skip(1);
        }

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out IEnumerable<T> rest)
            => (first, (second, rest)) = seq;

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out IEnumerable<T> rest)
            => (first, second, (third, rest)) = seq;

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out T fourth, out IEnumerable<T> rest)
            => (first, second, third, (fourth, rest)) = seq;

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out T fourth, out T fifth, out IEnumerable<T> rest)
            => (first, second, third, fourth, (fifth, rest)) = seq;

        public static IEnumerable<CodeInstruction> ReplaceInstructions(this IEnumerable<CodeInstruction> input, IList<Tuple<OpCode, object>> findInstructions, IEnumerable<CodeInstruction> replacement)
        {
            var inputList = input.ToList();
            var replacementInstructions = replacement.ToList();
            for (var i = 0; i < inputList.Count; i++)
            {
                var inInstruction = inputList[i];
                if (inputList.HasSequence(i, findInstructions))
                {
                    foreach (var replacementInstruction in replacementInstructions)
                    {
                        yield return replacementInstruction;
                    }

                    i += findInstructions.Count;
                }
                else
                {
                    yield return inInstruction;
                }
            }
        }

        public static bool HasSequence(this IList<CodeInstruction> instructions, int startPosition, IList<Tuple<OpCode, object>> findInstructions)
        {
            if (instructions.Count - startPosition < findInstructions.Count)
                return false;

            for (int i = startPosition, j = 0; i < instructions.Count; i++, j++)
            {
                var (opCode, operand) = findInstructions[j];
                if (!instructions[i].Is(opCode, operand))
                    return false;
            }

            return true;
        }
    }
}
