using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using JetBrains.Annotations;

namespace ModListGen
{
    internal static class Program
    {
        private static readonly IDictionary<string, ModInfo> Mods = new Dictionary<string, ModInfo>();
        private static readonly IList<ModInfo> ModsList = new List<ModInfo>();

        private static void Main()
        {
            try
            {
                var modListXml = Get("ModList.xml Bronze Age File", @"C:\Games\RimWorld-dev\Mods\Bronze Age\Common\ModList.xml");
                var rwConfigDir = Get("RimWorld Config Dir", @"C:\Users\Andy\AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios");
                var steamDir = Get("Steam Directory", @"C:\Steam");
                var rwInstallDir = Get("RimWorld Install Directory", @"C:\Steam\steamapps\common\RimWorld");

                var errors = new List<string>();
                if (!new FileInfo(modListXml).Exists)
                    errors.Add($"ModList.xml not found at {modListXml}.");
                if (!new DirectoryInfo(rwConfigDir).Exists)
                    errors.Add($"RimWorld config directory not found at {rwConfigDir}.");
                if (!new DirectoryInfo(steamDir).Exists)
                    errors.Add($"Steam directory not found at {steamDir}.");
                if (!new DirectoryInfo(rwInstallDir).Exists)
                    errors.Add($"RimWorld install directory not found at {rwInstallDir}.");

                if (errors.Any())
                {
                    Console.WriteLine(string.Join('\n', errors));
                    return;
                }

                LoadModsMeta(steamDir, rwInstallDir);
                LoadModsConfig(rwConfigDir);
                LoadExistingFile(modListXml);
                UpdateFile(modListXml);

                Console.WriteLine("Complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        private static void UpdateFile(string modListXml)
        {
            using var file = File.Open(modListXml, FileMode.Open, FileAccess.ReadWrite);
            var doc = XDocument.Load(file);
            var modsElement = doc.XPathSelectElement("ModsConfig/Mods") ?? throw new InvalidOperationException();

            var newModsElement = new XElement("Mods");
            foreach (var mod in ModsList)
            {
                var element = new XElement("li", 
                    new XElement("name", mod.Name),
                    new XElement("id", mod.PackageId),
                    new XElement("workshopUrl", $"https://steamcommunity.com/sharedfiles/filedetails/?id={mod.SteamId}"),
                    new XElement("type", mod.Type.ToString()));
                newModsElement.Add(element);
            }

            modsElement.ReplaceWith(newModsElement);

            file.Seek(0, SeekOrigin.Begin);
            doc.Save(file);
        }

        private static void LoadExistingFile(string modListXml)
        {
            using var file = File.OpenRead(modListXml);
            var doc = XDocument.Load(file);
            foreach (var li in doc.XPathSelectElements("ModsConfig/Mods/li"))
            {
                var packageId = li.Element("id")?.Value;
                if (!string.IsNullOrEmpty(packageId) && Mods.TryGetValue(packageId, out var mod))
                {
                    var type = li.Element("type")?.Value;
                    if (string.IsNullOrWhiteSpace(type))
                        type = ModType.Recommended.ToString();
                    
                    mod.Type = Enum.Parse<ModType>(type);
                }
            }
        }

        private static void LoadModsMeta(string steamDir, string rwInstallDir)
        {
            var i = 0;

            // Load mods from steam workshop
            foreach (var folder in Directory.EnumerateDirectories(Path.Combine(steamDir, "steamapps", "workshop", "content", "294100")))
            {
                Console.Write($"\rMods found: {i++}");
                LoadModFolder(folder);
            }

            // Load mods from game dir
            foreach (var folder in Directory.EnumerateDirectories(Path.Combine(rwInstallDir, "Mods")))
            {
                Console.Write($"\rMods found: {i++}");
                LoadModFolder(folder);
            }

            Console.Write($"\rFound {Mods.Count} mods in total. Loading configuration...\r\n");
        }

        private static void LoadModsConfig(string rwConfigDir)
        {
            var configInfo = new FileInfo(Path.Combine(rwConfigDir, "Config", "ModsConfig.xml"));
            if (!configInfo.Exists)
                throw new InvalidOperationException("Could not find ModsConfig.xml file.");

            using var config = File.OpenRead(configInfo.FullName);
            var configDoc = XDocument.Load(config);
            var expansions = configDoc.XPathSelectElements("ModsConfigData/knownExpansions/*").ToList();

            foreach (var li in configDoc.XPathSelectElements("ModsConfigData/activeMods/*"))
            {
                if (Mods.TryGetValue(li.Value.ToLowerInvariant(), out var mod))
                {
                    ModsList.Add(mod);
                }
                else if (expansions.Any(e => e.Value == li.Value))
                {
                    ModsList.Add(new ModInfo
                    {
                        PackageId = li.Value,
                        Name = li.Value,
                        Url = "N/A",
                        SteamId = "N/A",
                        Type = ModType.Required
                    });
                }
                else
                {
                    Console.WriteLine($"Mod {li.Value} is defined in ModsConfig.xml but was unable to find a mod with this package ID!");
                    ModsList.Add(new ModInfo
                    {
                        PackageId = li.Value
                    });
                }
            }

            Console.WriteLine($"Found {ModsList.Count} mods configured to be loaded.");
        }

        private static void LoadModFolder(string folder)
        {
            try
            {
                var mod = new ModInfo
                {
                    Folder = new DirectoryInfo(folder)
                };

                var aboutInfo = new FileInfo(Path.Combine(folder, "About", "About.xml"));
                if (!aboutInfo.Exists)
                    throw new InvalidOperationException($"Could not find About.xml for steam folder {folder}. Skipping.");

                using var about = File.OpenRead(aboutInfo.FullName);
                var metaDoc = XDocument.Load(about);
                var meta = metaDoc.Element("ModMetaData") ?? metaDoc.Element("ModMetadata");

                mod.Name = meta?.Element("name")?.Value;
                mod.PackageId = meta?.Element("packageId")?.Value.ToLowerInvariant();
                mod.Url = meta?.Element("url")?.Value;

                try
                {
                    using var publishedIdFile = new StreamReader(File.OpenRead(Path.Combine(folder, "About", "PublishedFileId.txt")));
                    mod.SteamId = publishedIdFile.ReadToEnd();
                }
                catch (IOException)
                {
                    // ignored
                }

                if (mod.PackageId == null)
                    throw new InvalidOperationException($"Could not find package ID for {mod.Name ?? folder}. Skipping.");

                Mods[mod.PackageId] = mod;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\rError: {ex.Message}");
            }
        }

        private static string Get(string text, string defaultValue)
        {
            Console.Write($"{text} ({defaultValue}): ");
            var val = Console.ReadLine();
            return string.IsNullOrEmpty(val) ? defaultValue : val;
        }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public enum ModType
    {
        Required,
        Core,
        Recommended,
        Optional
    }

    public class ModInfo
    {
        public string PackageId { get; set; } = "UNKNOWN";

        public string Name { get; set; } = "UNKNOWN";

        public string SteamId { get; set; } = "UNKNOWN";

        public DirectoryInfo Folder { get; set; }

        public string Url { get; set; } = "UNKNOWN";

        public ModType Type { get; set; } = ModType.Recommended;
    }
}
