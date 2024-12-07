using HarmonyLib;

namespace LBoLMod
{
    public static class PInfo
    {
        // each loaded plugin needs to have a unique GUID. usually author+generalCategory+Name is good enough
        public const string GUID = "cramps-firstmod";
        public const string Name = "Mayumi Mod";
        public const string version = "0.3.0";
        public static readonly Harmony harmony = new Harmony(GUID);

    }
}
