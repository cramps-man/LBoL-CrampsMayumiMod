using HarmonyLib;
using LBoL.Presentation.UI.Widgets;
using LBoLMod.Cards;

namespace LBoLMod.Patches
{
    [HarmonyPatch(typeof(CardWidget), nameof(CardWidget.SetProperties))]
    class ModFrontlineCardWidgetPatch
    {
        static void Postfix(CardWidget __instance)
        {
            var widget = __instance;
            if (widget._card is ModFrontlineCard frontlineCard)
            {
                widget.baseLoyaltyObj.SetActive(true);
                if (frontlineCard.IsDarknessMode)
                    widget.baseLoyalty.text = "∞";
                else
                    widget.baseLoyalty.text = frontlineCard.RemainingValue.ToString();
            }
        }
    }
}
