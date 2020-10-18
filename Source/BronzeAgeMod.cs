#nullable enable
using System.Collections.Generic;
using HarmonyLib;
using HugsLib;
using HugsLib.Settings;
using JetBrains.Annotations;
using Verse;

namespace ABrenneke.BronzeAge
{
    [EarlyInit, PublicAPI]
    public class BronzeAgeMod : ModBase
    {
        public static BronzeAgeMod Instance { get; private set; } = null!;

        public static readonly bool Debug = true;

        public sealed override string LogIdentifier => "abrenneke.bronzeage";

        protected override bool HarmonyAutoPatch => false;

        public SettingHandle<bool>? LogLevel { get; private set; }

        private static readonly Dictionary<string, bool> ModsLoadedByName = new Dictionary<string, bool>();
        private static readonly Dictionary<string, bool> ModsLoadedById = new Dictionary<string, bool>();

        // HugsLib Documentation: https://github.com/UnlimitedHugs/RimworldHugsLib/wiki

        public BronzeAgeMod()
        {
            HarmonyInst = new Harmony(LogIdentifier);
            Instance = this;
        }

        public override void EarlyInitialize()
        {
            // Patches to run as early as possible
            foreach (var (type, _) in Extensions.GetTypesWithAttribute<EarlyPatchAttribute>())
            {
                HarmonyInst.CreateClassProcessor(type).Patch();
            }

            LogLevel = Settings.GetHandle<bool>("hideMessageLogs", "Hide Message Logs", "Hide non-warning log messages");
        }

        public override void StaticInitialize()
        {
            foreach (var (type, _) in Extensions.GetTypesWithAttribute<HarmonyPatch>())
            {
                HarmonyInst.CreateClassProcessor(type).Patch();
            }

            // Patch mods if they're loaded
            foreach (var (type, attribute) in Extensions.GetTypesWithAttribute<ModPatchAttribute>())
            {
                HarmonyInst.PatchMod(attribute.ModPackageId, attribute.ModTypeName, attribute.ModMethodName, type);
            }

            // Patch things that rely on mods, if the mod is loaded
            foreach (var (type, attribute) in Extensions.GetTypesWithAttribute<ConditionalModPatchAttribute>())
            {
                if (ModIsRunning(modPackageIdMatch: attribute.ModPackageId))
                {
                    HarmonyInst.CreateClassProcessor(type).Patch();
                }
            }
        }

        public static bool ModIsRunning(string? modNameMatch = null, string? modPackageIdMatch = null)
        {
            return ModIsRunningName(modNameMatch) || ModIsRunningId(modPackageIdMatch);
        }

        private static bool ModIsRunningName(string? modNameMatch)
        {
            if (modNameMatch is null)
                return false;

            if (ModsLoadedByName.TryGetValue(modNameMatch, out var loaded))
                return loaded;

            loaded = LoadedModManager.RunningModsListForReading.Any(m => m.Name.ToLowerInvariant().Contains(modNameMatch.ToLowerInvariant()));
            ModsLoadedByName[modNameMatch] = loaded;
            return loaded;
        }

        private static bool ModIsRunningId(string? modPackageIdMatch)
        {
            if (modPackageIdMatch is null)
                return false;

            if (ModsLoadedById.TryGetValue(modPackageIdMatch, out var loaded) && loaded)
                return loaded;

            loaded = LoadedModManager.RunningModsListForReading.Any(m => m.PackageId.ToLowerInvariant().Contains(modPackageIdMatch.ToLowerInvariant()));
            ModsLoadedById[modPackageIdMatch] = loaded;
            return loaded;
        }
    }
}
