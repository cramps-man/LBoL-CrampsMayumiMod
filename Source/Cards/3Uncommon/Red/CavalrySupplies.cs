using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Assign;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class CavalrySuppliesDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CavalrySupplies);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Value1 = 25;
            cardConfig.Value2 = 2;
            cardConfig.UpgradedValue2 = 4;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.RelativeCards = new List<string>() { nameof(RManaCard), nameof(WManaCard), nameof(PManaCard), nameof(HaniwaAttacker) };
            cardConfig.UpgradedRelativeCards = new List<string>() { nameof(RManaCard), nameof(WManaCard), nameof(PManaCard), nameof(HaniwaAttacker) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CavalrySuppliesDef))]
    public sealed class CavalrySupplies : ModAssignCard
    {
        public override int CavalryAssigned => 3;
        public override int StartingCardCounter => IsUpgraded ? 5 : 8;
        public override int StartingTaskLevel => IsUpgraded ? 30 : 15;
        public override Type AssignStatusType => typeof(AssignCavalrySupplies);
    }
}
