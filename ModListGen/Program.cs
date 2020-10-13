using System;

namespace ModListGen
{
    internal static class Program
    {
        private static void Main()
        {
            var modListXml = Get("ModList.xml Bronze Age File", @"C:\Games\RimWorld-dev\Mods\Bronze Age\Common\ModList.xml");
            var rwConfigDir = Get("RimWorld Config Dir", @"C:\Users\Andy\AppData\LocalLow\Ludeon Studios\RimWorld by Ludeon Studios");


        }

        private static string Get(string text, string defaultValue)
        {
            Console.Write($"{text} ({defaultValue}): ");
            var val = Console.ReadLine();
            return string.IsNullOrEmpty(val) ? defaultValue : val;
        }
    }
}
