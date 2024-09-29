using HarmonyLib;
using LBoL.Core.Units;
using LBoLMod.GameEvents;

namespace LBoLMod.Patches
{
    [HarmonyPatch]
    internal class AddingGameEventsPatch
    {
        [HarmonyPatch(typeof(Unit), "OnEnterGameRun")]
        private static void Postfix(Unit __instance)
        {
            ModGameEvents.Init();
        }
    }
}
