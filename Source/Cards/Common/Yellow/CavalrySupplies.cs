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
            cardConfig.Type = CardType.Skill;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Cost = new ManaGroup() { Any = 0 };
            cardConfig.Value1 = 8;
            cardConfig.UpgradedValue1 = 3;
            cardConfig.Mana = new ManaGroup() { White = 1 };
            cardConfig.UpgradedMana = new ManaGroup() { Philosophy = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CavalrySuppliesDef))]
    public sealed class CavalrySupplies : ModAssignCard
    {
        public override int CavalryAssigned => 1;
        public override int StartingCardCounter => Value1;
        public override Type AssignStatusType => typeof(AssignCavalrySupplies);
    }
}
