using System.ComponentModel;

namespace GameBrains.EventSystem
{
    // Add our own event types
    public static partial class Events
    {
        [Description("StewReady")] public static readonly EventType StewReady = (EventType)Count++;
        [Description("HiHoneyImHome")] public static readonly EventType HiHoneyImHome = (EventType)Count++;
    }
}