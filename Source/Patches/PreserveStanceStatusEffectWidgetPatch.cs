using LBoL.Presentation.UI.Widgets;
using LBoLMod.StatusEffects;

namespace LBoLMod.Source.Patches
{
    [HarmonyLib.HarmonyPatch]
    internal class PreserveStanceStatusEffectWidgetPatch
    {
        [HarmonyLib.HarmonyPatch(typeof(StatusEffectWidget), nameof(StatusEffectWidget.TextRefresh))]
        private static void Postfix(StatusEffectWidget __instance)
        {
            if (__instance._statusEffect is ModStanceStatusEffect modStanceStatusEffect)
            {
                if (modStanceStatusEffect.Preserved)
                {
                    __instance.downText.text = "P";
                }
                else
                {
                    __instance.downText.text = "";
                }
            }
        }
    }
}
