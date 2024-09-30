using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Units;
using LBoLMod.GameEvents;

namespace LBoLMod.Patches
{
    [HarmonyPatch]
    internal class AddingGameEventsPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterBattle))]
        private static void Prefix(Unit __instance)
        {
            ModGameEvents.Init();
        }
    }
}
