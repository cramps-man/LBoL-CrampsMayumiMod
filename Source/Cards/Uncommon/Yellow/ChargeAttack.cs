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
    public sealed class ChargeAttackDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ChargeAttack);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.White };
            cardConfig.Damage = 15;
            cardConfig.Value1 = 7;
            cardConfig.UpgradedValue1 = 10;
            cardConfig.Value2 = 10;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ChargeAttackDef))]
    public sealed class ChargeAttack : ModAssignCard
    {
        public override int ArcherAssigned => 3;
        public override int StartingCardCounter => Value2;
        public override Type AssignStatusType => typeof(AssignChargeAttack);
    }
}
