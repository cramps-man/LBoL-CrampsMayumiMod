using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Widgets;
using LBoLMod.Cards;
using UnityEngine;

namespace LBoLMod.Source.Patches
{
    [HarmonyPatch(typeof(CardWidget), nameof(CardWidget.OnActivating))]
    class CardWidget_Patch
    {
        static void Postfix(CardWidget __instance)
        {
            var widget = __instance;
            if (widget._card.Id == nameof(HaniwaBodyguard))
            {
                var card = (HaniwaBodyguard)widget._card;
                PopupHud.Instance.PopupFromScene(card.DamageTaken, PopupHud.PlayerHitColor, widget.transform.position + new Vector3(0, 13));
            }
        }
    }
}
