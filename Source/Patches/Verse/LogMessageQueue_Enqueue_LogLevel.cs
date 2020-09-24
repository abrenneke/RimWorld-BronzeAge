using Verse;

namespace ABrenneke.BronzeAge.Patches.Verse
{
    [EarlyPatch(typeof(LogMessageQueue), nameof(LogMessageQueue.Enqueue))]
    public static class LogMessageQueue_Enqueue_LogLevel
    {
        public static bool Prefix(LogMessage msg)
        {
            var hideMessage = BronzeAgeMod.Instance.LogLevel?.Value ?? false;
            return !hideMessage || msg.type != LogMessageType.Message;
        }
    }
}
