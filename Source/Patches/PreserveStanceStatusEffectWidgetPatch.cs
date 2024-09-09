using LBoL.Presentation.UI.Widgets;
using LBoLMod.StatusEffects;
using UnityEngine;

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
                    //var textTransform = __instance.CenterWorldPosition;
                    __instance.upText.text = "KEEP";
                    __instance.downText.text = "KEEP";
                    //__instance.upText.fontSize = 30;
                    //__instance.upText.transform.localPosition = new Vector3 (textTransform.x + 38, textTransform.y, textTransform.z);
                    //__instance.upText.autoSizeTextContainer = true;
                    //__instance.upText.enableAutoSizing = false;
                }
                /*var textTransform = __instance.CenterWorldPosition;
                __instance.downText.text = "Lvl" + __instance._statusEffect.Level;
                __instance.downText.fontSize = 32;
                __instance.downText.transform.localPosition = new Vector3(textTransform.x + 38, textTransform.y - 89, textTransform.z);
                __instance.downText.autoSizeTextContainer = true;
                __instance.downText.enableAutoSizing = false;*/
            }
        }
    }
}
