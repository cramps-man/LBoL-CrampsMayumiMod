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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Damage = 15;
            cardConfig.UpgradedDamage = 21;
            cardConfig.Value1 = 4;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Mana = new ManaGroup() { White = 1 };
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(CavalryRushDef))]
    public sealed class CavalryRush : ModAssignCard
    {
        public override int CavalryAssigned => 2;
        public override int StartingCardCounter => Value1;
        public override Type AssignStatusType => typeof(AssignCavalryRush);
    }
}
