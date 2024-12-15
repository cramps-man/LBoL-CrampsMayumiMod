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
    public sealed class CavalryRushDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(CavalryRush);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 1;
            cardConfig.Value1 = 12;
            cardConfig.UpgradedValue1 = 10;
            cardConfig.Mana = new ManaGroup() { Red = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CavalryRushDef))]
    public sealed class CavalryRush : ModAssignCard
    {
        public override int CavalryAssigned => 2;
        public override int StartingCardCounter => IsUpgraded ? 2 : 4;
        public override int StartingTaskLevel => IsUpgraded ? 15 : 9;
        public override Type AssignStatusType => typeof(AssignCavalryRush);
    }
}
