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

        public static Func<S, T> CreateGetter<S, T>(this FieldInfo field)
        {
            string methodName = field.ReflectedType.FullName + ".get_" + field.Name;
            DynamicMethod setterMethod = new DynamicMethod(methodName, typeof(T), new Type[1] { typeof(S) }, true);
            ILGenerator gen = setterMethod.GetILGenerator();
            if (field.IsStatic)
            {
                gen.Emit(OpCodes.Ldsfld, field);
            }
            else
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);
            }
            gen.Emit(OpCodes.Ret);
            return (Func<S, T>)setterMethod.CreateDelegate(typeof(Func<S, T>));
        }

        public static Action<S, T> CreateSetter<S,T>(this FieldInfo field)
        {
            string methodName = field.ReflectedType.FullName+".set_"+field.Name;
            DynamicMethod setterMethod = new DynamicMethod(methodName, null, new Type[2]{typeof(S),typeof(T)},true);
            ILGenerator gen = setterMethod.GetILGenerator();
            if (field.IsStatic)
            {
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Stsfld, field);
            }
            else
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Stfld, field);
            }
            gen.Emit(OpCodes.Ret);
            return (Action<S, T>)setterMethod.CreateDelegate(typeof(Action<S, T>));
        }
    }
}
