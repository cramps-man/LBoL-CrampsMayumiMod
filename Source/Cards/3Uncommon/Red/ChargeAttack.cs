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
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 15;
            cardConfig.UpgradedDamage = 20;
            cardConfig.Value1 = 10;
            cardConfig.Keywords = Keyword.Retain;
            cardConfig.UpgradedKeywords = Keyword.Retain;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ChargeAttackDef))]
    public sealed class ChargeAttack : ModAssignCard
    {
        public override int ArcherAssigned => 3;
        public override int StartingCardCounter => 10;
        public override int StartingTaskLevel => IsUpgraded ? 20 : 10;
        public override Type AssignStatusType => typeof(AssignChargeAttack);
    }
}
