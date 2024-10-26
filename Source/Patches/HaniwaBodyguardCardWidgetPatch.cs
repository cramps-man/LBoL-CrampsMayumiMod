using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Widgets;
using LBoLMod.Cards;
using UnityEngine;

namespace LBoLMod.Patches
{
    [HarmonyPatch(typeof(CardWidget), nameof(CardWidget.OnActivating))]
    class HaniwaBodyguardCardWidgetPatch
    {
        static void Postfix(CardWidget __instance)
        {
            var widget = __instance;
            if (widget._card.Id == nameof(HaniwaBodyguard))
            {
                var card = (HaniwaBodyguard)widget._card;
                if (card.DamageTaken > 0)
                    PopupHud.Instance.PopupFromScene(card.DamageTaken, PopupHud.PlayerHitColor, widget.transform.position + new Vector3(0, 13));
            }
        }
    }
}
