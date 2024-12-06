using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLMod.StatusEffects.Assign;
using LBoLMod.StatusEffects.Keywords;
using System;
using System.Collections.Generic;

namespace LBoLMod.Cards
{
    public sealed class ArcherPrepDebuffDef : ModCardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ArcherPrepDebuff);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = base.MakeConfig();
            cardConfig.Rarity = Rarity.Uncommon;
            cardConfig.Type = CardType.Attack;
            cardConfig.Colors = new List<ManaColor>() { ManaColor.Red };
            cardConfig.Damage = 12;
            cardConfig.UpgradedDamage = 15;
            cardConfig.Value1 = 1;
            cardConfig.UpgradedValue1 = 2;
            cardConfig.Value2 = 7;
            cardConfig.UpgradedValue2 = 5;
            cardConfig.RelativeKeyword = Keyword.Accuracy;
            cardConfig.UpgradedRelativeKeyword = Keyword.Accuracy;
            cardConfig.RelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Vulnerable) };
            cardConfig.UpgradedRelativeEffects = new List<string>() { nameof(Haniwa), nameof(Assign), nameof(Vulnerable) };
            return cardConfig;
        }
    }

    [EntityLogic(typeof(ArcherPrepDebuffDef))]
    public sealed class ArcherPrepDebuff : ModAssignCard
    {
        public override int ArcherAssigned => 2;
        public override int StartingCardCounter => Value2;
        public override Type AssignStatusType => typeof(AssignArcherPrepDebuff);
    }
}
