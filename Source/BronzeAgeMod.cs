#nullable enable
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

        public SettingHandle<bool>? LogLevel { get; set; }

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
                if (LoadedModManager.RunningModsListForReading.Any(x => x.PackageId == attribute.ModPackageId))
                {
                    HarmonyInst.CreateClassProcessor(type).Patch();
                }
            }
        }
    }
}
