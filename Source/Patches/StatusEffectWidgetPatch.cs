using LBoL.Presentation.UI.Widgets;
using LBoLMod.StatusEffects;
using UnityEngine;

namespace LBoLMod.Source.Patches
{
    [HarmonyLib.HarmonyPatch]
    internal class StatusEffectWidgetPatch
    {
        [HarmonyLib.HarmonyPatch(typeof(StatusEffectWidget), nameof(StatusEffectWidget.TextRefresh))]
        private static void Postfix(StatusEffectWidget __instance)
        {
            if (__instance._statusEffect is ModHaniwaStatusEffect modStanceStatusEffect)
            {
                /*var textTransform = __instance.CenterWorldPosition;
                if (modStanceStatusEffect.Preserved)
                {
                    __instance.upText.text = "KEEP";
                    __instance.upText.fontSize = 32;
                    __instance.upText.autoSizeTextContainer = true;
                    __instance.upText.enableAutoSizing = false;
                }
                __instance.downText.text = "Lvl" + __instance._statusEffect.Level;
                __instance.downText.fontSize = 32;
                __instance.downText.transform.localPosition = new Vector3(textTransform.x + 34, textTransform.y - 89, textTransform.z);
                __instance.downText.autoSizeTextContainer = true;
                __instance.downText.enableAutoSizing = false;*/
            }
            else if (__instance._statusEffect is ModAssignStatusEffect modAssignStatusEffect)
            {
                __instance.downText.text = "";
            }
        }
    }
}
