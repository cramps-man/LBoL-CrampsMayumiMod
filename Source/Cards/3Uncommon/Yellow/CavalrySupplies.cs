using LBoL.Base;
using LBoL.ConfigData;
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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Value1 = 6;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Value2 = 1;
            cardConfig.Mana = new ManaGroup() { White = 1, Red = 1 };
            cardConfig.UpgradedMana = new ManaGroup() { White = 1, Philosophy = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CavalrySuppliesDef))]
    public sealed class CavalrySupplies : ModAssignCard
    {
        public override int CavalryAssigned => 2;
        public override int StartingCardCounter => Value1;
        public override Type AssignStatusType => typeof(AssignCavalrySupplies);
    }
}
