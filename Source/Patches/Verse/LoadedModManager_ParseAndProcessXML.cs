#nullable enable
using System;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    [EarlyPatch(typeof(LoadedModManager), nameof(LoadedModManager.ParseAndProcessXML))]
    public static class LoadedModManager_ParseAndProcessXML
    {
        private const string FileName = "AllDefs.xml";

        public static void Postfix(XmlDocument xmlDoc)
        {
            using var file = File.Open(FileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            using var writer = new XmlTextWriter(file, Encoding.UTF8)
            {
                Formatting = Formatting.Indented
            };

            xmlDoc.WriteContentTo(writer);
            writer.Flush();

            Debug.Log($"Saved all defs to {Path.Combine(Environment.CurrentDirectory, FileName)}");
        }
    }
}
